using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PT_Skill_Sword : PT_BaseSkill {

	[SerializeField] float myDistance = 1.92f;
	private float mySpeed = 180;
	private float myProcess = 0;

	protected override void Update () {
		if (!isServer)
			return;
		
		myProcess += Time.deltaTime * mySpeed;
		if (myProcess > 360)
			myProcess -= 360;
		Quaternion t_quaternion = Quaternion.Euler (0, 0, myProcess);

		this.transform.rotation = t_quaternion;
		this.transform.position = myCaster.transform.position + this.transform.right * myDistance;
	}
}
