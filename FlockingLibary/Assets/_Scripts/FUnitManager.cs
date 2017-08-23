using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FUnitManager : MonoBehaviour {

	public bool manualStart = false;
	public GameObject[] units;
	public GameObject unitPrefab;
	public int numUnits = 100;
	public Vector3 range = new Vector3 (10.0f, 5f, 10.0f);

	public bool seekGoal = true;
	public bool obedient = true;
	public bool willful = false;

	[Range(0,2)]
	public float maxForce = 0.5f;

	[Range(0,5)]
	public float maxvelocity = 2.0f;

	[Range(0, 1)]
	public float alignmentStrength = 0.5f;

	[Range(0, 25)]
	public float alignmentDistance = 6;

	[Range(0, 1)]
	public float cohesionStrength = 0.5f;

	[Range(0, 25)]
	public float cohesionDistance = 6;

	[Range(0, 1)]
	public float separationStrength = 0.5f;

	[Range(0, 25)]
	public float separationDistance = 5;

	[Range(0, 1)]
	public float randomizerStrength = 0.2f;

	// Update is called once per frame
	void Start () {
		units = new GameObject[numUnits];

		if (manualStart) {
			// manually find all gameobjects
			units = GameObject.FindGameObjectsWithTag("Boid");
			foreach (GameObject unit in units) {
				unit.GetComponent<FFlockingUnit> ().manager = this.gameObject;
			}

		} else {
			for (int i = 0; i < numUnits; i++) {
				Vector3 unitPos = new Vector3 (Random.Range (-range.x, range.x), 0.5f, Random.Range (-range.z, range.z));
				units [i] = Instantiate (unitPrefab, unitPos, Quaternion.LookRotation (new Vector3 (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f)))) as GameObject;
				units [i].GetComponent<FFlockingUnit> ().manager = this.transform.gameObject;
			}
		}
	}
}
