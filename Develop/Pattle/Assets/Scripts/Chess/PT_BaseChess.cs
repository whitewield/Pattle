using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Pattle.Global;

public class PT_BaseChess : NetworkBehaviour {
	[SyncVar] protected int myOwnerID = -1;
	[SyncVar] protected int myID = -1;
	private PT_PlayerController myPlayerController;

	//Process
	[SerializeField] GameObject myProcessDisplayPrefab;
	protected PT_ProcessDisplay myProcessDisplay;
	protected Process myProcess = Process.None;
	protected Process myQueueProcess = Process.None;
	protected Process myLastProcess = Process.None;

	//Attributes
	[SerializeField] protected SO_Attributes myAttributes;
	[SyncVar] int myCurHP;
	protected float myTimer;//timer to count the time in different Process

	//Status
	protected List<float> myStatus = new List<float> ();

	//For Action
	protected Vector2 myPosition;
	protected bool isSingleTarget = false;
	protected GameObject myTargetGameObject;
	protected Vector2 myTargetPosition;

	//my stuff
	protected CircleCollider2D myCollider;
	protected SpriteRenderer mySpriteRenderer;

	// Use this for initialization
	void Start () {
		Initialize ();
	}

	private void Initialize() {
		myCollider = this.GetComponent<CircleCollider2D> ();
		myCollider.isTrigger = true;
		mySpriteRenderer = this.GetComponent<SpriteRenderer> ();

		SetProcess (Process.Idle);
		myProcessDisplay = Instantiate (myProcessDisplayPrefab, this.transform).GetComponent<PT_ProcessDisplay> ();
		myProcessDisplay.transform.localPosition = Vector3.zero;
		myProcessDisplay.HideProcess ();

		myCurHP = myAttributes.HP;
		myProcessDisplay.ShowHP (myCurHP);

		for (int i = 0; i < (int)Status.End; i++) {
			myStatus.Add (0f);
		}

		//CD at beginning
		SetProcess (Process.CD);
		myTimer = myAttributes.CD;
		myProcessDisplay.ShowCD (myTimer);

		CustomInitialize ();
	}

	protected virtual void CustomInitialize () {

	}

	#region Action
	public virtual bool Action (GameObject g_target, Vector2 g_targetPos) {
		
		if (myProcess != Process.Idle && 
			myProcess != Process.CD) {
			return false;
		}

		return IndividualAction (g_target, g_targetPos);
	}

	/// <summary>
	/// if can take action, return true
	/// if can not take action, return false
	/// </summary>
	/// <returns><c>true</c>, if can take action, <c>false</c> if can not take action.</returns>
	/// <param name="g_target">target gameobject.</param>
	/// <param name="g_targetPos">target position.</param>
	protected virtual bool IndividualAction (GameObject g_target, Vector2 g_targetPos) {
		if (g_target.name == (Constants.NAME_MAP_FIELD + myOwnerID.ToString ())) {
			myTargetPosition = g_targetPos;
			QueueMove ();
			return true;
		} else if (g_target.name == (Constants.NAME_MAP_FIELD + (1 - myOwnerID).ToString ()) ||
			(g_target.GetComponent<PT_BaseChess> () && g_target.GetComponent<PT_BaseChess> ().GetMyOwnerID () != myOwnerID)) {
			myTargetPosition = g_targetPos;
			myPosition = this.transform.position;
			QueueAttack ();
			return true;
		}
		QueueIdle ();
		return false;
	}

	public void QueueIdle () {
		myQueueProcess = Process.Idle;
	}

	public void QueueMove () {
		myQueueProcess = Process.Move;

		if (myPlayerController != null)
			myPlayerController.RpcShowTarget (myID, -1, -1, myTargetPosition);
	}

	public void QueueAttack () {
		myQueueProcess = Process.Attack;

		if (myPlayerController != null)
		if (isSingleTarget) {
			PT_BaseChess t_baseChess = myTargetGameObject.GetComponent<PT_BaseChess> ();
			myPlayerController.RpcShowTarget (myID, t_baseChess.GetMyOwnerID (), t_baseChess.GetMyID (), myTargetPosition);
		} else {
			myPlayerController.RpcShowTarget (myID, -1, -1, myTargetPosition);
		}
	}

	public void QueueCast () {
		myQueueProcess = Process.CT;

		if (myPlayerController != null)
		if (isSingleTarget) {
			PT_BaseChess t_baseChess = myTargetGameObject.GetComponent<PT_BaseChess> ();
			myPlayerController.RpcShowTarget (myID, t_baseChess.GetMyOwnerID (), t_baseChess.GetMyID (), myTargetPosition);
		} else {
			myPlayerController.RpcShowTarget (myID, -1, -1, myTargetPosition);
		}
	}

	protected virtual void Idle () {
		SetProcess (Process.Idle);
	}

	protected virtual void Move () {
		SetProcess (Process.Move);
	}

	protected virtual void CoolDown (float g_scale = 1) {
		SetProcess (Process.CD);
		myTimer = myAttributes.CD * g_scale;
		RpcShowCD (myTimer);

		if (myPlayerController != null)
			myPlayerController.RpcHideTarget (myID);
	}

	protected virtual void Cast (float g_scale = 1) {
		SetProcess (Process.CT);
		myTimer = myAttributes.CT * g_scale;
		RpcShowCT (myTimer);
	}

	protected virtual void Attack () {
		SetProcess (Process.Attack);
	}

	protected void AttackBack () {
		SetProcess (Process.AttackBack);

		if (myPlayerController != null)
			myPlayerController.RpcHideTarget (myID);
	}

	protected void SetProcess (Process g_process) {
		myProcess = g_process;
		//update the collider, if attack or attack back, the collider is trigger, else is not trigger
		if (isServer) {
			if (myProcess == Process.Move ||
			    myProcess == Process.Attack ||
			    myProcess == Process.AttackBack)
				myCollider.isTrigger = true;
			else
				myCollider.isTrigger = false;
		}
	}

	public Process GetProcess () {
		return myProcess;
	}

	#endregion

	#region Update
	void Update () {
		//keep the rotation of chess, may be used in fish boss and PVP (Arena)
		this.transform.rotation = Camera.main.transform.rotation;

		if (!isServer)
			return;
		
		UpdateStatus ();

		if (GetStatus (Status.Freeze) || GetStatus (Status.Gold)) {
			return;
		}

		UpdateAction ();
	}

	protected virtual void UpdateAction () {

		switch (myProcess) {
		case Process.Idle:
			UpdateAction_Idle ();
			break;
		case Process.Move:
			UpdateAction_Move ();
			break;
		case Process.CD:
			UpdateAction_CD ();
			break;
		case Process.CT:
			UpdateAction_CT ();
			break;
		case Process.Attack:
			UpdateAction_Attack ();
			break;
		case Process.AttackBack:
			UpdateAction_AttackBack ();
			break;
		}

		//update Timer
		if (myTimer > 0)
			myTimer -= Time.deltaTime;
//		Debug.Log ("timer: " + myTimer);
	}

	protected virtual void UpdateAction_Idle () {
		if (myQueueProcess == Process.Move) {
			Move ();
			//play SFX move 
//			PlayMySFX (myMoveSFX);
		} else if (myQueueProcess == Process.Attack) {
			Attack ();
			//play SFX ATK 
//			PlayMySFX (myAttackSFX);
		} else if (myQueueProcess == Process.CT) {
			Cast ();
			//play SFX CT 
//			PlayMySFX (myCastSFX);
		}
		myQueueProcess = Process.None;
	}

	protected virtual void UpdateAction_Move () {
		//move the chess
		this.transform.position = 
			Vector2.Lerp (this.transform.position, myTargetPosition, Constants.SPEED_MOVE * Time.deltaTime);

		//if the chess at the target, stop
		if (Vector2.Distance (this.transform.position, myTargetPosition) <= Constants.DISTANCE_RESET) {
			//set my position
			this.transform.position = myTargetPosition;
			myPosition = myTargetPosition;

			//start cool down
			CoolDown (0.5f);
		}
	}

	protected virtual void UpdateAction_CD () {
		if (myTimer <= 0)
			Idle ();
	}

	protected virtual void UpdateAction_CT () {
		if (myTimer <= 0)
			Attack ();
	}

	public virtual void UpdateAction_Attack () {
		//different in different character
		//move the chess
		this.transform.position = 
			Vector2.Lerp (this.transform.position, myTargetPosition, Constants.SPEED_MOVE * Time.deltaTime);

		//if the chess at the target, stop
		if (Vector2.Distance (this.transform.position, myTargetPosition) <= Constants.DISTANCE_RESET) {
			//Reset and come back
			AttackBack ();
		}
	}

	public virtual void UpdateAction_AttackBack () {
		//move the chess
		this.transform.position = 
			Vector2.Lerp (this.transform.position, myPosition, Constants.SPEED_MOVE * Time.deltaTime);

		//if the chess at the target, stop
		if (Vector2.Distance (this.transform.position, myPosition) <= Constants.DISTANCE_RESET) {
			//set my position
			this.transform.position = myPosition;

			//start cool down
			CoolDown ();
		}
	}

	#endregion

	#region Status
	public void SetStatus (Status g_status, float g_time) {
		if (GetStatus (Status.Gold))
			return;

		if ((GetStatus (Status.SpellImmune) || GetStatus (Status.Bubble)) &&
		    g_status == Status.Freeze)
			return;

		myStatus [(int)g_status] = Mathf.Max (myStatus [(int)g_status], g_time);

		if (g_status == Status.Freeze) {
			RpcShowStatus_Freeze (myStatus [(int)g_status]);
		}
		if (g_status == Status.Gold) {
			RpcShowStatus_Gold (myStatus [(int)g_status]);
		}

		OnSetStatus ();
	}

	protected virtual void OnSetStatus () {

	}

	public bool GetStatus (Status g_status) {
		if (myStatus [(int)g_status] > 0) {
			return true;
		}
		return false;
	}

	private void UpdateStatus () {
		for (int i = 0; i < myStatus.Count; i++) {
			if (myStatus [i] > 0)
				myStatus [i] -= Time.deltaTime;
		}
	}

	/// <summary>
	/// Used by soprano
	/// </summary>
	public void BeReady () {
		if (myPlayerController != null)
			myPlayerController.RpcHideTarget (myID);
		RpcShowIdle ();

		Idle ();
	}

	#endregion

	#region Collision
	void OnTriggerEnter2D (Collider2D g_collider2D) {
		CollisionAction (g_collider2D.gameObject);
	}

	void OnCollisionEnter2D (Collision2D g_ollision2D) {
		CollisionAction (g_ollision2D.gameObject);
	}

	protected virtual void CollisionAction(GameObject g_GO_Collision) {
		if (!isServer)
			return;
		if (myProcess == Process.Dead)
			return;

		//need to be rewrite in different chess
	}
	#endregion

	#region Damage
	public virtual bool HPModify (HPModifierType g_type, int g_value) {
		if (myProcess == Process.Dead)
			return false;

		//gold prevent damage
//		if (st_Gold >= 0)
//			return;

		int t_modifier = g_value;

		if (g_type == HPModifierType.PhysicalDamage) {
			t_modifier -= myAttributes.PR;
			if (t_modifier < 1)
				t_modifier = 1;
		}

		if (t_modifier <= 0)
			return false;

		switch (g_type) {
		case HPModifierType.PhysicalDamage:
			HPModify_PhysicalDamage (t_modifier);
			break;
		case HPModifierType.MagicDamage:
			HPModify_MagicDamage (t_modifier);
			break;
		case HPModifierType.Healing:
			HPModify_Healing (t_modifier);
			break;
		}

		return true;
		//play SFX damage 
//		PlayMySFX (myDamageSFX);
	}

	protected virtual void DoOnDamage () {

	}

	protected virtual void HPModify_PhysicalDamage (int g_value) {
		//used by chameleon, mushroom

		if (GetStatus (Status.Gold))
			return;

		myCurHP -= g_value;

		RpcShowDamage (g_value);

		DoOnDamage ();

		CheckIsDead ();
	}

	protected virtual void HPModify_MagicDamage (int g_value) {
		//used by chameleon

		if (GetStatus (Status.Gold) ||
		    GetStatus (Status.SpellImmune) ||
		    GetStatus (Status.Bubble))
			return;

		myCurHP -= g_value;

		RpcShowDamage (g_value);

		DoOnDamage ();

		CheckIsDead ();
	}

	protected virtual void HPModify_Healing (int g_value) {

		myCurHP += g_value;

		if (myCurHP > myAttributes.HP) {
			myCurHP = myAttributes.HP;
		}

		RpcShowHealing (g_value);

		RpcShowHP (myCurHP);
	}

	protected void CheckIsDead () {
		if (myProcess == Process.Dead)
			return;

		if (myCurHP <= 0) {
			SetProcess (Process.Dead);

			DoOnDead ();
			myCurHP = 0;

			if (myPlayerController != null) {
				//remove the display
				myPlayerController.RpcHideTarget (myID);
			}
		}

		RpcShowHP (myCurHP);
	}

	public int GetCurHP () {
		return myCurHP;
	}

	protected virtual void DoOnDead () {
		if (myPlayerController != null) {
			//check if lose
			myPlayerController.CheckLose ();
		}
	}

	#endregion
		
	#region Network
	public int GetMyOwnerID () {
		return myOwnerID;
	}

	public void SetMyOwnerID (int g_ID) {
		myOwnerID = g_ID;
	}

	public void SetMyPlayerController (PT_PlayerController g_controller) {
		if (myPlayerController == null)
			myPlayerController = g_controller;
	}

	public void SetMyID (int g_ID) {
		myID = g_ID;
	}

	public int GetMyID () {
		return myID;
	}

	[ClientRpc]
	void RpcShowIdle () {
		myProcessDisplay.HideProcess ();
	}

	[ClientRpc]
	void RpcShowCD (float g_time) {
		//		Debug.Log ("RpcShowCD");
		myProcessDisplay.ShowCD (g_time);
	}
		
	[ClientRpc]
	void RpcShowCT (float g_time) {
		//		Debug.Log ("RpcShowCT");
		myProcessDisplay.ShowCT (g_time);
	}

	[ClientRpc]
	void RpcShowDamage (int g_value) {
		//		Debug.Log ("RpcShowCT");
		myProcessDisplay.ShowDamage (g_value);
	}

	[ClientRpc]
	void RpcShowHealing (int g_value) {
		//		Debug.Log ("RpcShowCT");
		myProcessDisplay.ShowHealing (g_value);
	}

	[ClientRpc]
	void RpcShowStatus_Freeze (float g_time) {
		//		Debug.Log ("RpcShowCT");
		myProcessDisplay.ShowStatus_Freeze (g_time);
	}

	[ClientRpc]
	void RpcShowStatus_Gold (float g_time) {
		//		Debug.Log ("RpcShowCT");
		myProcessDisplay.ShowStatus_Gold (g_time);
	}

	[ClientRpc]
	void RpcShowHP (int g_hp) {
		//		Debug.Log ("RpcShowCT");
		myProcessDisplay.ShowHP (g_hp);

		if (g_hp == 0) {
			//dead

			myProcessDisplay.HideHP ();
			myProcessDisplay.HideProcess ();
			mySpriteRenderer.sortingLayerName = Constants.SORTINGLAYER_DEADBODY;
			mySpriteRenderer.color = Constants.COLOR_DEADBODY;
			myCollider.enabled = false;
		}
	}
	#endregion

	public SO_Attributes GetAttributes () {
		return myAttributes;
	}
}


