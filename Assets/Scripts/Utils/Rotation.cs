using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    public Vector3 axis = Vector3.up;
    public float speedDegrees = 90f;

    void Update()
    {
        transform.Rotate(axis, speedDegrees * Time.deltaTime, Space.Self);
    }
}
