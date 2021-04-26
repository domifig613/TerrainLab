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
    private Rigidbody rigidBody;
    private bool isGrounded;
    private float distanceToGround;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        distanceToGround = GetComponent<Collider>().bounds.extents.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(transform.position, -Vector3.up, distanceToGround + 0.01f))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rigidBody.AddForce(Vector3.up * jumpHeight);
            isGrounded = false;
        }
        if(Input.GetKey(KeyCode.W))
        {
            rigidBody.MovePosition(transform.position + transform.forward * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            rigidBody.MovePosition(transform.position - transform.forward * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            Quaternion deltaRotation = Quaternion.Euler(rotationAngle * Time.deltaTime);
            rigidBody.MoveRotation(rigidBody.rotation * deltaRotation);
        }
        if (Input.GetKey(KeyCode.A))
        {
            Quaternion deltaRotation = Quaternion.Euler(-rotationAngle * Time.deltaTime);
            rigidBody.MoveRotation(rigidBody.rotation * deltaRotation);
        }
    }
}
