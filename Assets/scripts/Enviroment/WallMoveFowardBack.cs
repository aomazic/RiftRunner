using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMoveFowardBack : MonoBehaviour
{
    [SerializeField] float speed = 2.0f;
    [SerializeField] float distance = 2.0f;
    [SerializeField] float delay = 0.0f;

    private Vector3 startPos;
    private Vector3 endPos;
    private bool movingForward = true;
    private bool onDelay = true; 

    void Start()
    {
        startPos = transform.position;
        endPos = startPos + new Vector3(0, 0, distance);
    }

    void FixedUpdate()
    {
        StartCoroutine(MoveWallDelayed(delay));

        if (!onDelay) { 
            float step = speed * Time.deltaTime;
            if (movingForward)
            {
                transform.position = Vector3.MoveTowards(transform.position, endPos, step);
                if (transform.position == endPos)
                {
                    movingForward = false;
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, startPos, step);
                if (transform.position == startPos)
                {
                    movingForward = true;
                }
            }
        }
    }

    IEnumerator MoveWallDelayed(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        onDelay = false;   
    }

    private Transform playerTransform;
    private Transform originalParent;

    private void OnTriggerEnter(Collider other)
    {

        playerTransform = other.transform;
        originalParent = playerTransform.parent;
        playerTransform.parent = transform;

    }

    private void OnTriggerExit(Collider other)
    {
        playerTransform.parent = originalParent;
    }
}
