using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mug : MonoBehaviour {

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

            transform.SetPositionAndRotation(
                Vector3.Lerp(transform.position, worldPosition, 0.2f),
                transform.rotation);

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
                Debug.Log("Putting mug down");
                controller.pickedMug = null;
                moveState = MoveState.MoveToSlot;
            }
        }
    }

    void OnMouseDown()
    {
        if (controller.pickedMug != null) return;

        if (moveState != MoveState.FollowMouse)
        {
            controller.pickedMug = this;
            moveState = MoveState.PickingUp;
            if (slot)
            {
                slot.GetComponent<Collider>().enabled = true;
            }
            Debug.Log("Picked up mug");
        }
    }

    const string slotTag = "MugSlot";

    GameObject slot;

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Entered collision");

        if (collision.collider.CompareTag(slotTag))
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
