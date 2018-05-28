using UnityEngine;
using System.Collections;
using Pattle.Global;

namespace Pattle {
	namespace Settings {

		[CreateAssetMenu(fileName = "LanguageSetup", menuName = "Wield/LanguageSetup", order = 1)]
		public class SO_LanguageSetup : ScriptableObject {
			public Language myLanguage;
			public Font myFont;
		}
	}
}