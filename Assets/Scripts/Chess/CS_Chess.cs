//Pattle
//This is the base class used by all chess-classes, including AI chesses

using UnityEngine;
using System.Collections;

public class CS_Chess : MonoBehaviour {

	//Who control this chess;
	protected GameObject myController;
	
	// team number of the chess
	public int myTeamNumber;

	//Attributes
	//HealthPoint
	public int at_HP;
	//CoolDown
	public float at_CD;
	//CastTime
	public float at_CT;
	//Current HealthPoint
	protected int at_CurHP;
	//damage and defence
	public int at_PDM;
	public int at_PDF;
	public int at_MDM;

	//Process
	protected int process;
	protected int preProcess;
	protected int lastProcess;

	//Status
	protected float st_Freeze = 0;
	protected float st_Gold = 0;
	protected float st_SpellImmune = 0;
	protected float st_Bubble = 0;

	protected float st_Fish_Yin = 0;
	protected float st_Fish_Yang = 0;

	protected float st_Poisoned = 0;
	protected float st_Infected = 0;
	protected float st_Antibody = 0;
	protected float st_Recovery = 0;
	protected float st_Focus = 0;
	
	//For Action
	protected Vector2 myPosition;
	protected GameObject myTargetGameObject;
	protected Vector2 myTargetPosition;

	[SerializeField] bool myTargetIsSingle;
	[SerializeField] GameObject myTargetSign;
	[SerializeField] GameObject myTargetLine;

	//my stuff
	protected CircleCollider2D myCollider;
	protected SpriteRenderer mySpriteRenderer;
	public float myPackageRatio = 1;
	public GameObject myParticleMove;
	public GameObject myPackageProcess;
	public GameObject myPackageStable;
	protected Animator myAnimatorProcess;
	protected float myAnimatorProcessSpeed;
	protected Animator myAnimatorStable;
	protected GameObject myText;
	public GameObject mySkill;

	//SFX
//	public AudioClip myMoveSFX;
//	public AudioClip myAttackSFX;
//	public AudioClip myCastSFX;
//	public AudioClip myDamageSFX;
//	public AudioClip myHealSFX;
//	public AudioClip myDeadSFX;

	public CS_AudioClip myMoveSFX;
	public CS_AudioClip myAttackSFX;
	public CS_AudioClip myCastSFX;
	public CS_AudioClip myDamageSFX;
	public CS_AudioClip myHealSFX;
	public CS_AudioClip myDeadSFX;

	//make sure that the chess Init
	//private bool InitDone = false;
	
	//timer to count the time in different Process
	protected float timer;
	
	void Start () {
		//if (InitDone == true)
		//	return;

		Initialize ();
	}

	public void Initialize()
	{
		//delet all child
		foreach (Transform child in transform) {
			Destroy (child.gameObject);
		}

		if (myParticleMove != null) {
			GameObject t_myParticleMove = Instantiate (myParticleMove, this.transform.position, Quaternion.identity) as GameObject;
//			t_myParticleMove.transform.parent = this.transform;
//			t_myParticleMove.transform.localScale = Vector3.one * myPackageRatio;

			var sizeOverLifetime = t_myParticleMove.GetComponent<ParticleSystem>().sizeOverLifetime;
			sizeOverLifetime.enabled = true;
			sizeOverLifetime.sizeMultiplier = myPackageRatio;

			t_myParticleMove.GetComponent<CS_Particle> ().SetMyIdol (this.gameObject);
			myParticleMove = t_myParticleMove;
		}
		else Debug.LogError("Cannot Find myParticleMove");
		
		if (myPackageProcess != null) {
			GameObject t_myPackageProcess = Instantiate (myPackageProcess, this.transform.position, Quaternion.identity) as GameObject;
			t_myPackageProcess.name = "PackageProcess";
			t_myPackageProcess.transform.parent = this.transform;
			t_myPackageProcess.transform.localScale = Vector3.one * myPackageRatio;
			myPackageProcess = t_myPackageProcess;
		}
		else Debug.LogError("Cannot Find Package");
		
		if (myPackageStable != null) {
			GameObject t_myPackageStable = Instantiate (myPackageStable, this.transform.position, Quaternion.identity) as GameObject;
			t_myPackageStable.name = "PackageStable";
			t_myPackageStable.transform.parent = this.transform;
			t_myPackageStable.transform.localScale = Vector3.one * myPackageRatio;
			myPackageStable = t_myPackageStable;
		}
		else Debug.LogError("Cannot Find PackageStable");

		if (myTargetSign != null) {
			GameObject t_myTargetSign = Instantiate (myTargetSign) as GameObject;
			t_myTargetSign.name = "TargetSign";
			//t_myTargetSign.transform.parent = this.transform;
			myTargetSign = t_myTargetSign;
			myTargetSign.SetActive (false);
		}
		else Debug.LogWarning("Cannot Find TargetSign");

		if (myTargetLine != null) {
			GameObject t_myTargetLine = Instantiate (myTargetLine) as GameObject;
			t_myTargetLine.name = "TargetLine";
			t_myTargetLine.transform.parent = this.transform;
			myTargetLine = t_myTargetLine;
			myTargetLine.SetActive (false);
		}
		else Debug.LogWarning("Cannot Find TargetLine");
		
		myText = myPackageProcess.transform.FindChild (CS_Global.NAME_TEXT_HP).gameObject;
		
		myCollider = this.GetComponent<CircleCollider2D> ();
		mySpriteRenderer = this.GetComponent<SpriteRenderer> ();
		
		myAnimatorProcess = myPackageProcess.GetComponent<Animator> ();
		myAnimatorStable = myPackageStable.GetComponent<Animator> ();
		
		at_CurHP = at_HP; 


		CoolDown (at_CD);
		preProcess = CS_Global.PS_NULL;
		ShowHP ();

		CheckIsDead ();

		CustomInitialize ();

		//InitDone = true;
	}

	public virtual void CustomInitialize () {

	}

	//===============================================================================================
	//COLLISION
	void OnTriggerEnter2D (Collider2D other) {
		CollisionAction (other.gameObject);
	}
	
	void OnCollisionEnter2D (Collision2D collision) {
		CollisionAction (collision.gameObject);
	}
	
	public virtual void CollisionAction(GameObject g_GO_Collision)
	{
		if (process == CS_Global.PS_DEAD)
			return;
		
		//need to be rewrite in different chess
		if (process == CS_Global.PS_ATTACK && 
		    g_GO_Collision.tag != this.tag && 
		    g_GO_Collision.tag != CS_Global.TAG_MAP && 
		    g_GO_Collision.tag != CS_Global.TAG_FA && 
		    g_GO_Collision.tag != CS_Global.TAG_FB) {
			AttackBack();
		}
	}

	
	//===============================================================================================
	//PROCESS
	public virtual void Action (GameObject g_Input) {
		//if can take action
		//g_Input.SendMessage ("Done");
		//if can not take action
		//g_Input.SendMessage ("Undone");

		//not idle or freeze, can not take action
		if (process != CS_Global.PS_IDLE &&
		    process != CS_Global.PS_CD) {
			g_Input.SendMessage ("Undone");
			//Debug.Log ("Return");
			return;
		}

		IndividualAction (g_Input);
	}

	public virtual void IndividualAction (GameObject g_Input) {
		//if can take action
		//g_Input.SendMessage ("Done");
		//if can not take action
		//g_Input.SendMessage ("Undone");

		if (myTargetGameObject.tag == ("F" + this.tag)) {
			PreMove ();
			g_Input.SendMessage ("Done");
		} else if (myTargetGameObject.tag != this.tag){
			PreAttack ();
			g_Input.SendMessage ("Done");
		} else {
			g_Input.SendMessage ("Undone");
			//Cast();
		}
	}

	public void PreIdle () {
		preProcess = CS_Global.PS_IDLE;
	}

	public void PreMove () {
		preProcess = CS_Global.PS_MOVE;
		ShowTargetSign ();
	}

	public void PreAttack () {
		preProcess = CS_Global.PS_ATTACK;
		ShowTargetSign ();
	}

	public void PreCast () {
		preProcess = CS_Global.PS_CT;
		ShowTargetSign ();
	}
		
	public void ShowTargetSign () {
		if (myTargetLine == null || myTargetSign == null)
			return;

		UpdateTargetSign ();

		myTargetLine.SetActive (true);
		myTargetSign.SetActive (true);
	}

	public void HideTargetSign () {
		if (myTargetLine == null || myTargetSign == null)
			return;
		
		myTargetLine.SetActive (false);
		myTargetSign.SetActive (false);
	}
	
	public virtual void Idle () {
		SetProcess (CS_Global.PS_IDLE);
	}
	
	public virtual void Move () {
		SetProcess (CS_Global.PS_MOVE);
	}
	
	public virtual void CoolDown (float g_time) {
		SetProcess (CS_Global.PS_CD);
		timer = g_time;

		HideTargetSign ();
	}
	
	public virtual void Cast () {
		SetProcess (CS_Global.PS_CT);
		timer = at_CT;
	}
	
	public virtual void Attack () {
		SetProcess (CS_Global.PS_ATTACK);
		myPosition = this.transform.position;
	}
	
	public virtual void AttackBack () {
		SetProcess (CS_Global.PS_ATTACKBACK);

		HideTargetSign ();
	}
	
	//===============================================================================================
	//UPDATE
	void Update () {
		
		this.GetComponent<Rigidbody2D> ().velocity = Vector3.zero;

		//keep the rotation of chess, may be used in fish boss and PVP (Arena)
		this.transform.rotation = Camera.main.transform.rotation;

		UpdateStatus ();

		UpdateSpriteRenderer ();

		UpdateTargetSign ();

		//if Dead, can not update Action
		//if freeze, can not update Action
		
		if (st_Gold >= 0)
			return;

		if (st_Freeze >= 0) 
			return;




		UpdateAction ();

		CustomUpdate ();
	}

	public virtual void CustomUpdate () {

	}

	//For dead
	public void UpdateSpriteRenderer () {
		if (process == CS_Global.PS_DEAD)
			mySpriteRenderer.color = new Color(0.2f, 0.2f, 0.2f, 0.5f);
		else mySpriteRenderer.color = Color.white;
	}

	public void UpdateTargetSign () {
		if (myTargetLine == null || myTargetSign == null)
			return;
		
		if (process == CS_Global.PS_DEAD) {
			myTargetLine.SetActive (false);
			myTargetSign.SetActive (false);
			return;
		}

		if (myTargetLine.activeSelf == true && myTargetSign.activeSelf == true) {
			
			if (myTargetIsSingle && (preProcess == CS_Global.PS_CT || GetProcess() == CS_Global.PS_CT)) {
				myTargetSign.transform.position = myTargetGameObject.transform.position;
//				Debug.Log (myTargetIsSingle + "&" + preProcess + "=" + myTargetGameObject);
			} else
				myTargetSign.transform.position = myTargetPosition;
			myTargetLine.GetComponent<LineRenderer> ().SetPosition (0, this.transform.position);
			myTargetLine.GetComponent<LineRenderer> ().SetPosition (1, myTargetSign.transform.position);
		}
	}

	public void UpdateStatus () {
		//Debug.Log (st_Gold);
		if (st_Gold >= 0) {

			st_Gold -= Time.deltaTime;

			myAnimatorStable.SetFloat ("st_Gold", st_Gold);

			//if gold stop process animation speed
			myAnimatorProcess.speed = 0;

		} else if (st_Freeze >= 0) {

			st_Freeze -= Time.deltaTime;

			myAnimatorStable.SetFloat ("st_Freeze", st_Freeze);

			//if freeze stop process animation speed
			myAnimatorProcess.speed = 0;

		} else {
			myAnimatorProcess.speed = myAnimatorProcessSpeed;
		}

		if (st_Bubble >= 0) {
			//Debug.Log ("st_Bubble : " + st_Bubble);
			st_Bubble -= Time.deltaTime;
		}

		if (st_SpellImmune >= 0) {

			st_SpellImmune -= Time.deltaTime;

			myAnimatorStable.SetFloat ("st_SpellImmune", st_SpellImmune);
		}

		if (st_Fish_Yin > 0 && st_Fish_Yang > 0) {
			DamageM (CS_Global.DAMAGE_FISH);
			st_Fish_Yin = 0;
			st_Fish_Yang = 0;
		}

		if (st_Fish_Yin > 0) {
			st_Fish_Yin -= Time.deltaTime;
		}

		if (st_Fish_Yang >= 0) {
			st_Fish_Yang -= Time.deltaTime;
		}
	}

	public virtual void UpdateAction () {

		if (process == CS_Global.PS_IDLE) {
			UpdateAction_Idle ();
		} else if (process == CS_Global.PS_MOVE) {
			UpdateAction_Move ();
		} else if (process == CS_Global.PS_ATTACK) {
			UpdateAction_Attack ();
		} else if (process == CS_Global.PS_ATTACKBACK) {
			UpdateAction_AttackBack ();
		} else if (process == CS_Global.PS_CD) {
			UpdateAction_CD ();
		} else if (process == CS_Global.PS_CT) {
			UpdateAction_CT ();
		}

		//update Timer
		if (timer > 0)
			timer -= Time.deltaTime;
	}

	public void UpdateAction_Idle () {
		if (preProcess == CS_Global.PS_MOVE) {
			Move ();
			//play SFX move 
			PlayMySFX (myMoveSFX);
		} else if (preProcess == CS_Global.PS_ATTACK) {
			Attack ();
			//play SFX ATK 
			PlayMySFX (myAttackSFX);
		} else if (preProcess == CS_Global.PS_CT) {
			Cast ();
			//play SFX CT 
			PlayMySFX (myCastSFX);
		}
		preProcess = CS_Global.PS_NULL;
	}
	
	public virtual void UpdateAction_Move () {
		//move the chess
		this.transform.position = 
			Vector2.Lerp (this.transform.position, myTargetPosition, CS_Global.SPEED_MOVE * Time.deltaTime);
		
		//if the chess at the target, stop
		if (Vector2.Distance (this.transform.position, myTargetPosition) <= CS_Global.DISTANCE_RESET) {
			//set my position
			this.transform.position = myTargetPosition;
			myPosition = myTargetPosition;
			
			//start cool down
			CoolDown(at_CD / 2);
		}
	}

	public virtual void UpdateAction_CD () {
		if (timer <= 0)
			Idle ();
	}
	
	public virtual void UpdateAction_CT () {
		if (timer <= 0)
			Attack ();
	}

	public virtual void UpdateAction_Attack () {
		//different in different character
		//move the chess
		this.transform.position = 
			Vector2.Lerp (this.transform.position, myTargetPosition, CS_Global.SPEED_MOVE * Time.deltaTime);
		
		//if the chess at the target, stop
		if (Vector2.Distance (this.transform.position, myTargetPosition) <= CS_Global.DISTANCE_RESET) {
			//Reset and come back
			AttackBack();
		}
	}
	
	public virtual void UpdateAction_AttackBack () {
		//move the chess
		this.transform.position = 
			Vector2.Lerp (this.transform.position, myPosition, CS_Global.SPEED_MOVE * Time.deltaTime);
		
		//if the chess at the target, stop
		if (Vector2.Distance (this.transform.position, myPosition) <= CS_Global.DISTANCE_RESET) {
			//set my position
			this.transform.position = myPosition;
			
			//start cool down
			CoolDown(at_CD);
		}
	}

	//======================================================================================
	//STATUS
	public void ST_Freeze (float g_freezeTime) {

		if (st_Gold >= 0)
			return;
		//if spell immune, prevent freeze
		if (st_SpellImmune >= 0)
			return;

		if (g_freezeTime >= st_Freeze) {
			st_Freeze = g_freezeTime;
		}
	}

	public void ST_Gold (float g_goldTime) {
		if (st_Gold < 0) {
			st_Gold = g_goldTime;
		}
	}

	public void ST_SpellImmune (float g_splimuTime) {

		if (st_Gold >= 0)
			return;

		if (g_splimuTime >= st_SpellImmune) {
			st_SpellImmune = g_splimuTime;
		}
	}

	public void ST_Bubble (float g_bubbleTime) {

		if (st_Gold >= 0)
			return;

		if (g_bubbleTime >= st_Bubble) {
			st_Bubble = g_bubbleTime;
		}
	}

	public bool IsOn_ST_Bubble () {
		if (st_Bubble >= 0)
			return true;
		else 
			return false;
	}

	public void ST_FISH (int g_fish_num) {

		if (st_Gold >= 0)
			return;

		if (g_fish_num == CS_Global.ST_FISH_YIN) {
			st_Fish_Yin = 1;
		} else if (g_fish_num == CS_Global.ST_FISH_YANG) {
			st_Fish_Yang = 1;
		}
	}

	//======================================================================================
	//DAMAGE
	public virtual void DamageP (int g_PDM) {

		if (process == CS_Global.PS_DEAD)
			return;

		//gold prevent damage
		if (st_Gold >= 0)
			return;

		int t_damage = g_PDM - at_PDF;
		if (t_damage < 0)
			t_damage = 0;
		
		at_CurHP -= t_damage;

		if (t_damage != 0) 
			myPackageStable.SendMessage("ShowDamage", -t_damage);

		//Debug.Log (t_damage);

		DoOnDamage ();
		DoOnPDM ();
		CheckIsDead ();

		//play SFX damage 
		PlayMySFX (myDamageSFX);
	}
	
	public virtual void DamageM (int g_MDM) {

		if (process == CS_Global.PS_DEAD)
			return;

		//Spell Immune prevent MDM
		if (st_SpellImmune >= 0)
			return;

		//gold prevent damage
		if (st_Gold >= 0)
			return;

		if (st_Bubble >= 0)
			return;

		at_CurHP -= g_MDM;

		if (g_MDM != 0) 
			myPackageStable.SendMessage("ShowDamage", -g_MDM);

		DoOnDamage ();
		DoOnMDM ();
		CheckIsDead ();

		//play SFX damage 
		PlayMySFX (myDamageSFX);
	}
	
	public virtual void Heal (int g_MDM) {
		if (process == CS_Global.PS_DEAD)
			return;

		//gold prevent heal
		if (st_Gold >= 0)
			return;

		int t_heal = at_HP - at_CurHP;

		if (g_MDM <= t_heal)
			t_heal = g_MDM;

		myPackageStable.SendMessage("ShowHeal", t_heal);

		//Debug.Log ("Heal:" + g_MDM);

		at_CurHP += t_heal;

		//if (at_CurHP > at_HP)
		//	at_CurHP = at_HP;

		DoOnHeal ();
		ShowHP ();

		//play SFX Heal 
		PlayMySFX (myHealSFX);
	}

	public virtual void DoOnDamage () {

	}

	public virtual void DoOnPDM () {
		//used by chameleon, mushroom
	}

	public virtual void DoOnMDM () {
		//used by chameleon
	}

	public virtual void DoOnHeal () {
		
	}

	//======================================================================================
	//SMALL FUCTIONS
	public void CheckIsDead () {
		if (process == CS_Global.PS_DEAD)
			return;
		
		if (at_CurHP <= 0) {
			SetProcess (CS_Global.PS_DEAD);
			//process = CS_Global.PS_DEAD;
			//myAnimatorProcess.SetInteger ("process", process);
			DoOnDead ();
			at_CurHP = 0;

			//play SFX dead
			PlayMySFX (myDeadSFX);
		}

		ShowHP ();
	}

	public virtual void DoOnDead () {
		
	}

	//Show HP on chess
	public void ShowHP () {
		if (myText == null)
			myText = myPackageProcess.transform.FindChild (CS_Global.NAME_TEXT_HP).gameObject;
		myText.GetComponent<TextMesh> ().text = at_CurHP.ToString();
	}


	//used by Update or Kill
	protected void SetProcess (int g_process) {
		//if (InitDone == false)
		//	Initialize ();

		process = g_process;

		//reset animation 20160229
		myAnimatorProcess.PlayInFixedTime ("Idle");

		//update the process integer in animator
		myAnimatorProcess.SetInteger ("process", process);

		//update the collider, if attack or attack back, the collider is trigger, else is not trigger
		if (process == CS_Global.PS_MOVE ||
			//process == CS_Global.PS_DEAD ||
		    process == CS_Global.PS_ATTACK ||
			process == CS_Global.PS_ATTACKBACK )
			myCollider.isTrigger = true;
		else
			myCollider.isTrigger = false;

		//if dead, close collider
		if (process == CS_Global.PS_DEAD) {
			myText.SetActive(false);
			mySpriteRenderer.sortingLayerName = CS_Global.SL_DEADBODY;
			myCollider.enabled = false;
		} else {
			myText.SetActive(true);
			mySpriteRenderer.sortingLayerName = CS_Global.SL_CHESS;
			myCollider.enabled = true;
		}

		//update the speed of animator
		if (process == CS_Global.PS_CT)
			myAnimatorProcessSpeed = 1 / at_CT;
		else if (process == CS_Global.PS_CD) {
			if(lastProcess == CS_Global.PS_MOVE)
				myAnimatorProcessSpeed = 2 / at_CD;
			else
				myAnimatorProcessSpeed = 1 / at_CD;
		} else
			myAnimatorProcessSpeed = 1;
		myAnimatorProcess.speed = myAnimatorProcessSpeed;

		lastProcess = process;

		//Debug.Log(this.name + " " + process);
	}

	//used by boss AI and Soprano
	public int GetProcess () {
		return process;
	}

	public int GetCurHP () {
		return at_CurHP;
	}

	public void PlayMySFX (CS_AudioClip g_mySFX) {
		//play SFX  
		CS_AudioManager.Instance.PlaySFX (g_mySFX);
	}

	//=======================================================================================
	//Set target
	public void SetTargetGameObject (GameObject g_GO) {
		if (process != CS_Global.PS_IDLE &&
		    process != CS_Global.PS_CD) {
			return;
		}
		myTargetGameObject = g_GO;
	}

	public void SetTargetPosition (Vector2 g_pos) {
		if (process != CS_Global.PS_IDLE &&
		    process != CS_Global.PS_CD) {
			return;
		}
		myTargetPosition = g_pos;
	}

	public void SetTag (string g_tag) {
		this.tag = g_tag;
	}
}

	