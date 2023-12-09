using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class death : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        DamageControll damageController = other.GetComponent<DamageControll>();
        if (damageController != null)
        {
            damageController.TakeDamage(10000);
        }
    }
}
