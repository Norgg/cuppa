using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kettle : MonoBehaviour {

    enum WaterState
    {
        Cold, Boiling, Ready
    }

    WaterState waterState = WaterState.Cold;

    private void Update()
    {
        if (waterState == WaterState.Boiling)
        {

        }
    }

    void OnMouseDown()
    {
        Debug.Log("Clicked the kettle");

        if (waterState == WaterState.Cold)
        {
            waterState = WaterState.Boiling;
            GetComponent<AudioSource>().Play();
        }
    }
}
