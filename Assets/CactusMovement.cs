using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CactusMovement : MonoBehaviour
{
    [SerializeField]
    private float jumpHeight;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    Vector3 rotationAngle;
    private Rigidbody _rigidBody;
    private bool _isGrounded;
    private float _distanceToGround;

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _distanceToGround = GetComponent<Collider>().bounds.extents.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(transform.position, -Vector3.up, _distanceToGround + 0.01f))
        {
            _isGrounded = true;
        }
        else
        {
            _isGrounded = false;
        }

        if(Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            _rigidBody.AddForce(Vector3.up * jumpHeight);
            _isGrounded = false;
        }
        if(Input.GetKey(KeyCode.W))
        {
            _rigidBody.MovePosition(transform.position + transform.forward * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            _rigidBody.MovePosition(transform.position - transform.forward * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            Quaternion deltaRotation = Quaternion.Euler(rotationAngle * Time.deltaTime);
            _rigidBody.MoveRotation(_rigidBody.rotation * deltaRotation);
        }
        if (Input.GetKey(KeyCode.A))
        {
            Quaternion deltaRotation = Quaternion.Euler(-rotationAngle * Time.deltaTime);
            _rigidBody.MoveRotation(_rigidBody.rotation * deltaRotation);
        }
    }
}
