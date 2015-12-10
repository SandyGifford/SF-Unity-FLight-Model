﻿using UnityEngine;
using System.Collections;

public class ShipControl : ShipComponent {
	// TODO: smooth turning should maybe be its own controller
	public float maxRoll = 20;
	public float maxPitch = 10;
	public float horizontalAngularAcceleration = 3;
	public float verticalAcceleration = 4;
	public float maxHorizontalAngularVelocity = 3;
	public float maxVerticalVelocity = 3;
	public readonly float MIN_HEADING = -1;
	public readonly float MAX_HEADING = 1;

	private float horizontalAngularVelocity = 0;
	private float verticalVelocity = 0;

	protected override void Start () {
		base.Start ();
	}

	void Update () {
		Vector2 targetHeading = clampVector(ship.pilot.getHeading (), MIN_HEADING, MAX_HEADING);

		updateHorizontalAngularVelocity (targetHeading.x);
		updateVerticalVelocity (targetHeading.y);

		Debug.DrawRay (transform.position, 5 * horizontalAngularVelocity * transform.right / maxHorizontalAngularVelocity, Color.red);
		Debug.DrawRay (transform.position, 5 * verticalVelocity * transform.up / maxVerticalVelocity, Color.green);

		ship.rollObj.localRotation = Quaternion.Euler (-1 * (verticalVelocity / maxVerticalVelocity) * maxPitch, 0, -1 * (horizontalAngularVelocity / maxHorizontalAngularVelocity) * maxRoll);
		//Debug.Log (Quaternion.Euler( (horizontalAngularVelocity, Vector3.up));
		//Debug.Log (horizontalAngularVelocity);
		//ship.rb.MoveRotation(Quaternion.AngleAxis(horizontalAngularVelocity, Vector3.up));
		//ship.rb.MoveRotation(transform.rotation * Quaternion.AngleAxis(horizontalAngularVelocity * Time.deltaTime, Vector3.up));
		ship.rb.angularVelocity = horizontalAngularVelocity * Time.deltaTime * Vector3.up;
		ship.rb.MovePosition(transform.position + new Vector3(0, verticalVelocity * Time.deltaTime, 0));
	}
	
	private Vector2 clampVector(Vector2 vec, float min, float max) {
		return new Vector2 (Mathf.Clamp (vec.x, min, max), Mathf.Clamp (vec.y, min, max));
	}

	private void updateHorizontalAngularVelocity(float horizontalHeading) {
		float targetAngV = horizontalHeading * maxHorizontalAngularVelocity;

		float delta = targetAngV - horizontalAngularVelocity;
		horizontalAngularVelocity += delta * horizontalAngularAcceleration * Time.deltaTime;
	}

	private void updateVerticalVelocity(float verticalHeading) {
		float targetV = verticalHeading * maxVerticalVelocity;

		float delta = targetV - verticalVelocity;
		verticalVelocity += delta * verticalAcceleration * Time.deltaTime;
	}
}
