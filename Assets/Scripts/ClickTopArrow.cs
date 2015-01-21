﻿using UnityEngine;
using System.Collections;

public class ClickTopArrow : MonoBehaviour {

	private GameObject player;
	private Vector2 up;
	void Update() {
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if ((Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Began) || (Input.GetMouseButtonDown(0))) 
		{
			if (Physics.Raycast(ray, out hit)) {
				if (hit.transform.tag == "arrow_top" )
					Debug.Log( "TOP");
			}
		}
	}

}
