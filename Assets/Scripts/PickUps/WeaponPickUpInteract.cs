using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class WeaponPickUpInteract : MonoBehaviour
{
    public GameObject canvasTooltip;
    public GameObject canvasPickUpAudioTextCanvas;
    public KeyCode pickupKey;
    public GameObject player;
    public int weaponIndex;
    public GameObject prefab;
    private bool insideRange = false;
    private WeaponSwitching playerInventory;

    void Start(){
        playerInventory = player.GetComponent<WeaponSwitching>();
    }

    void Update()
    {
        if (insideRange && Input.GetKeyDown(pickupKey))
        {
            playerInventory.AddAndSetWeapon(weaponIndex);
            prefab.SetActive(false);
            canvasTooltip.SetActive(false);
            TutorialAudioText.Instance.tutorialStageAudioText(weaponIndex);
        }
    }

    void OnTriggerEnter(Collider other){
        if(other.CompareTag(player.tag)){
            insideRange = true;
            canvasTooltip.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other){
        if(other.CompareTag(player.tag)){
            insideRange = false;
            canvasTooltip.SetActive(false);
        }
    }
}
