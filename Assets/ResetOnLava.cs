using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetOnLava : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            ResetGame();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "lava")
        {
            ResetGame();
        }
    }
    
    private void ResetGame()
    {
        this.transform.position = new Vector3(59.12f, 42.04f, -4.72f);
        //this.transform.rotation = Q
    }
}
