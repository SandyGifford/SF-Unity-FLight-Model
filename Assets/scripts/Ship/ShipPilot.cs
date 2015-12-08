﻿using UnityEngine;
using System.Collections;

public class ShipPilot : ShipComponent {

	protected override void Start () {
		base.Start ();
	}

	public virtual float getThrottleModifier() {
		return 1;
	}

	public virtual Vector2 getHeading() {
		return new Vector2 (0, 0);
	}
}
