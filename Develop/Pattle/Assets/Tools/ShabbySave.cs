//Created by Wang Rui
//Reorganized by Ruan Hang 160128
//160215update : ResetGameSave ()

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Text;

public static class ShabbySave {

	public static string myGameName = "Pattle";

	public static void CreatGameSave () {
		if (!File.Exists(GetPath ())) {

			XmlDocument t_xmlDoc = new XmlDocument();
			
			//create root node
			XmlElement t_root = t_xmlDoc.CreateElement(myGameName + "Data");
			t_xmlDoc.AppendChild(t_root);

			//save file
			t_xmlDoc.Save (GetPath ());
			
			Debug.Log("createGameSaveData!");
			return;
		}
		Debug.Log("GameSaveData exist!");
		return;
	}

	public static void ResetGameSave () {
		XmlDocument t_xmlDoc = new XmlDocument();
		
		//create root node
		XmlElement t_root = t_xmlDoc.CreateElement(myGameName + "Data");
		t_xmlDoc.AppendChild(t_root);
		
		//save file
		t_xmlDoc.Save (GetPath ());
		
		Debug.Log("ResetGameSaveData!");
		return;
	}
	
	public static void SaveGame(string g_category, string g_title, string g_data) {

//		Debug.Log ("Save Game Start!");

		if (!File.Exists (GetPath ())) {
			CreatGameSave ();
		}

		//load xml
		XmlDocument t_xmlDoc = new XmlDocument ();
		t_xmlDoc.Load(GetPath ());

		//get root node
		XmlNode t_gameRoot = t_xmlDoc.SelectSingleNode(myGameName + "Data");

		//get category list
		XmlNodeList t_categoryList = t_gameRoot.ChildNodes;

		//go through category list
		for (int i = 0; i < t_categoryList.Count; i++) {

			//if category exist
			if (t_categoryList[i].Name == g_category) {

				//get title list
				XmlNodeList t_titleList = t_categoryList[i].ChildNodes;

				//go through title list
				for (int j = 0; j < t_titleList.Count; j++) {

					//if title exsit
					if (t_titleList[j].Name == g_title) {

						//update data
						t_titleList[j].InnerText = g_data;

						//save file
						t_xmlDoc.Save (GetPath ());

						Debug.Log("update this Save!");
						return;
					}
				}

				//if title does not exist, create title
				XmlElement t_newTitle = t_xmlDoc.CreateElement(g_title);

				t_newTitle.InnerText = g_data;

				t_categoryList[i].AppendChild(t_newTitle);

				//save file
				t_xmlDoc.Save (GetPath ());

				Debug.Log("create a new title!");
				return;
			}
		}

		//if category does not exsit, 
		XmlElement t_new_Category = t_xmlDoc.CreateElement(g_category);
		XmlElement t_new_Title = t_xmlDoc.CreateElement(g_title);

		t_new_Title.InnerText = g_data;

		t_new_Category.AppendChild(t_new_Title);
		t_gameRoot.AppendChild(t_new_Category);
		t_xmlDoc.AppendChild(t_gameRoot);

		Debug.Log("create a new category!");

		//save file
		t_xmlDoc.Save (GetPath ());

		return;	
			
	}
	
	public static string LoadGame (string g_category, string g_title) {
//		Debug.Log("Load Game Start!");
		
		if (!File.Exists (GetPath ())) {
			CreatGameSave ();
		}

		//load file
		XmlDocument t_xmlDoc = new XmlDocument();
		t_xmlDoc.Load (GetPath ());

		//get category list
		XmlNodeList t_categoryList = t_xmlDoc.SelectSingleNode(myGameName + "Data").ChildNodes;

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
						return titleElement.InnerText;
					//return int.Parse(titleElement.InnerText);
				}

				//can not find title in this category, return
				return "0";
			}
		}

		//can not find category, return
		return "0";
	}
	
	private static string GetPath () {
		if (Application.platform == RuntimePlatform.Android) {
			return (Application.persistentDataPath + "//ShabbySave.xml");
		} else if (Application.platform == RuntimePlatform.IPhonePlayer) {
			return (Application.persistentDataPath + "//ShabbySave.xml");
		} else {
			return (Application.dataPath + @"/ShabbySave.xml");
		}
	}
}
