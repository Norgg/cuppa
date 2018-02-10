using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teabag : MonoBehaviour {

    Controller controller;

	// Use this for initialization
	void Start () {
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<Controller>();
	}

    enum MoveState
    {
        None,
        MoveIntoMug,
        PickingUp,
        FollowMouse
    };

    MoveState moveState = MoveState.None;

	// Update is called once per frame
	void Update () {
        if (moveState == MoveState.PickingUp)
        {
            var mousePosition = Input.mousePosition;
            mousePosition.z = 30;
            var worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

            transform.position = worldPosition;
            Debug.Log("Moving");

            if (Vector3.Distance(transform.position, worldPosition) < 0.1f)
            {
                moveState = MoveState.FollowMouse;
            }
        }
        else if (moveState == MoveState.MoveIntoMug)
        {
            transform.position = 
                Vector3.Lerp(
                    transform.position, 
                    controller.workingMug.transform.position, 
                    0.2f);
        }
        else if (moveState == MoveState.FollowMouse)
        {
            var mousePosition = Input.mousePosition;
            mousePosition.z = 30;
            var worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

            transform.SetPositionAndRotation(worldPosition, transform.rotation);

            if (Input.GetMouseButtonDown(0) && readyToDrop)
            {
                Debug.Log("Putting bag down");
                moveState = MoveState.MoveIntoMug;
                controller.pickedTeabag = null;
                controller.workingMug.hasTeabag = true;
            }
        }
    }

    void OnMouseDown()
    {
        if (moveState != MoveState.FollowMouse)
        {
            moveState = MoveState.PickingUp;
            controller.pickedTeabag = this;
            Debug.Log("Picked up bag");
        }
    }

    bool readyToDrop = false;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject == controller.workingMug.gameObject)
        {
            Debug.Log("Entered collision with working mug");
            readyToDrop = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.collider.gameObject == controller.workingMug.gameObject)
        {
            Debug.Log("Left collision with working mug");
            readyToDrop = false;
        }
    }
}
