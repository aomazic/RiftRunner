using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBob : MonoBehaviour
{
    [SerializeField] private bool enable = true;

    [SerializeField, Range(0, 0.1f)] private float amplitude = 0.015f;
    [SerializeField, Range(0, 30)] private float frequency = 10.0f;

    [SerializeField] private Transform cam;
    [SerializeField] private Transform camHolder;

    private float toggleSpeed = 6.0f;
    private Vector3 startPos;
    private PlayerMovement controller;

    private void Awake()
    {
        controller = GetComponent<PlayerMovement>();
        startPos = cam.localPosition;
    }

    private void Update()
    {
        if (!enable) return;
        CheckMotion();
        
    }

    private Vector3 FocusTarget()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + cam.localPosition.y, transform.position.z);
        pos += camHolder.forward * 15.0f;
        return pos;
    }

    private void PlayMotion(Vector3 motion)
    {
        cam.localPosition += motion;
    }

    private void CheckMotion()
    {
        
        float speed = new Vector3(controller.rb.velocity.x, 0, controller.rb.velocity.z).magnitude;
        ResetPosition();
        if (speed < toggleSpeed) return;
        if (!controller.grounded || controller.wallrunning || controller.dashing || controller.isSliding) return;
        PlayMotion(FootStepMotion());
        cam.LookAt(FocusTarget());
    }

    private Vector3 FootStepMotion() 
    {
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Sin(Time.time * frequency) * amplitude;
        pos.x += Mathf.Cos(Time.time * frequency / 2) * amplitude * 2;
        return pos;
    }

    private void ResetPosition() 
    {
        if (cam.localPosition == startPos) return;
        cam.localPosition = Vector3.Lerp(cam.localPosition, startPos, 1 * Time.deltaTime);
    }
}
