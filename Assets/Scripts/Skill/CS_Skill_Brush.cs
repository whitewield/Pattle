//While Instantiate
//Please send message to these functions
// 1. SetMyCaster
// 2. SetPDM or SetMDM
// 3. (SetMoveSpeed)

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CS_Skill_Brush : CS_Skill {

	[SerializeField] GameObject targetSign;
	[SerializeField] GameObject line;
	[SerializeField] GameObject particle;
	public List<GameObject> targetList = new List<GameObject>();
	public List<Vector3> linePositionList = new List<Vector3> ();

	void Start () {
		line = Instantiate (line);

		particle = Instantiate (particle, this.transform.position, Quaternion.identity) as GameObject;
		particle.GetComponent<CS_Particle> ().SetMyIdol (this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		UpdateAction_Move ();
		UpdateLine ();
	}

	public void UpdateLine () {

		linePositionList.Clear ();

		linePositionList.Add (this.transform.position);

		foreach (GameObject t_GO in targetList) {
			linePositionList.Add (t_GO.transform.position);
		}
			
		Vector3[] t_positions = linePositionList.ToArray ();
		line.GetComponent<LineRenderer> ().numPositions = t_positions.Length;
		line.GetComponent<LineRenderer> ().SetPositions(t_positions);
////
//
//		line.GetComponent<LineRenderer> ().SetPosition (0, this.transform.position);
//
//		int i = 1;
//		if (targetList.Count > 0) {
//			for (; i < targetList.Count; i++) {
////				t_positions [i + 1] = targetList [i].gameObject.transform.position;
//		
//				line.GetComponent<LineRenderer> ().SetPosition (i, targetList [i - 1].gameObject.transform.position);
//			}
//
//			for (; i < t_positions.Length; i++) {
//				t_positions [i] = targetList [targetList.Count - 1].transform.position;
//			}
//		} else {
//			for (; i < t_positions.Length; i++) {
//				t_positions [i] = this.transform.position;
//			}
//		}
//
//		line.GetComponent<LineRenderer> ().SetPositions(t_positions);
//		line.GetComponent<LineRenderer> ().

	}

	public override void CollisionAction (GameObject g_GO_Collision) {
		//if hit not chess , return
		if (g_GO_Collision.tag != CS_Global.TAG_A && g_GO_Collision.tag != CS_Global.TAG_B)
			return;

		if (g_GO_Collision.tag != myCaster.tag ) {
			g_GO_Collision.SendMessage ("DamageP", at_PDM);
			//g_GO_Collision.SendMessage ("DamageM", at_MDM);
		} else {
			g_GO_Collision.SendMessage ("Heal", at_MDM);
		}
	}

	public void SetTarget (Vector2 g_targetPosition) {
		Vector3 t_position = g_targetPosition;
		t_position += Vector3.forward * 5;
		targetList.Add (Instantiate (targetSign, t_position, Quaternion.identity) as GameObject);
	}

	public override void DoOnKill () {
		foreach (GameObject target in targetList) {
			Destroy (target);
		}
		Destroy (line);
	}

	private void UpdateAction_Move () {
		if (targetList.Count > 0) {
			//move the brush
			this.transform.position = 
				Vector3.Lerp (this.transform.position, targetList[0].transform.position, CS_Global.SPEED_MOVE / 3.2f * Time.deltaTime);

			//if the chess at the target, stop
			if (Vector3.Distance (this.transform.position, targetList[0].transform.position) <= CS_Global.DISTANCE_RESET) {
				//set my position
				this.transform.position = targetList[0].transform.position;
				Destroy (targetList[0].gameObject);
				targetList.RemoveAt(0);
			}
		}
	}
}
