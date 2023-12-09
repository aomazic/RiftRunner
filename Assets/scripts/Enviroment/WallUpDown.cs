using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallUpDown : MonoBehaviour
{
 [SerializeField] float speed = 2.0f;
    [SerializeField] float distance = 2.0f;
    [SerializeField] float delay = 0.0f;

    private Vector3 startPos;
    private Vector3 endPos;
    private bool movingRight = true; 
    private bool onDelay = true;

    void Start()
    {
        startPos = transform.position;
        endPos = startPos + new Vector3(0, distance, 0); 
    }

    void FixedUpdate()
    {
        StartCoroutine(MovePlatformDelayed(delay));

        if (!onDelay)
        {
            float step = speed * Time.deltaTime;
            if (movingRight)
            {
                transform.position = Vector3.MoveTowards(transform.position, endPos, step);
                if (Vector3.Distance(transform.position, endPos) < 0.001f) 
                {
                    movingRight = false;
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, startPos, step);
                if (Vector3.Distance(transform.position, startPos) < 0.001f) 
                {
                    movingRight = true;
                }
            }
        }
    }

    IEnumerator MovePlatformDelayed(float delayTime)
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
