//http://www.cnblogs.com/gameprogram/archive/2012/08/15/2640357.html
//http://www.blog.silentkraken.com/2010/04/06/audiomanager/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Hang {
	namespace AiryAudio {

		public struct AiryAudioClip {
			public AiryAudioClip (AudioClip g_audioClip, float g_baseVolume) : this() {
				this.myAudioClip = g_audioClip;
				this.myBaseVolume = g_baseVolume;

			}
			public AudioClip myAudioClip;
			public float myBaseVolume;
		}

		public static class AiryAudioActions {
			public static void Play (AiryAudioSource g_airyAudioSource) {
				g_airyAudioSource.SetPosition (Vector3.zero);
				g_airyAudioSource.Action (AiryAudioSourceAction.Play);
			}

			public static void Play (AiryAudioSource g_airyAudioSource, Vector3 g_position) {
				g_airyAudioSource.SetPosition (g_position);
				g_airyAudioSource.Action (AiryAudioSourceAction.Play);
			}

			public static void Play (AiryAudioSource g_airyAudioSource, Vector2 g_position) {
				g_airyAudioSource.SetPosition (g_position);
				g_airyAudioSource.Action (AiryAudioSourceAction.Play);
			}

			public static void Play (AiryAudioSource g_airyAudioSource, Transform g_parent) {
				g_airyAudioSource.SetParent (g_parent);
				g_airyAudioSource.Action (AiryAudioSourceAction.Play);
			}

			public static void SetVolume (AiryAudioSource g_airyAudioSource, float g_volume) {
				g_airyAudioSource.SetVolume (g_volume * g_airyAudioSource.GetVolume ());
			}

			public static void SetRandomVolume (AiryAudioSource g_airyAudioSource, float g_minVolume, float g_maxVolume) {
				g_airyAudioSource.SetVolume (Random.Range (g_minVolume, g_maxVolume) * g_airyAudioSource.GetVolume ());
			}

			public static void SetPitch (AiryAudioSource g_airyAudioSource, float g_pitch) {
				g_airyAudioSource.SetPitch (g_pitch);
			}

			public static void SetRandomPitch (AiryAudioSource g_airyAudioSource, float g_minPitch, float g_maxPitch) {
				g_airyAudioSource.SetPitch (Random.Range (g_minPitch, g_maxPitch));
			}

			//Edit based on Matt Bock's code
			public static float RemapRange (float g_input, float g_inputFrom, float g_inputTo, float g_outputFrom, float g_outputTo) {
				//need to test

				//make sure the value between g_inputFrom and g_inputTo;
				float t_input = g_input;
				if (g_inputFrom < g_inputTo)
					t_input = Mathf.Clamp (g_input, g_inputFrom, g_inputTo);
				else 
					t_input = Mathf.Clamp (g_input, g_inputTo, g_inputFrom);

				float t_inputRange = (g_inputTo - g_inputFrom);
				float t_outputRange = (g_outputTo - g_outputFrom);
				return (((t_input - g_inputFrom) * t_outputRange) / t_inputRange) + g_outputFrom;
			}
		}

		public class AiryAudioManager : MonoBehaviour {

			private static AiryAudioManager instance = null;

			//========================================================================
			public static AiryAudioManager Instance {
				get { 
					return instance;
				}
			}

			void Awake () {
				if (instance != null && instance != this) {
					Destroy(this.gameObject);
				} else {
					instance = this;
				}

				DontDestroyOnLoad(this.gameObject);
			}
			//========================================================================

			[SerializeField] AiryAudioBank myAiryAudioBank;
			private Dictionary<string, AiryAudioClip> myBank = new Dictionary<string, AiryAudioClip> ();

			[SerializeField] GameObject myPool_Prefab;
			[SerializeField] int myPool_Size = 50;
			private List<AiryAudioSource> myPool = new List<AiryAudioSource> ();


			[SerializeField] AudioSource myAudioSource;

			void Start () {

				// init bank
				foreach (AiryAudioBank.AiryAudioData t_data in myAiryAudioBank.GetMyBank()) {
					myBank.Add (t_data.myName, new AiryAudioClip (t_data.myAudioClip, t_data.myBaseVolume));
				}

				// init pool
				for (int i = 0; i < myPool_Size; i++) {
					myPool.Add (Instantiate (myPool_Prefab, this.transform).GetComponent<AiryAudioSource> ());
				}
			}

			public AiryAudioSource InitAudioSource (string[] g_clipNames) {
				return InitAudioSource (g_clipNames [Random.Range (0, g_clipNames.Length)]);
			}

			public AiryAudioSource InitAudioSource (string g_clipName) {
				AiryAudioClip t_clip = myBank [g_clipName];

				AiryAudioSource t_audioSource = null;
				for (int i = 0; i < myPool.Count; i++) {
					if (myPool [i].AudioSource.isPlaying == false)
						t_audioSource = myPool [i];
				}
				if (t_audioSource == null) {
					Debug.Log ("Can not find idle audio source!");
					t_audioSource = Instantiate (myPool_Prefab, this.transform).GetComponent<AiryAudioSource> ();
					myPool.Add (t_audioSource);
				}

				t_audioSource.SetAudioClip (t_clip.myAudioClip);
				t_audioSource.SetVolume (t_clip.myBaseVolume);
				t_audioSource.SetPitch (1);

				return t_audioSource;
			}

			public void PlayBGM (AudioClip g_BGM, float g_Volume = 1) {
				if (myAudioSource.isPlaying == false) {
					myAudioSource.clip = g_BGM;
					myAudioSource.volume = g_Volume;
					myAudioSource.Play ();
					return;
				} else if (g_BGM == myAudioSource.clip) {
					myAudioSource.volume = g_Volume;
					return;
				}

				myAudioSource.Stop ();
				myAudioSource.clip = g_BGM;
				myAudioSource.volume = g_Volume;
				myAudioSource.Play ();
			}

			public void StopBGM () {
				myAudioSource.Stop ();
			}


		}

	}
}
