//While Instantiate
//Please send message to these functions
// 1. SetMyCaster
// 2. SetPDM or SetMDM
// 3. SetDirection

using UnityEngine;
using System.Collections;

public class CS_Skill_FireShoot : CS_Skill {
	
	private Vector2 direction;

	public void SetDirection (Vector2 g_targetPosition) {
		Vector2 myPosition = this.transform.position;
		direction = g_targetPosition - myPosition;
		direction = direction.normalized;
		
		if (direction.x > 0)
			this.transform.Rotate (0.0f, 0.0f, -(Vector2.Angle (Vector2.up, direction)));
		else this.transform.Rotate (0.0f, 0.0f, Vector2.Angle (Vector2.up, direction));
	}
}
