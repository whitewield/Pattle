using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CS_AfterBattle : MonoBehaviour {

	public string myTeamTag;

	public List<GameObject> A_ChessList = new List<GameObject>();
	public List<GameObject> B_ChessList = new List<GameObject>();

	private bool isOn = false;
	
	public GameObject TX_TimerCountDown;
	private float timerCountDown;

	public GameObject SP_Reward;
	public GameObject TX_Coin;
	public Sprite reward_Coin;
	public Sprite reward_FireMage;
	public Sprite reward_Warrior;
	public Sprite reward_Archer;
	public Sprite reward_IceMage;
	public Sprite reward_Paladin;

	// Use this for initialization
	void Start () {
		HideTimerCountDown ();
		ResetTimerCountDown ();
	}
	
	// Update is called once per frame
	void Update () {
		if (isOn == false)
			return;

		//Check team A has alive
		bool t_A_hasAlive = false;
		foreach (GameObject t_chess in A_ChessList) {
			if(t_chess.GetComponent<CS_Chess> ().GetProcess () != CS_Global.PS_DEAD) {
				t_A_hasAlive = true;
				break;
			}
		}
		//Debug.Log ("t_A_hasAlive:" + t_A_hasAlive);

		//Check team B has alive
		bool t_B_hasAlive = false;
		foreach (GameObject t_chess in B_ChessList) {
			if(t_chess.GetComponent<CS_Chess> ().GetProcess () != CS_Global.PS_DEAD) {
				t_B_hasAlive = true;
				break;
			}
		}

		if (t_A_hasAlive == false || t_B_hasAlive == false) {
			timerCountDown -= Time.deltaTime;
			ShowTimerCountDown ();
			if (timerCountDown <= 0) {
				//Check win or lose
				if (t_A_hasAlive == false) {
					if (myTeamTag == CS_Global.TAG_A)
						Lose ();
					else 
						Win ();
				} else if (t_B_hasAlive == false) {
					if (myTeamTag == CS_Global.TAG_B)
						Lose ();
					else 
						Win ();
				}
				Off ();
				HideTimerCountDown ();
			}
		} else {
			HideTimerCountDown ();
			ResetTimerCountDown ();
		}
	}

	public void On (string g_teamTag) {
		isOn = true;
		myTeamTag = g_teamTag;
	}

	public void Off () {
		isOn = false;
	}

	public void AddChess (GameObject g_chess) {
//		Debug.Log ("AddChess:" + g_chess + " Tag:" + g_chess.tag);
		if (g_chess.tag == "A")
			A_ChessList.Add (g_chess);
		if (g_chess.tag == "B")
			B_ChessList.Add (g_chess);
	}

	public void Win () {
//		Debug.Log ("Win");
		this.GetComponent<Animator>().SetTrigger("Win");

		int t_chess_number_dead = 0;
		if (myTeamTag == CS_Global.TAG_A) {
			foreach (GameObject t_chess in A_ChessList) {
				if (t_chess.GetComponent<CS_Chess> ().GetProcess () == CS_Global.PS_DEAD) {
					t_chess_number_dead ++;
				}
			}
		} else if (myTeamTag == CS_Global.TAG_B) {
			foreach (GameObject t_chess in B_ChessList) {
				if (t_chess.GetComponent<CS_Chess> ().GetProcess () == CS_Global.PS_DEAD) {
					t_chess_number_dead ++;
				}
			}
		}

		Debug.Log ("Chess number dead: " + t_chess_number_dead);

		if (t_chess_number_dead == 0) {
			
			//Perfect

			CS_MessageBox.Instance.AdventureVictory (CS_Global.STAR_PERFECT);
		} else {
			
			//Done

			CS_MessageBox.Instance.AdventureVictory (CS_Global.STAR_DONE);
		}
	}

	public void ShowRewardChess (string g_chess) {
		Sprite t_chess;
		switch (g_chess) {
		case "FireMage" : 
			t_chess = reward_FireMage; break;
		case "Warrior" : 
			t_chess = reward_Warrior; break;
		case "Archer" : 
			t_chess = reward_Archer; break;
		case "IceMage" : 
			t_chess = reward_IceMage; break;
		case "Paladin" : 
			t_chess = reward_Paladin; break;
		default : 
			t_chess = reward_Coin; break;
		}
		SP_Reward.GetComponent<SpriteRenderer> ().sprite = t_chess;
		TX_Coin.SetActive(false);
	}

	public void ShowRewardCoins (int g_coins) {
		SP_Reward.GetComponent<SpriteRenderer> ().sprite = reward_Coin;
		TX_Coin.SetActive(true);
		TX_Coin.GetComponent<TextMesh> ().text = g_coins.ToString ();
	}

	public void Lose () {
//		Debug.Log ("Lose");
		this.GetComponent<Animator>().SetTrigger("Lose");
	}

	private void ResetTimerCountDown () {
		timerCountDown = CS_Global.TIME_COUNTDOWN;
	}
	
	private void ShowTimerCountDown () {
		TX_TimerCountDown.SetActive (true);
		TX_TimerCountDown.GetComponent<TextMesh> ().text = timerCountDown.ToString ("f1");
	}
	
	private void HideTimerCountDown () {
		TX_TimerCountDown.SetActive (false);
	}
}
