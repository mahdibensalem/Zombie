using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CarMouvment : MonoBehaviour, IDamageable
{
    public static CarMouvment instance;
  [SerializeField]  FloatingJoystick joystick;

    public float MoveSpeed = 50;
    public float MaxSpeed = 15;
    public float SteerAngle = 20;
    Rigidbody rb;
    Vector3 movedir;
    [SerializeField] float speed;

    public float health=100;


    // Variables
    //float Drag = 0.98f;
    //float Traction = 1;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        instance = this;
    }
    public Transform GetTransform()
    {
        return transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //MoveForce += movedir;
        //transform.position += MoveForce * Time.fixedDeltaTime;
        //MoveForce *= Drag;
        //MoveForce = Vector3.Lerp(MoveForce.normalized, -movedir, Traction * Time.fixedDeltaTime) * MoveForce.magnitude;
        //rb.AddForce(movedir, ForceMode.Acceleration);

        rb.AddForce(transform.forward*((Mathf.Abs(joystick.Vertical)+ Mathf.Abs(joystick.Horizontal))%2) * MoveSpeed);
       movedir = new Vector3(joystick.Horizontal * MoveSpeed, 0, joystick.Vertical * MoveSpeed);
        if (movedir != Vector3.zero)
        {
            Quaternion toRot = Quaternion.LookRotation(movedir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRot,SteerAngle );
              rb.velocity = Vector3.ClampMagnitude(rb.velocity, MaxSpeed);
        }
        else
        {
            rb.velocity = Vector3.Lerp(rb.velocity,Vector3.zero,Time.fixedDeltaTime*speed);
        }
        //transform.rotation = Quaternion.LookRotation(rb.velocity);
        //Debug.Log(transform.forward * ((Mathf.Abs(joystick.Vertical) + Mathf.Abs(joystick.Horizontal)) % 2) * MoveSpeed);

        //// Moving


        //MoveForce += transform.forward * MoveSpeed * joystick.Vertical* Time.deltaTime;
        //transform.position += MoveForce * Time.deltaTime;

        //// Steering
        //float steerInput = joystick.Horizontal;
        //transform.Rotate(transform.up * joystick.Horizontal  * SteerAngle * Time.deltaTime);

        //// Drag and max speed limit
        //MoveForce *= Drag;


        //// Traction
        //Debug.DrawRay(transform.position, MoveForce.normalized * 3);
        //Debug.DrawRay(transform.position, transform.forward * 3, Color.blue);
        //MoveForce = Vector3.Lerp(MoveForce.normalized, transform.forward, Traction * Time.deltaTime) * MoveForce.magnitude;
    }
    public void TakeDamage( int damage)
    {
        health -= damage;
        //updateHealth();
    }
}
