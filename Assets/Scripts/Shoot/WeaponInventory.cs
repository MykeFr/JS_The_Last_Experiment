using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInventory : MonoBehaviour
{
    public List<GameObject> inventory;
    private PlayerShooter shooter;
    private GameObject currentWeapon;

    void Awake()
    {
        shooter = GetComponent<PlayerShooter>();
    }

    public int WeaponCount()
    {
        return inventory.Count;
    }

    public void SwitchWeapon(int weaponIndex)
    {
        if (currentWeapon)
        {
            Gun gunDisable = currentWeapon.GetComponent<Gun>();
            if (gunDisable)
                gunDisable.Disable();
        }

        foreach (GameObject g in inventory)
            g.SetActive(false);

        if (weaponIndex != -1)
        {
            currentWeapon = inventory[weaponIndex % inventory.Count];
            currentWeapon.SetActive(true);
            Shot projectileGun = currentWeapon.GetComponent<Shot>();
            if (projectileGun)
                shooter.gun = projectileGun;
            else
                shooter.gun = null;

            Gun gunEnable = currentWeapon.GetComponent<Gun>();
            if (gunEnable)
                gunEnable.Enable();
        }
        else
            currentWeapon = null;
    }
}