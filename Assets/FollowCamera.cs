using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    Camera camera;
    private void Start()
    {
        camera = Camera.main;
    }
    void LateUpdate()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - camera.transform.position);
    }
}
