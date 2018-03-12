using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Attributes", menuName = "Wield/Attributes", order = 1)]
public class SO_Attributes : ScriptableObject {

	public string name;

	/// <summary>
	/// Hit Point.
	/// </summary>
	public int HP = 8;

	/// <summary>
	/// Cool Down.
	/// </summary>
	public int CD = 3;

	/// <summary>
	/// Casting Time.
	/// </summary>
	public int CT = 0;

	/// <summary>
	/// Physical Damage.
	/// </summary>
	public int PD = 1;

	/// <summary>
	/// Physical Resistance.
	/// </summary>
	public int PR = 0;

	/// <summary>
	/// Magic Damage.
	/// </summary>
	public int MD = 0;


}