using UnityEngine;
using System.Collections;

public static class CS_StoreList {

	public static int GetChessPrice (string g_chessName) {
		switch (g_chessName) {
			//Blood
		case "Warrior" : return 100;
		case "Berserker" : return 200;
		case "Vampire" : return 400;
		case "Sword" : return 600;
			//Magic
		case "FireMage" : return 100;
		case "IceMage" : return 100;
		case "LightMage" : return 200;
		case "AirMage" : return 400;
		case "Brush" : return 600;
			//Nature
		case "Archer" : return 100;
		case "Soprano" : return 200;
		case "Jam" : return 400;
		case "PolarBear" : return 400;
			//Light
		case "Paladin" : return 100;
		case "Avalok" : return 600;
		case "Angel" : return 200;
		case "Neptu" : return 400;
		default : return 100 ;
		}
	}

	public static int GetCoupon (string g_coupon) {
		switch (g_coupon) {
		case "euler" : return 1201;
		case "wield" : return 99;
		case "rabbit": return 1107;
		case "cuc" : return 5;

		case "soundofmystery" : return 233;
		case "holarula" : return 801;

		case "10seconds" : return 99;

		default : return -1;
		}
	}
}
