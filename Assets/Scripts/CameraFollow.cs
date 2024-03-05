using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset;
    public float sensitivity = 2.0f;
    public float maxYAngle = 80.0f;

    private float rotationX = 0.0f;

    void Start()
    {
        //offset = transform.position;
        //Debug.Log(offset);
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        transform.parent.Rotate(Vector3.up * mouseX * sensitivity);

        rotationX -= mouseY * sensitivity;
        rotationX = Mathf.Clamp(rotationX, -maxYAngle, maxYAngle);
        transform.localRotation = Quaternion.Euler(rotationX, 0.0f, 0.0f);
    }

    void LateUpdate() 
    {
        //transform.position = player.transform.position + offset;    
    }
}

