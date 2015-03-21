﻿using UnityEngine;
using System.Collections;

public class BoostScript : MonoBehaviour {

	public float boostRange = 3;
	public float boostPower = 100;
	public Component cameraObject;
	public LayerMask compatibleLayer;

	public float animationDelay = 0.1f;
	public Color lineColourHit = Color.magenta;
	public Color lineColourMiss = Color.magenta;

	LineRenderer traceLine;

	// Use this for initialization
	void Start () {
		//Debug.Log ("Look what i found! " + rigidbody2D.ToString ());

		try{
			traceLine = (LineRenderer) gameObject.GetComponent("LineRenderer");
		}catch{
			Debug.Log("There is no Line Renderer attatche to this object. There will be no visual feeback.");
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Fire1") ) 
		{
			Vector2 currentPosition = new Vector2 (transform.position.x, transform.position.y);
			Vector3 mousePosition =  cameraObject.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
			Vector2 direction = new Vector2 (mousePosition.x, mousePosition.y) - currentPosition;

			RaycastHit2D rayHit = Physics2D.Raycast(currentPosition, direction, boostRange, compatibleLayer);



			if (rayHit) {
				GetComponent<Rigidbody2D>().AddForce(direction.normalized * -boostPower);
				StartCoroutine(DrawBoostTrace(currentPosition, rayHit.point, lineColourHit));
				//Debug.Log ("Boost recieved of size " + (Vector2.ClampMagnitude(direction, 1) * boostPower).magnitude.ToString() + " and value " + (Vector2.ClampMagnitude(direction, 1) * boostPower).ToString());

			}else{
				StartCoroutine(DrawBoostTrace(currentPosition, currentPosition + direction.normalized * boostRange, lineColourMiss));
			}

			//Debug.Log ("Mouse is at " + mousePosition.ToString() + " and object is at " + position.ToString() + ".");
			//Debug.Log("Resulting direction: " + direction.ToString()+ "  And the ray hit " + rayHit.transform);
		}
	}


	IEnumerator DrawBoostTrace(Vector2 origin, Vector2 destination, Color lineColour){
		Color lineColourStart = new Color(lineColour.r, lineColour.g, lineColour.b, 0);
		Color lineColourEnd = lineColour;
		traceLine.SetColors(lineColourStart, lineColourEnd);
		traceLine.SetVertexCount(2);
		traceLine.SetPosition(0, origin);
		traceLine.SetPosition(1, destination);

		yield return new WaitForSeconds (animationDelay);

		traceLine.SetVertexCount(0);

		 yield break;
	}
}
