using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ActiveArrangement : MonoBehaviour {

	enum Mode {
		None,
		CreateGrid,
		Rearrange
	}

	enum RearrangeMode {
		Horizontal,
		Vertical
	}

	[SerializeField] Mode myMode = Mode.None;
	[SerializeField] bool active = false;

	[Header("CreateGrid")]
	[SerializeField] GameObject myGridPrefab;
	private List<GameObject> myGrids = new List<GameObject> ();
	[SerializeField] int myGridColumnCount = 16;
	[SerializeField] int myGridRowCount = 9;
	[SerializeField] Vector2 myGridBottomLeft = new Vector2 (-8, -4);
	[SerializeField] Vector2 myGridTopRight = new Vector2 (8, 4);
	[SerializeField] float myGridPositionZ = 10;

	[Header("Rearrange")]
	[SerializeField] Transform[] myRearrangeTransforms;
	[SerializeField] RearrangeMode myRearrangeMode = RearrangeMode.Horizontal;
	[SerializeField] Vector2 myRearrangePivot = Vector2.zero;
	[SerializeField] int myRearrangeLineCount = 2;
	[SerializeField] Vector2 myRearrangeDeltaPosition = Vector2.one;

	// Update is called when something changes in the scene
	void Update () {
		if (active == true) {
			switch (myMode) {
			case Mode.CreateGrid:
				CreateGrid ();
				break;
			case Mode.Rearrange:
//				update
				break;
			}
		}
	}

	void CreateGrid () {
		active = false;

		if (myGridPrefab == null) {
			Debug.LogError ("My Grid Prefab is empty. Put a prefab in and try again :)");
			return;
		}

		//Clear the existing grids
		foreach (GameObject t_grid in myGrids) {
			GameObject.DestroyImmediate (t_grid);
		}
		myGrids.Clear ();

		//create grids
		for (int i = 0; i < myGridRowCount; i++) {
			for (int j = 0; j < myGridColumnCount; j++) {
				Vector3 t_position = new Vector3 (
					(myGridTopRight.x - myGridBottomLeft.x) / (myGridColumnCount - 1) * j + myGridBottomLeft.x, 
					(myGridTopRight.y - myGridBottomLeft.y) / (myGridRowCount - 1) * i + myGridBottomLeft.y,
					myGridPositionZ); //Calculate the position
				GameObject t_grid = Instantiate (myGridPrefab, t_position, Quaternion.identity); //Create the grid
				myGrids.Add (t_grid); //Add it to the list
				t_grid.transform.SetParent (this.transform); //Set the parent to this gameObject
				t_grid.name = myGridPrefab.name + "(" + i + ")(" + j + ")"; //Name it
			}
		}

		Debug.Log ("Create Grid DONE!");
	}

	void Rearrange () {
		//Clear the existing grids
		foreach (GameObject t_grid in myGrids) {
			GameObject.DestroyImmediate (t_grid);
		}
		myGrids.Clear ();

		//create grids
		for (int i = 0; i < myRows; i++) {
			for (int j = 0; j < myColumns; j++) {
				Vector3 t_position = new Vector3 (
					(myTopRight.x - myBottomLeft.x) / (myColumns - 1) * j + myBottomLeft.x, 
					(myTopRight.y - myBottomLeft.y) / (myRows - 1) * i + myBottomLeft.y,
					myPositionZ); //Calculate the position
				GameObject t_grid = Instantiate (myGridPrefab, t_position, Quaternion.identity); //Create the grid
				myGrids.Add (t_grid); //Add it to the list
				t_grid.transform.SetParent (this.transform); //Set the parent to this gameObject
				t_grid.name = myGridPrefab.name + "(" + i + ")(" + j + ")"; //Name it
			}
		}

		Debug.Log ("Create Grid DONE!");
	}
}
