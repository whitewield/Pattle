using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Hang {
	namespace AiryAudio {
		[CreateAssetMenu(fileName = "AiryAudioBank", menuName = "Hang/AiryAudioBank", order = 1)]
		public class AiryAudioBank : ScriptableObject {
			[System.Serializable]
			public struct AiryAudioData {
				public string myName;
				public AudioClip myAudioClip;
				public float myBaseVolume;
			}

			[SerializeField] List<AiryAudioData> myBank;

			public List<AiryAudioData> GetMyBank () {
				return myBank;
			}
		}
	}
}