using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hang {
	namespace AiryAudio {

		public enum AiryAudioSourceAction {
			Play,
			Stop,
			Pause,
			UnPause,
		}

		public class AiryAudioSource : MonoBehaviour {

			[SerializeField] AudioSource myAudioSource;
			public AudioSource AudioSource {
				get {
					return myAudioSource;
				}
			}
			private Transform myParent;
			// Use this for initialization
//			void Start () {
//
//			}

			// Update is called once per frame
			void Update () {
				if (myParent != null) {
					if (myAudioSource.isPlaying)
						this.transform.position = myParent.position;
					else
						myParent = null;
				}
			}

			public void SetParent (Transform g_parent) {
				myParent = g_parent;
				this.transform.position = myParent.position;
			}

			public void SetAudioClip (AudioClip g_audioClip) {
				myAudioSource.clip = g_audioClip;
			}

			public float GetVolume () {
				return myAudioSource.volume;
			}

			public void SetVolume (float g_volume) {
				myAudioSource.volume = g_volume;
			}

			public void SetPitch (float g_pitch) {
				myAudioSource.pitch = g_pitch;
			}

			public void SetPosition (Vector3 g_position) {
				this.transform.position = g_position;
			}

			public void SetPosition (Vector2 g_position) {
				SetPosition ((Vector3)g_position);
			}

			public void Action (AiryAudioSourceAction g_action) {
				switch (g_action) {
				case AiryAudioSourceAction.Play:
					myAudioSource.Play ();
					break;
				case AiryAudioSourceAction.Stop:
					myAudioSource.Stop ();
					break;
				case AiryAudioSourceAction.Pause:
					myAudioSource.Pause ();
					break;
				case AiryAudioSourceAction.UnPause:
					myAudioSource.UnPause ();
					break;
				}
			}
		}
	}
}
