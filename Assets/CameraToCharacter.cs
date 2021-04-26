using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraToCharacter : MonoBehaviour
{
    [SerializeField] private Transform _transform;

    void Update()
    {
        transform.position = _transform.position + new Vector3(157.2f, 223, -74.4f);
        transform.rotation = Quaternion.LookRotation(new Vector3(46.07f, -65.7f, 0) );
    }
}
