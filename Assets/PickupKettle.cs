using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupKettle : MonoBehaviour {

    Controller controller;
    Kettle kettle;
    Quaternion startRotation;
    public GameObject pourTarget;
    public ParticleSystem waterParticles;

    Vector3 startPos;

	// Use this for initialization
	void Start () {
        startRotation = transform.rotation;
        kettle = GetComponent<Kettle>();
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<Controller>();
	}

    enum MoveState
    {
        Resting,
        PickingUp,
        FollowMouse,
        PuttingBack
    };

    MoveState moveState = MoveState.Resting;

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
        else if (moveState == MoveState.FollowMouse)
        {
            var mousePosition = Input.mousePosition;
            mousePosition.z = 30;
            var worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

            float targetDistance = Vector3.Distance(pourTarget.transform.position, transform.position);
            targetDistance /= 3;
            targetDistance = Mathf.Clamp(targetDistance, 1, 100);
            float rotationAmount = 45 * 1 / targetDistance;
            if (targetDistance >= 1.5) {
                rotationAmount = 0;
            }

            if (rotationAmount > 0) {
                if (!waterParticles.isPlaying) {
                    waterParticles.Play();
                    controller.workingMug.Fill();
                }
            } else {
                waterParticles.Stop();
            }

            Quaternion tipRotation = startRotation * Quaternion.Euler(rotationAmount, 0, 0);

            transform.position = worldPosition;
            transform.rotation = Quaternion.Slerp(transform.rotation, tipRotation, 0.1f);

            if (Input.GetMouseButtonDown(0))
            {
                moveState = MoveState.PuttingBack;
            }
        }
        else if (moveState == MoveState.PuttingBack)
        {
            transform.position = Vector3.Lerp(transform.position, startPos, 0.2f);
            transform.rotation = Quaternion.Slerp(transform.rotation, startRotation, 0.5f);

            if (Vector3.Distance(transform.position, startPos) < 0.05f)
            {
                moveState = MoveState.Resting;
                waterParticles.Stop();
                transform.rotation = startRotation;
            }
        }
    }

    void OnMouseDown()
    {
        if (moveState != MoveState.FollowMouse && kettle.IsReady)
        {
            startPos = transform.position;
            moveState = MoveState.PickingUp;
            if (slot)
            {
                slot.GetComponent<Collider>().enabled = true;
            }
            Debug.Log("Picked up kettle");
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
