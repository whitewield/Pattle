using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pattle.Adventure;
using UnityEngine.UI;
using PT_Global;

public class PT_AdventureButton : MonoBehaviour {

	private Button myButton;

	[SerializeField] AdventureSetup mySetup;
	[SerializeField] RectTransform myButtonFace;

	[SerializeField] Image myMedal_Image;
	[SerializeField] Text myMedal_Text;

	private PT_AdventureMenuCanvas myCanvas;

	void Awake () {
		myButton = this.GetComponent<Button> ();

	}

	// Use this for initialization
	void Start () {
		myCanvas = PT_AdventureMenuCanvas.Instance;

		myMedal_Image.color = PT_AdventureMenuCanvas.Instance.GetMedalColor (MedalType.Bronze);
		myMedal_Text.text = mySetup.myUnlockMedalCount.ToString ("#");
	}

	public void Init (GameObject g_medalPrefab, MedalType[] g_medalTypes) {
		Instantiate (mySetup.myMenuDisplayPrefab, myButtonFace);

		PT_AdventureButton_Medals t_medals = Instantiate (g_medalPrefab, myButtonFace).GetComponent<PT_AdventureButton_Medals> ();
		t_medals.Show (g_medalTypes);
	}

	public void CheckLock (int g_medalCount) {
		if (g_medalCount < mySetup.myUnlockMedalCount)
			myButton.interactable = false;
	}

//	// Update is called once per frame
//	void Update () {
//		
//	}

	public BossType GetBossType () {
		return mySetup.myBossType;
	}


	public void OnButtonPressed () {
		myCanvas.OnButtonAdventure (mySetup);
	}
}

