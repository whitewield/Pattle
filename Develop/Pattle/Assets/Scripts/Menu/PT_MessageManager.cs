using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hang.AiryAudio;

public class PT_MessageManager : MonoBehaviour {
	
	protected static PT_MessageManager instance = null;
	public static PT_MessageManager Instance { get { return instance; } }

	public enum BoxSize {
		Short,
		Long,
	}

	[SerializeField] GameObject myCanvas;
	[SerializeField] PT_MessageManager_Box myBox_Short;
	[SerializeField] PT_MessageManager_Box myBox_Long;
	private PT_MessageManager_Box myCurrentBox;

	[SerializeField] AiryAudioData myAiryAudioData_Hide;

	void Awake () {
		if (instance != null && instance != this) {
			Destroy(this.gameObject);
		} else {
			instance = this;
			DontDestroyOnLoad (this.gameObject);
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ShowMessage (string g_text, BoxSize g_size) {

		switch (g_size) {
		case BoxSize.Short:
			myCurrentBox = myBox_Short;
			break;
		case BoxSize.Long:
			myCurrentBox = myBox_Long;
			break;
		}
		myCurrentBox.ShowText (g_text);

		myCanvas.SetActive (true);
	}

	public void OnButtonHide () {
		if (myAiryAudioData_Hide != null)
			myAiryAudioData_Hide.Play ();

		myCanvas.SetActive (false);

		if (myCurrentBox != null)
			myCurrentBox.Hide ();
	}
}
