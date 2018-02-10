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

        if (waterState == WaterState.Cold)
        {
            waterState = WaterState.Boiling;
            GetComponent<AudioSource>().Play();
            GetComponentInChildren<ParticleSystem>().Play();
        }
    }
}
