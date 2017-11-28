using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PT_BaseChess : NetworkBehaviour {
	[SyncVar] protected int myOwnerID = -1;
	[SyncVar] protected int myID = -1;
	private PT_PlayerController myPlayerController;

	//Process
	[SerializeField] GameObject myProcessDisplayPrefab;
	protected PT_ProcessDisplay myProcessDisplay;
	protected PT_Global.Process myProcess = PT_Global.Process.None;
	protected PT_Global.Process myQueueProcess = PT_Global.Process.None;
	protected PT_Global.Process myLastProcess = PT_Global.Process.None;

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

		SetProcess (PT_Global.Process.Idle);
		myProcessDisplay = Instantiate (myProcessDisplayPrefab, this.transform).GetComponent<PT_ProcessDisplay> ();
		myProcessDisplay.transform.localPosition = Vector3.zero;
		myProcessDisplay.HideProcess ();

		myCurHP = myAttributes.HP;
		myProcessDisplay.ShowHP (myCurHP);

		for (int i = 0; i < (int)PT_Global.Status.End; i++) {
			myStatus.Add (0f);
		}

		//CD at beginning
		SetProcess (PT_Global.Process.CD);
		myTimer = myAttributes.CD;
		myProcessDisplay.ShowCD (myTimer);

		CustomInitialize ();
	}

	protected virtual void CustomInitialize () {

	}

	#region Action
	public virtual bool Action (GameObject g_target, Vector2 g_targetPos) {
		
		if (myProcess != PT_Global.Process.Idle && 
			myProcess != PT_Global.Process.CD) {
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
		if (g_target.name == (PT_Global.Constants.NAME_MAP_FIELD + myOwnerID.ToString ())) {
			myTargetPosition = g_targetPos;
			QueueMove ();
			return true;
		} else if (g_target.name == (PT_Global.Constants.NAME_MAP_FIELD + (1 - myOwnerID).ToString ()) ||
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
		myQueueProcess = PT_Global.Process.Idle;
	}

	public void QueueMove () {
		myQueueProcess = PT_Global.Process.Move;
		myPlayerController.RpcShowTarget (myID, -1, -1, myTargetPosition);
	}

	public void QueueAttack () {
		myQueueProcess = PT_Global.Process.Attack;

		if (isSingleTarget) {
			PT_BaseChess t_baseChess = myTargetGameObject.GetComponent<PT_BaseChess> ();
			myPlayerController.RpcShowTarget (myID, t_baseChess.GetMyOwnerID (), t_baseChess.GetMyID (), myTargetPosition);
		} else {
			myPlayerController.RpcShowTarget (myID, -1, -1, myTargetPosition);
		}
	}

	public void QueueCast () {
		myQueueProcess = PT_Global.Process.CT;

		if (isSingleTarget) {
			PT_BaseChess t_baseChess = myTargetGameObject.GetComponent<PT_BaseChess> ();
			myPlayerController.RpcShowTarget (myID, t_baseChess.GetMyOwnerID (), t_baseChess.GetMyID (), myTargetPosition);
		} else {
			myPlayerController.RpcShowTarget (myID, -1, -1, myTargetPosition);
		}
	}

	protected virtual void Idle () {
		SetProcess (PT_Global.Process.Idle);
	}

	protected void Move () {
		SetProcess (PT_Global.Process.Move);
	}

	protected virtual void CoolDown (float g_scale = 1) {
		SetProcess (PT_Global.Process.CD);
		myTimer = myAttributes.CD * g_scale;
		RpcShowCD (myTimer);

		myPlayerController.RpcHideTarget (myID);
	}

	protected virtual void Cast (float g_scale = 1) {
		SetProcess (PT_Global.Process.CT);
		myTimer = myAttributes.CT * g_scale;
		RpcShowCT (myTimer);
	}

	protected virtual void Attack () {
		SetProcess (PT_Global.Process.Attack);
	}

	protected void AttackBack () {
		SetProcess (PT_Global.Process.AttackBack);

		myPlayerController.RpcHideTarget (myID);
	}

	protected void SetProcess (PT_Global.Process g_process) {
		myProcess = g_process;
		//update the collider, if attack or attack back, the collider is trigger, else is not trigger
		if (isServer) {
			if (myProcess == PT_Global.Process.Move ||
			    myProcess == PT_Global.Process.Attack ||
			    myProcess == PT_Global.Process.AttackBack)
				myCollider.isTrigger = true;
			else
				myCollider.isTrigger = false;
		}
	}

	public PT_Global.Process GetProcess () {
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

		if (GetStatus (PT_Global.Status.Freeze) || GetStatus (PT_Global.Status.Gold)) {
			return;
		}

		UpdateAction ();
	}

	protected virtual void UpdateAction () {

		switch (myProcess) {
		case PT_Global.Process.Idle:
			UpdateAction_Idle ();
			break;
		case PT_Global.Process.Move:
			UpdateAction_Move ();
			break;
		case PT_Global.Process.CD:
			UpdateAction_CD ();
			break;
		case PT_Global.Process.CT:
			UpdateAction_CT ();
			break;
		case PT_Global.Process.Attack:
			UpdateAction_Attack ();
			break;
		case PT_Global.Process.AttackBack:
			UpdateAction_AttackBack ();
			break;
		}

		//update Timer
		if (myTimer > 0)
			myTimer -= Time.deltaTime;
//		Debug.Log ("timer: " + myTimer);
	}

	protected virtual void UpdateAction_Idle () {
		if (myQueueProcess == PT_Global.Process.Move) {
			Move ();
			//play SFX move 
//			PlayMySFX (myMoveSFX);
		} else if (myQueueProcess == PT_Global.Process.Attack) {
			Attack ();
			//play SFX ATK 
//			PlayMySFX (myAttackSFX);
		} else if (myQueueProcess == PT_Global.Process.CT) {
			Cast ();
			//play SFX CT 
//			PlayMySFX (myCastSFX);
		}
		myQueueProcess = PT_Global.Process.None;
	}

	protected virtual void UpdateAction_Move () {
		//move the chess
		this.transform.position = 
			Vector2.Lerp (this.transform.position, myTargetPosition, PT_Global.Constants.SPEED_MOVE * Time.deltaTime);

		//if the chess at the target, stop
		if (Vector2.Distance (this.transform.position, myTargetPosition) <= PT_Global.Constants.DISTANCE_RESET) {
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
			Vector2.Lerp (this.transform.position, myTargetPosition, PT_Global.Constants.SPEED_MOVE * Time.deltaTime);

		//if the chess at the target, stop
		if (Vector2.Distance (this.transform.position, myTargetPosition) <= PT_Global.Constants.DISTANCE_RESET) {
			//Reset and come back
			AttackBack ();
		}
	}

	public virtual void UpdateAction_AttackBack () {
		//move the chess
		this.transform.position = 
			Vector2.Lerp (this.transform.position, myPosition, PT_Global.Constants.SPEED_MOVE * Time.deltaTime);

		//if the chess at the target, stop
		if (Vector2.Distance (this.transform.position, myPosition) <= PT_Global.Constants.DISTANCE_RESET) {
			//set my position
			this.transform.position = myPosition;

			//start cool down
			CoolDown ();
		}
	}

	#endregion

	#region Status
	public void SetStatus (PT_Global.Status g_status, float g_time) {
		if (GetStatus (PT_Global.Status.Gold))
			return;

		if ((GetStatus (PT_Global.Status.SpellImmune) || GetStatus (PT_Global.Status.Bubble)) &&
		    g_status == PT_Global.Status.Freeze)
			return;

		myStatus [(int)g_status] = Mathf.Max (myStatus [(int)g_status], g_time);

		if (g_status == PT_Global.Status.Freeze) {
			RpcShowStatus_Freeze (myStatus [(int)g_status]);
		}
		if (g_status == PT_Global.Status.Gold) {
			RpcShowStatus_Gold (myStatus [(int)g_status]);
		}

		OnSetStatus ();
	}

	protected virtual void OnSetStatus () {

	}

	public bool GetStatus (PT_Global.Status g_status) {
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
		Idle ();
		myPlayerController.RpcHideTarget (myID);
		RpcShowIdle ();
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
		if (myProcess == PT_Global.Process.Dead)
			return;

		//need to be rewrite in different chess
	}
	#endregion

	#region Damage
	public virtual void HPModify (PT_Global.HPModifierType g_type, int g_value) {
		if (myProcess == PT_Global.Process.Dead)
			return;

		//gold prevent damage
//		if (st_Gold >= 0)
//			return;

		int t_modifier = g_value;

		if (g_type == PT_Global.HPModifierType.PhysicalDamage) {
			t_modifier -= myAttributes.PR;
			if (t_modifier < 1)
				t_modifier = 1;
		}

		if (t_modifier <= 0)
			return;

		switch (g_type) {
		case PT_Global.HPModifierType.PhysicalDamage:
			HPModify_PhysicalDamage (t_modifier);
			break;
		case PT_Global.HPModifierType.MagicDamage:
			HPModify_MagicDamage (t_modifier);
			break;
		case PT_Global.HPModifierType.Healing:
			HPModify_Healing (t_modifier);
			break;
		}

		//play SFX damage 
//		PlayMySFX (myDamageSFX);
	}

	protected virtual void DoOnDamage () {

	}

	protected virtual void HPModify_PhysicalDamage (int g_value) {
		//used by chameleon, mushroom

		if (GetStatus (PT_Global.Status.Gold))
			return;

		myCurHP -= g_value;

		RpcShowDamage (g_value);

		DoOnDamage ();

		CheckIsDead ();
	}

	protected virtual void HPModify_MagicDamage (int g_value) {
		//used by chameleon

		if (GetStatus (PT_Global.Status.Gold) ||
		    GetStatus (PT_Global.Status.SpellImmune) ||
		    GetStatus (PT_Global.Status.Bubble))
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
		if (myProcess == PT_Global.Process.Dead)
			return;

		if (myCurHP <= 0) {
			SetProcess (PT_Global.Process.Dead);

			DoOnDead ();
			myCurHP = 0;
			myPlayerController.RpcHideTarget (myID);

			//play SFX dead
//			PlayMySFX (myDeadSFX);
		}

		RpcShowHP (myCurHP);
	}

	protected virtual void DoOnDead () {

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
			mySpriteRenderer.sortingLayerName = PT_Global.Constants.SORTINGLAYER_DEADBODY;
			mySpriteRenderer.color = PT_Global.Constants.COLOR_DEADBODY;
			myCollider.enabled = false;
		}
	}
	#endregion


}


