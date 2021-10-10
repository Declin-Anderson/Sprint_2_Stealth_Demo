using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Rigidbody M_Rigidbody;
    public float M_Speed = 0.5f;
    Animator Animator;

    void Start()
    {
        //Fetch the Rigidbody from the GameObject with this script attached
        M_Rigidbody = GetComponent<Rigidbody>();
        Animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        //Store user input as a movement vector
        Vector3 M_Input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        //Apply the movement vector to the current position, which is
        //multiplied by deltaTime and speed for a smooth MovePosition
        if (M_Input != new Vector3(0, 0, 0))
        {
            M_Rigidbody.MovePosition(transform.position + M_Input * Time.deltaTime * M_Speed);
            Animator.SetBool("Walking", true);
        }
        else {
            Animator.SetBool("Walking", false);
        }
    }
}
