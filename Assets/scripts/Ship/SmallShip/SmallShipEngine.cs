﻿using UnityEngine;
using System.Collections;

public class SmallShipEngine : SmallShipComponent {
	
	protected SmallShip smallShip {
		get {
			return ship as SmallShip;
		}
	}

	public float velocity = 5f;
	public float minMultiplier = 0.5f;
	public float maxMultiplier = 2.0f;
	
	void Update () {
		ship.rb.velocity = velocity * transform.forward * Mathf.Clamp(ship.pilot.getThrottleModifier (), minMultiplier, maxMultiplier);
	}
}
