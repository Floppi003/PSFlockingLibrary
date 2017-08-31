using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*! An example class that implements random movement for a goal GameObject, which will be followed by units. */
public class PSGoalRandomMovement : MonoBehaviour {

	/*! The speed of the camera movmeent */
	public float speed = 1.0f;

	private Vector3 moveDirection = Vector3.zero;
	private float timeUntilNewDirection = 0.0f;
	private float maximumTimeUntilNewDirection = 3.0f;

	
	/**
	 * @brief Called periodically from Unity. Do not call manually.
	 */
	void Update () {

		// move forward
		this.transform.position += this.moveDirection;

		// make new move direction
		this.timeUntilNewDirection -= Time.deltaTime;
		if (this.timeUntilNewDirection < 0.0f) {
			this.timeUntilNewDirection = Random.Range (0.2f, this.maximumTimeUntilNewDirection);
			moveDirection = new Vector3 (Random.Range (0.0f, 2.0f) - 1.0f, 0.0f, Random.Range (0.0f, 2.0f) - 1.0f).normalized * this.speed * Time.deltaTime;
		}
	}
}
