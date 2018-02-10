using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeabagString : MonoBehaviour {

	public GameObject bagbag;
	public GameObject bagtag;
	LineRenderer line;

	void Start() {
		line = GetComponent<LineRenderer>();
	}
	
	void Update() {
		line.SetPosition(0, bagbag.transform.position + bagbag.transform.forward * transform.localScale.y );
		line.SetPosition(1, bagtag.transform.position - bagtag.transform.forward * transform.localScale.y / 3);
	}
}
