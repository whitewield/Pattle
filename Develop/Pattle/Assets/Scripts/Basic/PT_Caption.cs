using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Text;
using Pattle.Global;
using Pattle.Settings;

public class PT_Caption : MonoBehaviour {

	private static PT_Caption instance = null;
	public static PT_Caption Instance { get { return instance; } }

	[SerializeField] string myGameName = "Pattle";
	[SerializeField] SO_LanguageSetup[] myLanguageSetups;
	private Language myLanguage;
	private Dictionary<Language, SO_LanguageSetup> myLanguageDictionary;

	private XmlDocument xmlDoc;


	void Awake () {
		if (instance != null && instance != this) {
			Destroy(this.gameObject);
		} else {
			instance = this;
		}

		DontDestroyOnLoad(this.gameObject);
		LoadCaptionLanguage ();
		InitLanguageDictionary ();
	}

//	void Start () {
//	}

	public void SetCaptionLanguage (string g_language) {
		ShabbySave.SaveGame (Constants.SAVE_CATEGORY_SETTINGS, Constants.SAVE_TITLE_LANGUAGE, g_language);
	}

	private void InitLanguageDictionary () {
		myLanguageDictionary = new Dictionary<Language, SO_LanguageSetup> ();
		foreach (SO_LanguageSetup f_setup in myLanguageSetups) {
			myLanguageDictionary.Add (f_setup.myLanguage, f_setup);
		}
	}

	public void LoadCaptionLanguage () {

		string t_language = ShabbySave.LoadGame (Constants.SAVE_CATEGORY_SETTINGS, Constants.SAVE_TITLE_LANGUAGE);

		if (t_language == "0") {
			//set default language
			switch (Application.systemLanguage) { 
//			case SystemLanguage.ChineseSimplified:
//				t_language = Constants.LANGUAGE_CHS;
//				break;
//			case SystemLanguage.ChineseTraditional:
//				t_language = Constants.LANGUAGE_CHT;
//				break;
//			case SystemLanguage.Chinese:
//				t_language = Constants.LANGUAGE_CHS;
//				break;
			case SystemLanguage.English:
				myLanguage = Language.EN;
				break;
			default :
				myLanguage = Language.EN;
				break;
			}

			ShabbySave.SaveGame (Constants.SAVE_CATEGORY_SETTINGS, Constants.SAVE_TITLE_LANGUAGE, myLanguage.ToString ());
		} else {
			myLanguage = (Language)System.Enum.Parse (typeof(Language), t_language);
		}

		Debug.Log ("load caption language : " + myLanguage);
		xmlDoc = new XmlDocument();
		xmlDoc.LoadXml (Resources.Load<TextAsset> ("Caption_" + myLanguage.ToString ()).ToString ());
	}

	public Font GetFont () {
		return myLanguageDictionary [myLanguage].myFont;
	}

	public string LoadCaption (string g_category, string g_title) {
		//get category list
		XmlNodeList t_categoryList = xmlDoc.SelectSingleNode(myGameName + "Data").ChildNodes;

		//go through category list
		foreach (XmlElement categoryElement in t_categoryList) {

			//if category exist
			if (categoryElement.Name == g_category) {

				//get title list
				XmlNodeList t_titleList = categoryElement.ChildNodes;

				//go through title list
				foreach (XmlElement titleElement in t_titleList) {

					//if title exsit, return data
					if (titleElement.Name == g_title)
						return titleElement.InnerText.Replace("\\r\\n", System.Environment.NewLine);
					//return titleElement.InnerText;
					//return int.Parse(titleElement.InnerText);
				}

				//can not find title in this category, return
				Debug.Log ("can not find title : " + g_title);
				return "0";
			}
		}

		//can not find category, return
		Debug.Log ("can not find category : " + g_category);
		return "0";
	}
}
