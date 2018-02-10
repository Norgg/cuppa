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
        MoveToSlot,
        PickingUp,
        FollowMouse
    };

    MoveState moveState = MoveState.MoveToSlot;

	// Update is called once per frame
	void Update () {
        if (moveState == MoveState.PickingUp)
        {
            var mousePosition = Input.mousePosition;
            mousePosition.z = 30;
            var worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

            transform.position = worldPosition;
            /*transform.SetPositionAndRotation(
                Vector3.Lerp(transform.position, worldPosition, 0.2f),
                transform.rotation);*/
            Debug.Log("Moving");

            if (Vector3.Distance(transform.position, worldPosition) < 0.1f)
            {
                moveState = MoveState.FollowMouse;
            }
        }
        else if (
            moveState == MoveState.MoveToSlot &&
            slot != null)
        {
            transform.SetPositionAndRotation(
                Vector3.Lerp(transform.position, slot.transform.position, 0.2f),
                transform.rotation);

            slot.GetComponent<Collider>().enabled = false;
        }
        else if (moveState == MoveState.FollowMouse)
        {
            var mousePosition = Input.mousePosition;
            mousePosition.z = 30;
            var worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

            transform.SetPositionAndRotation(worldPosition, transform.rotation);

            if (Input.GetMouseButtonDown(0) && slot)
            {
                Debug.Log("Putting bag down");
                controller.pickedMug = null;
                moveState = MoveState.MoveToSlot;
            }
        }
    }

    void OnMouseDown()
    {
        if (moveState != MoveState.FollowMouse)
        {
            moveState = MoveState.PickingUp;
            if (slot)
            {
                slot.GetComponent<Collider>().enabled = true;
            }
            Debug.Log("Picked up bag");
        }
    }

    const string mugTag = "MugSlot";

    GameObject slot;

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Entered collision");

        if (collision.collider.CompareTag(mugTag))
        {
            Debug.Log("Colliding with a slot");

            slot = collision.collider.gameObject;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        Debug.Log("Left collision");

        if (collision.collider.gameObject == slot)
        {
            Debug.Log("Stopped colliding with a slot");
            slot = null;
        }
    }
}
