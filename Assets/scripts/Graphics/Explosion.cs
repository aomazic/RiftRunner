using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] ParticleSystem exp; 
    void Start()
    {
        exp.Play();
        Destroy(gameObject, exp.main.duration);

    }
}
