using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Pattle {
	namespace Chess{
		public class PT_Chess : NetworkBehaviour {
			[SyncVar][SerializeField] PT_ChessAttributes myAttributes;

			// Use this for initialization
			void Start () {

			}

			// Update is called once per frame
			void Update () {

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
