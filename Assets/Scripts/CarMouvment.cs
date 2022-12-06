using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMouvment : MonoBehaviour
{
  [SerializeField]  FloatingJoystick joystick;

    public float MoveSpeed = 50;
    public float MaxSpeed = 15;
    public float Drag = 0.98f;
    public float SteerAngle = 20;
    public float Traction = 1;

    // Variables
    private Vector3 MoveForce;

    /// <summary>
    /// /////////////////////////////////////
    /// </summary>
   [SerializeField] Rigidbody rb;
    Vector3 move;
    Vector3 movedir;
    private void Start()
    {

    }


    // Update is called once per frame
    void FixedUpdate()
    {
       movedir = new Vector3(joystick.Horizontal * MoveSpeed, 0, joystick.Vertical * MoveSpeed);
        //MoveForce += movedir;
        //transform.position += MoveForce * Time.fixedDeltaTime;
        //MoveForce *= Drag;
        //MoveForce = Vector3.Lerp(MoveForce.normalized, -movedir, Traction * Time.fixedDeltaTime) * MoveForce.magnitude;
        rb.AddForce(transform.forward*((Mathf.Abs(joystick.Vertical)+ Mathf.Abs(joystick.Horizontal))%2) * MoveSpeed);
        //rb.AddForce(movedir, ForceMode.Acceleration);
        if (movedir != Vector3.zero)
        {
            Quaternion toRot = Quaternion.LookRotation(movedir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRot,SteerAngle );
        }
        //transform.rotation = Quaternion.LookRotation(rb.velocity);


        //// Moving


        //MoveForce += transform.forward * MoveSpeed * joystick.Vertical* Time.deltaTime;
        //transform.position += MoveForce * Time.deltaTime;

        //// Steering
        //float steerInput = joystick.Horizontal;
        //transform.Rotate(transform.up * joystick.Horizontal  * SteerAngle * Time.deltaTime);

        //// Drag and max speed limit
        //MoveForce *= Drag;
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, MaxSpeed);

        //// Traction
        //Debug.DrawRay(transform.position, MoveForce.normalized * 3);
        //Debug.DrawRay(transform.position, transform.forward * 3, Color.blue);
        //MoveForce = Vector3.Lerp(MoveForce.normalized, transform.forward, Traction * Time.deltaTime) * MoveForce.magnitude;
    }
}
