using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    public int startingWeapon = -1;
    private WeaponInventory inventory;
    public List<int> activeWeapons;
    public GameObject hand;
    public WeaponsGUI weaponsGUI;

    public GlobalVariables gv;

    void Start()
    {
        inventory = GetComponent<WeaponInventory>();
        if (startingWeapon != -1)
        {
            SetWeapon(startingWeapon);
        } else
            hand.SetActive(false);

    }

    private int getNumberInput()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha1))
            return 1;
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha2))
            return 2;
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha3))
            return 3;
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha4))
            return 4;
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha5))
            return 5;
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha6))
            return 6;
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha7))
            return 7;
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha8))
            return 8;
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha9))
            return 9;

        return -1;
    }

    void Update()
    {
        if(!gv.freeLookCamera)
        {
            int numberInput = getNumberInput();
            if (numberInput != -1)
            {
                if (numberInput <= activeWeapons.Count)
                    SetWeapon(numberInput - 1);
            }
        }
        
    }

    public void AddWeapon(int weaponIndex)
    {
        if (!activeWeapons.Contains(weaponIndex)){
            if(weaponsGUI)
                weaponsGUI.AddWeaponGUI(weaponIndex);
            activeWeapons.Add(weaponIndex);
        }
    }

    public void SetWeapon(int position)
    {
        hand.SetActive(true);
        inventory.SwitchWeapon(activeWeapons[position]);
    }

    public void AddAndSetWeapon(int weaponIndex)
    {
        AddWeapon(weaponIndex);
        SetWeapon(activeWeapons.IndexOf(weaponIndex));
    }
}
