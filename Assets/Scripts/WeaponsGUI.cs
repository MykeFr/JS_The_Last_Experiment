using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsGUI : MonoBehaviour
{
    public Transform[] positions;
    public GameObject[] images;
    private int num = 0;

    public void AddWeaponGUI(int index){
        images[index].SetActive(true);
        images[index].transform.position = positions[num++].position;
    } 
}
