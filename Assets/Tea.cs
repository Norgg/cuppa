using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tea : MonoBehaviour {
	Material teaMaterial;
	Color startColor;
	public Color doneColor;

	public float brewTime = 2;
	public float brewTimer = 0;

	void Start() {
		teaMaterial = GetComponent<Renderer>().material;
		startColor = teaMaterial.color;
	}
	
	void Update() {
		if (brewTimer < brewTime) {
			brewTimer += Time.deltaTime;
		}
		teaMaterial.color = Color.Lerp(startColor, doneColor, brewTimer/brewTime);
	}
}
