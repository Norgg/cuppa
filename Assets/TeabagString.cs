using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeabagString : MonoBehaviour {

	public GameObject bag;
	public GameObject tag;
	LineRenderer line;

	void Start() {
		line = GetComponent<LineRenderer>();
	}
	
	void Update() {
		line.SetPosition(0, bag.transform.position + bag.transform.forward * transform.localScale.y );
		line.SetPosition(1, tag.transform.position - tag.transform.forward * transform.localScale.y / 3);
	}
}
