using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartDamageControll: MonoBehaviour
{
    [SerializeField] int damageMultiplyer;
    [SerializeField] DamageControll damageControll;
    [SerializeField] AudioClip headShotCling;

    public void applyDamage(int damage) {

        if (headShotCling != null)
        {
            AudioSource.PlayClipAtPoint(headShotCling, transform.position);
        }
        damageControll.TakeDamage(damageMultiplyer * damage);
    }

}
