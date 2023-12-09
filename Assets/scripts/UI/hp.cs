using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class hp : MonoBehaviour
{
    [SerializeField] DamageControll dm;
    private Slider slider;
    void Start()
    {
        slider = GetComponent<Slider>();
    }

    void Update() { 

    slider.value = dm.health;
    }
}
