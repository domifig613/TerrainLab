using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassChanger : MonoBehaviour
{
    void Start()
    {
        GetComponent<Rigidbody>().mass = 0.2f;
        StartCoroutine(ChangeMass());
    }

    private IEnumerator ChangeMass()
    {
        while (true)
        {
            yield return new WaitForSeconds(5);
            GetComponent<Rigidbody>().mass = 0.2f;
            yield return new WaitForSeconds(5);
            GetComponent<Rigidbody>().mass = 0.1f;
        }
    }

}
