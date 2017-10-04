using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PT_BaseChess : NetworkBehaviour {
	[SyncVar] int myOwnerID = -1;

	protected PT_Global.Process myProcess = PT_Global.Process.Idle;

	//For Action
	protected Vector2 myPosition;
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
		if (g_target.name == (PT_Global.NAME_MAP_FIELD + myOwnerID.ToString ())) {
			myTargetPosition = g_targetPos;
			Move ();
			return true;
		} else if (g_target.name == (PT_Global.NAME_MAP_FIELD + (1 - myOwnerID).ToString ()) ||
			(g_target.GetComponent<PT_BaseChess> () && g_target.GetComponent<PT_BaseChess> ().GetMyOwnerID () != myOwnerID)) {
			myTargetPosition = g_targetPos;
			myPosition = this.transform.position;
			Attack ();
		}
		return false;
	}

	public virtual void Idle () {
		SetProcess (PT_Global.Process.Idle);
	}

	private void Move () {
		SetProcess (PT_Global.Process.Move);
	}

	private void Attack () {
		SetProcess (PT_Global.Process.Attack);
	}

	private void AttackBack () {
		SetProcess (PT_Global.Process.AttackBack);
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
	#endregion

	#region Update
	void Update () {
		//keep the rotation of chess, may be used in fish boss and PVP (Arena)
		this.transform.rotation = Camera.main.transform.rotation;

		if (!isServer)
			return;
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
		case PT_Global.Process.Attack:
			UpdateAction_Attack ();
			break;
		case PT_Global.Process.AttackBack:
			UpdateAction_AttackBack ();
			break;
		}
	}

	protected virtual void UpdateAction_Idle () {

	}

	protected virtual void UpdateAction_Move () {
		//move the chess
		this.transform.position = 
			Vector2.Lerp (this.transform.position, myTargetPosition, PT_Global.SPEED_MOVE * Time.deltaTime);

		//if the chess at the target, stop
		if (Vector2.Distance (this.transform.position, myTargetPosition) <= PT_Global.DISTANCE_RESET) {
			//set my position
			this.transform.position = myTargetPosition;
			myPosition = myTargetPosition;

			//start cool down
			Idle ();
		}
	}

	public virtual void UpdateAction_Attack () {
		//different in different character
		//move the chess
		this.transform.position = 
			Vector2.Lerp (this.transform.position, myTargetPosition, PT_Global.SPEED_MOVE * Time.deltaTime);

		//if the chess at the target, stop
		if (Vector2.Distance (this.transform.position, myTargetPosition) <= PT_Global.DISTANCE_RESET) {
			//Reset and come back
			AttackBack();
		}
	}

	public virtual void UpdateAction_AttackBack () {
		//move the chess
		this.transform.position = 
			Vector2.Lerp (this.transform.position, myPosition, PT_Global.SPEED_MOVE * Time.deltaTime);

		//if the chess at the target, stop
		if (Vector2.Distance (this.transform.position, myPosition) <= PT_Global.DISTANCE_RESET) {
			//set my position
			this.transform.position = myPosition;

			//start cool down
			Idle ();
		}
	}

	#endregion

	#region Collision
	void OnTriggerEnter2D (Collider2D g_collider2D) {
		CollisionAction (g_collider2D.gameObject);
	}

	void OnCollisionEnter2D (Collision2D g_ollision2D) {
		CollisionAction (g_ollision2D.gameObject);
	}

	public virtual void CollisionAction(GameObject g_GO_Collision) {
		if (!isServer)
			return;
		if (myProcess == PT_Global.Process.Dead)
			return;

		//need to be rewrite in different chess
		if (myProcess == PT_Global.Process.Attack && 
			g_GO_Collision.GetComponent<PT_BaseChess>() && g_GO_Collision.GetComponent<PT_BaseChess>().GetMyOwnerID() != myOwnerID) {
			AttackBack ();
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
	#endregion

}
