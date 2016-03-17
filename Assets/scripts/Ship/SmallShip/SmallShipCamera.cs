﻿using UnityEngine;
using System.Collections;

public class SmallShipCamera : SmallShipComponent {

	    ////////////////////////
	   ////                ////
	  ////   Properties   ////
	 ////                ////
	////////////////////////

	  ////////////////////////
	 //  public            //
	////////////////////////

	public Transform cameraTarget;
	public Vector2 lookAhead = new Vector2(1, 1);

	public float verticalCameraDrift = 2;

	public Camera cam { get; protected set; }

	  ////////////////////////
	 //  protected         //
	////////////////////////

	  ////////////////////////
	 //  private           //
	////////////////////////
	
	private Color healthFGColor = new Color(1,    0, 0) ;
	private Color healthBGColor = new Color(0.5f, 0, 0) ;
	private float barLength     = 200                   ;
	private float barHeight     = 10                    ;

	    ////////////////////////
	   ////                ////
	  ////     Unity      ////
	 ////                ////
	////////////////////////
	
	protected override void Start () {
		if (cameraTarget == null)
			throw new MissingReferenceException ("No Camera Target specified");
		
		cam = Camera.main;
		
		base.Start ();
	}
	
	protected virtual void Update () {
		updateCameraPosition ();
		updateCameraRotation ();
	}
	
	private void OnGUI() {
		DrawQuad (new Rect (10, 10, barLength, barHeight), healthBGColor);
		DrawQuad (new Rect (10, 10, barLength * ship.health.healthFrac, barHeight), healthFGColor);
	}
	
	    ////////////////////////
	   ////                ////
	  ////    Methods     ////
	 ////                ////
	////////////////////////

	  ////////////////////////
	 //  public            //
	////////////////////////

	  ////////////////////////
	 //  protected         //
	////////////////////////
	
	protected virtual void updateCameraPosition() {
		
		// Simple follow
		float verticalDrift = gameManager.heightFraction(transform.position.y) * verticalCameraDrift;
		cam.transform.position = cameraTarget.position - Vector3.up * verticalDrift;
		
		// Lag follow
		/*
		Vector3 pDiff = cameraTarget.position - cam.transform.position;

		if (pDiff.magnitude > 1)
			pDiff.Normalize ();

		cam.transform.position += pDiff / 2f;
		*/
		
	}
	
	protected virtual void updateCameraRotation() {
		float hrFrac = transformControlFractions(ship.control.getHorizontalFraction());
		float vrFrac = transformControlFractions(ship.control.getVerticalFraction());

		// Camera rotate
		cam.transform.forward = cameraTarget.forward;

		// Rotational lag
		cam.transform.RotateAround(transform.position, Vector3.up, -hrFrac * lookAhead.x);
		
		// Look ahead
		cam.transform.Rotate (hrFrac * lookAhead.x * transform.up - vrFrac * lookAhead.y * transform.right);		
	}

	  ////////////////////////
	 //  private           //
	////////////////////////

	private float transformControlFractions(float frac) {
//		if(frac > 0.5)
//			return -Mathf.Cos(Mathf.PI * (2 * frac + 1)) + 1;
//		else if(frac < -0.5)
//			return Mathf.Cos(Mathf.PI * (2 * frac + 1)) - 1;
		if(frac < -0.25 || frac > 0.25)
			return Mathf.SmoothStep(0f, 1.0f, Mathf.Abs(frac) - 0.25f) * Mathf.Sign(frac);

		return 0;
	}
	
	private void DrawQuad(Rect position, Color color) {
		Texture2D texture = new Texture2D(1, 1);
		texture.SetPixel(0,0,color);
		texture.Apply();
		GUI.skin.box.normal.background = texture;
		GUI.Box(position, GUIContent.none);
	}
}
