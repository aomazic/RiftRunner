using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    public GameObject[] fpsArmsPrefabs;
    public bool rifle, shootgun;
    int currentWeaponIndex = 0;

    private GameObject currentFpsArms;

    void Start()
    {
        EquipRifle();
    }

    void Update()
    {
        WeaponController();
    }

    private void WeaponController()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && currentWeaponIndex != 0)
        {
            EquipRifle();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && currentWeaponIndex != 1)
        {
            EquipShootgun();
        }
    }

    void EquipRifle()
    {
        currentWeaponIndex = 0;
        shootgun = false;
        rifle = true;
        EquipWeapon(0);
    }
    void EquipShootgun()
    {
        currentWeaponIndex = 1;
        shootgun = true;
        rifle = false;
        EquipWeapon(1);
    }




    void EquipWeapon(int index)
    {
        if (currentFpsArms != null)
        {
            Destroy(currentFpsArms);
        }
        Vector3 cameraPos = Camera.main.transform.position;
        cameraPos += Camera.main.transform.right * -0.1f; 
        cameraPos += Camera.main.transform.up * -1.812f;
        cameraPos += Camera.main.transform.forward * 0.01f;
        currentFpsArms = Instantiate(fpsArmsPrefabs[index], cameraPos, Camera.main.transform.rotation, Camera.main.transform);

    }
}
