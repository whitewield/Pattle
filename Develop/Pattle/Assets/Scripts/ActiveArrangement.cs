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
	[SerializeField] int myRearrangeLineCount = 2;
	[SerializeField] Vector2 myRearrangePivot = Vector2.zero;
	[SerializeField] Vector2 myRearrangePosition = Vector2.zero;
	[SerializeField] Vector2 myRearrangeDeltaPosition = Vector2.one;

	// Update is called when something changes in the scene
	void Update () {
		if (active == true) {
			switch (myMode) {
			case Mode.CreateGrid:
				CreateGrid ();
				break;
			case Mode.Rearrange:
				Rearrange ();
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
		//create grids

		if (myRearrangeMode == RearrangeMode.Horizontal) {
			int t_rowCount = myRearrangeLineCount;
			int t_columnCount = Mathf.CeilToInt ((float)myRearrangeTransforms.Length / (float)myRearrangeLineCount);

			Vector2 t_size = new Vector2 (
				                 (t_columnCount - 1) * myRearrangeDeltaPosition.x,
				                 (t_rowCount - 1) * myRearrangeDeltaPosition.y
			                 );

			Vector2 t_startPos = new Vector2 (
				                     myRearrangePosition.x - t_size.x * myRearrangePivot.x,
				                     myRearrangePosition.y + t_size.y * myRearrangePivot.y
			                     );

			Debug.Log (t_startPos);

			for (int i = 0; i < myRearrangeTransforms.Length; i++) {
				myRearrangeTransforms [i].localPosition = new Vector2 (
					t_startPos.x + i / t_rowCount * myRearrangeDeltaPosition.x,
					t_startPos.y - i % t_rowCount * myRearrangeDeltaPosition.y
				);
			}
		} else {
			int t_rowCount = Mathf.CeilToInt ((float)myRearrangeTransforms.Length / (float)myRearrangeLineCount);
			int t_columnCount = myRearrangeLineCount;

			Vector2 t_size = new Vector2 (
				                 (t_columnCount - 1) * myRearrangeDeltaPosition.x,
				                 (t_rowCount - 1) * myRearrangeDeltaPosition.y
			                 );

			Vector2 t_startPos = new Vector2 (
				                     myRearrangePosition.x - t_size.x * myRearrangePivot.x,
				                     myRearrangePosition.y + t_size.y * myRearrangePivot.y
			                     );

			for (int i = 0; i < myRearrangeTransforms.Length; i++) {
				myRearrangeTransforms [i].localPosition = new Vector2 (
					t_startPos.x + i % t_columnCount * myRearrangeDeltaPosition.x,
					t_startPos.y - i / t_columnCount * myRearrangeDeltaPosition.y
				);
			}
		}


		Debug.Log ("Rearranging!");
	}
}
