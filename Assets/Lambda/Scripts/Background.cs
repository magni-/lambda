﻿using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour {
	
	public float angle;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(0, 0, angle);
	}
}
