using UnityEngine;
using System.Collections;

public static class CS_AdventureReward {

	public static int REWARD_CHESS = 1;
	public static int REWARD_COINS = 2;

	public static int GetRewardType (string g_adventureName)
	{
		switch (g_adventureName) {
		case "TutorialMove" : return REWARD_CHESS;
		case "TutorialClose" : return REWARD_CHESS;
		case "TutorialSingle" : return REWARD_CHESS;
		case "TutorialSpell" : return REWARD_CHESS;
		case "TutorialDistance" : return REWARD_CHESS;
		default : return REWARD_COINS;
		}
	}

	public static string GetRewardChess (string g_adventureName)
	{
		switch (g_adventureName) {
		case "TutorialMove" : return "FireMage";
		case "TutorialClose" : return "Warrior";
		case "TutorialDistance" : return "Archer";
		case "TutorialSpell" : return "IceMage";
		case "TutorialSingle" : return "Paladin";
		default : return "Error";
		}
	}

	public static int GetRewardCoins (string g_adventureName)
	{
		switch (g_adventureName) {
		case "TutorialMove" : return 10;
		case "TutorialClose" : return 10;
		case "TutorialSingle" : return 10;
		case "TutorialSpell" : return 10;
		case "TutorialDistance" : return 10;

		case "FireDragon" : return 100;
		case "Mushroom" : return 150;
		case "IceMage" : return 150;
		case "Archer" : return 150;
		case "Brush" : return 250;
		case "Chameleon" : return 200;
		case "Neptu" : return 200;
		case "Fish" : return 200;

		default : return 100 ;
		}
	}
}
