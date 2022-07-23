using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickUp : MonoBehaviour
{
    public int weaponIndex;
    private WeaponSwitching playerInventory;
    public AudioSource sound;

    void Start()
    {
        if (!playerInventory)
            playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<WeaponSwitching>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !other.attachedRigidbody)
        {
            sound.PlayOneShot(sound.clip);
            playerInventory.AddAndSetWeapon(weaponIndex);
        }
    }
}
