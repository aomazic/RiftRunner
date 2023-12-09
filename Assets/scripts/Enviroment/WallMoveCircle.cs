using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMove : MonoBehaviour
{
    [SerializeField] float speed = 2.0f;
    [SerializeField] float radius = 2.0f;
    [SerializeField] float angle = 0.0f;
    [SerializeField] bool direction;

    private Vector3 center;

    void Start()
    {
        center = transform.position;
    }

    void Update()
    {
        if (direction)
            angle = (angle + speed * Time.deltaTime) % 360.0f;
        else angle = (angle - speed * Time.deltaTime) % 360.0f;

        float x = Mathf.Sin(angle) * radius;
        float z = Mathf.Cos(angle) * radius;

        transform.position = center + new Vector3(x, 0, z);
        transform.LookAt(center);
        transform.rotation *= Quaternion.Euler(0, 90, 0);

    }
}
