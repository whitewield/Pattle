using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Text;

public class CS_Caption : MonoBehaviour {

	private static CS_Caption instance = null;

	public string myGameName = "Pattle";

	public XmlDocument xmlDoc;

	//========================================================================
	public static CS_Caption Instance {
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

	void Start () {
		LoadCaptionLanguage ();
	}

	public void SetCaptionLanguage (string g_language) {
		CS_GameSave.SaveGame (CS_Global.SAVE_CATEGORY_SETTINGS, CS_Global.SAVE_TITLE_LANGUAGE, g_language);
	}

	public void LoadCaptionLanguage () {

		string t_language = CS_GameSave.LoadGame (CS_Global.SAVE_CATEGORY_SETTINGS, CS_Global.SAVE_TITLE_LANGUAGE);

		if (t_language == "0") {
			//set default language
			switch (Application.systemLanguage) { 
			case SystemLanguage.ChineseSimplified:
				t_language = CS_Global.LANGUAGE_CHS;
				break;
			case SystemLanguage.ChineseTraditional:
				t_language = CS_Global.LANGUAGE_CHT;
				break;
			case SystemLanguage.Chinese:
				t_language = CS_Global.LANGUAGE_CHS;
				break;
			case SystemLanguage.English:
				t_language = CS_Global.LANGUAGE_EN;
				break;
			default :
				t_language = CS_Global.LANGUAGE_EN;
				break;
			}

			CS_GameSave.SaveGame (CS_Global.SAVE_CATEGORY_SETTINGS, CS_Global.SAVE_TITLE_LANGUAGE, t_language);
		}

		Debug.Log("load caption language : " + t_language);
		xmlDoc = new XmlDocument();
		xmlDoc.LoadXml (Resources.Load<TextAsset> ("Caption_" + t_language).ToString ());
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
