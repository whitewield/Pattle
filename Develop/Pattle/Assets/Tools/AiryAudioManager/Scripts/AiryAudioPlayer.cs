using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hang {
	namespace AiryAudio {
		public class AiryAudioPlayer : MonoBehaviour {

			[SerializeField] string[] myAudioNames;
			[SerializeField] bool playOnStart;


			// Use this for initialization
			void Start () {
				if (playOnStart && myAudioNames != null) {
					AiryAudioSource t_source = AiryAudioManager.Instance.InitAudioSource (myAudioNames);
					AiryAudioActions.Play (t_source);

				}
			}

			public void Play (int t_index) {
				AiryAudioSource t_source = AiryAudioManager.Instance.InitAudioSource (myAudioNames [t_index]);
				AiryAudioActions.Play (t_source);
			}

			public void Play (string t_name) {
				AiryAudioSource t_source = AiryAudioManager.Instance.InitAudioSource (t_name);
				AiryAudioActions.Play (t_source);
			}

			public void Play () {
				AiryAudioSource t_source = AiryAudioManager.Instance.InitAudioSource (myAudioNames);
				AiryAudioActions.Play (t_source);
			}
		}
	}
}
