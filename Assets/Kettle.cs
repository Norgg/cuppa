using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kettle : MonoBehaviour {

    enum WaterState
    {
        Cold, Boiling, Ready
    }

    WaterState waterState = WaterState.Cold;

    public float secondsToBoil = 45;

    float secondsBoiling = 0;

    public bool IsReady { get { return waterState == WaterState.Ready; } }

    Controller controller;

    // Use this for initialization
    void Start()
    {
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<Controller>();
    }

    private void Update()
    {
        if (waterState == WaterState.Boiling)
        {
            secondsBoiling += Time.deltaTime;
            if (secondsBoiling >= secondsToBoil)
            {
                waterState = WaterState.Ready;
            }
        }
    }

    void OnMouseDown()
    {
        Debug.Log("Clicked the kettle");

        if (IsReady)
        {
            // do the pick up thing?
        }
        else if (waterState == WaterState.Cold)
        {
            waterState = WaterState.Boiling;
            GetComponent<AudioSource>().Play();
            GetComponentInChildren<ParticleSystem>().Play();
        }
    }
}
