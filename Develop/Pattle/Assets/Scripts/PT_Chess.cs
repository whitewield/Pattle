using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Pattle {
	namespace Chess{
		public class PT_Chess : NetworkBehaviour {
			[SyncVar][SerializeField] protected PT_ChessAttributes myAttributes;
			[SerializeField] protected NetworkPlayer myNetworkPlayerID;
			//Process
			protected int process;
			protected int preProcess;
			protected int lastProcess;
			// Use this for initialization
			void Start () {

			}

			// Update is called once per frame
			void Update () {

			}

			public bool Action (GameObject g_target, Vector2 g_targetPosition) {
				if (!isServer) {
					Debug.Log ("Action not server!");
					return false;
				}

				this.transform.position = g_targetPosition;
				return true;
			}

			public void SetMyNetworkPlayer (NetworkPlayer g_playerID) {
				myNetworkPlayerID = g_playerID;
			}
		}
			
		[System.Serializable]
		public class PT_ChessAttributes {
			//HealthPoint
			[SerializeField] int at_HP;
			//CoolDown
			[SerializeField] float at_CD;
			//CastTime
			[SerializeField] float at_CT;
			//damage and defence
			[SerializeField] int at_PDM;
			[SerializeField] int at_PDF;
			[SerializeField] int at_MDM;

			//Current HealthPoint
			private int at_CurHP;
		}
	}
}
