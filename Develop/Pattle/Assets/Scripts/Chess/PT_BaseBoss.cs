﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PT_Action;

public class PT_BaseBoss : PT_BaseChess {

	protected PT_NetworkAdventureManager myManager;

	protected ActionType myActionType;
	protected ActionType myLastActionType;

	[SerializeField] protected SO_MoveSettings myMoveSettings;

	protected virtual void ActionAI () {


	}

	protected override void Idle () {
		
		base.Idle ();

		ActionAI ();
	}

	public void InitPosition () {
		this.transform.position = myMoveSettings.GetStartPosition ();
	}

	public void SetMyManager (PT_NetworkAdventureManager g_manager) {
		myManager = g_manager;
	}

	protected override void Move () {

		myTargetPosition = myMoveSettings.GetOtherRandomMovePosition (this.transform.position);

		base.Move ();

	}

	protected virtual void Skill_1 () {
		CoolDown ();
	}

	protected virtual void Skill_2 () {
		CoolDown ();
	}

	protected virtual void Skill_3 () {
		CoolDown ();
	}

	protected virtual void Skill_4 () {
		CoolDown ();
	}

	protected virtual void Skill_5 () {
		CoolDown ();
	}

	protected override void UpdateAction_CT () {
		if (myTimer <= 0) {
			switch (myActionType) {
			case ActionType.Skill_1:
				Skill_1 ();
				break;
			case ActionType.Skill_2:
				Skill_2 ();
				break;
			case ActionType.Skill_3:
				Skill_3 ();
				break;
			case ActionType.Skill_4:
				Skill_4 ();
				break;
			case ActionType.Skill_5:
				Skill_5 ();
				break;
			}
		}
	}

	protected GameObject GetEnemy_Random () {
		// get the enemy list from manager
		List<GameObject> t_enemyList = myManager.GetPlayerChessList ();

		// if the list is empty, return null
		if (t_enemyList == null || t_enemyList.Count == 0)
			return null;
		
		return t_enemyList [Random.Range (0, t_enemyList.Count)];
	}


	/// <summary>
	/// Gets the enemy with lowest HP.
	/// if there are more than 1 enemy all have the lowest HP, pick a random one among them
	/// </summary>
	/// <returns>the enemy with lowest HP.</returns>
	protected GameObject GetEnemy_LowestHP () {
		// get the enemy list from manager
		List<GameObject> t_enemyList = myManager.GetPlayerChessList ();

		// if the list is empty, return null
		if (t_enemyList == null || t_enemyList.Count == 0)
			return null;

		// create a list that puts in all the enemies with lowest HP
		List<GameObject> t_targetEnemyList = new List<GameObject> ();
		t_targetEnemyList.Add (t_enemyList [0]);
		int t_targetEnemyHP = GetEnemyHP (t_enemyList [0]);

		// go though the list of enemies 
		for (int i = 0; i < t_enemyList.Count; i++) {
			
			int f_HP = GetEnemyHP (t_enemyList [i]);

			// if current HP is smaller that recoreded enemies
			if (f_HP < t_targetEnemyHP) {
				t_targetEnemyHP = f_HP;

				t_targetEnemyList.Clear ();
				t_targetEnemyList.Add (t_enemyList [i]);

			} else if (f_HP == t_targetEnemyHP && t_targetEnemyList.Contains (t_enemyList [i]) == false) {
				t_targetEnemyList.Add (t_enemyList [i]);
			}
		}

		return t_targetEnemyList [Random.Range (0, t_targetEnemyList.Count)];
	}

	/// <summary>
	/// Gets the list of enemies, arange them randomly
	/// </summary>
	/// <returns>a list of enemies.</returns>
	protected List<GameObject> GetEnemies_Random () {
		// get the enemy list from manager
		List<GameObject> t_enemyList = myManager.GetPlayerChessList ();

		// if the list is empty, return null
		if (t_enemyList == null || t_enemyList.Count == 0)
			return null;

		t_enemyList = new List<GameObject> (t_enemyList);
		List<GameObject> t_targetList = new List<GameObject> ();

		for (int f_loopTime = 0; f_loopTime < 100; f_loopTime++) {

			int w_index = Random.Range (0, t_enemyList.Count);
			t_targetList.Add (t_enemyList [w_index]);
			t_enemyList.RemoveAt (w_index);

			if (f_loopTime == 100) {
				Debug.LogError ("I Spend Too Much Time In This Loop!");
			}
		}

		return t_targetList;
	}


	/// <summary>
	/// Gets the list of enemies, arange them from lowest HP to highest
	/// </summary>
	/// <returns>a list of enemies.</returns>
	protected List<GameObject> GetEnemies_LowerHP () {
		// get the enemy list from manager
		List<GameObject> t_enemyList = myManager.GetPlayerChessList ();

		// if the list is empty, return null
		if (t_enemyList == null || t_enemyList.Count == 0)
			return null;

		t_enemyList = new List<GameObject> (t_enemyList);

		for (int i = 0; i < t_enemyList.Count - 1; i++) {
			for (int j = 0; j < t_enemyList.Count - i - 1; j++) {
				if (GetEnemyHP (t_enemyList [j]) > GetEnemyHP (t_enemyList [j + 1])) {
					GameObject f_temp = t_enemyList [j];
					t_enemyList [j] = t_enemyList [j + 1];
					t_enemyList [j + 1] = f_temp;
				}
			}
		}

		return t_enemyList;
	}


	/// <summary>
	/// Gets the enemy HP.
	/// </summary>
	/// <returns> enemy HP.</returns>
	/// <param name="g_enemyObject"> Enemy game object.</param>
	protected int GetEnemyHP (GameObject g_enemyObject) {
		if (g_enemyObject.GetComponent<PT_BaseChess> () == null) {
			Debug.LogError ("cannot get the base chess script!");
			return -1;
		}
		return g_enemyObject.GetComponent<PT_BaseChess> ().GetCurHP ();
	}
}
