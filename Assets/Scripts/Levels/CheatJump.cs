using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheatJump : MonoBehaviour
{
    [SerializeField] private GameObject[] positions;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Alpha1) && !Input.GetKey(KeyCode.LeftShift))
        {
            GetComponent<PlayerMovement>().enabled = false;
            this.transform.position = positions[0].transform.position;
            this.transform.rotation = positions[0].transform.rotation;
            GetComponent<PlayerMovement>().enabled = true;
        }

        if (Input.GetKey(KeyCode.Alpha2) && !Input.GetKey(KeyCode.LeftShift))
        {
            GetComponent<PlayerMovement>().enabled = false;
            this.transform.position = positions[1].transform.position;
            this.transform.rotation = positions[1].transform.rotation;
            GetComponent<WeaponSwitching>().AddAndSetWeapon(2);
            GetComponent<PlayerMovement>().enabled = true;
        }

        if (Input.GetKey(KeyCode.Alpha3) && !Input.GetKey(KeyCode.LeftShift))
        {
            GetComponent<PlayerMovement>().enabled = false;
            this.transform.position = positions[2].transform.position;
            this.transform.rotation = positions[2].transform.rotation;
            WeaponSwitching ws = GetComponent<WeaponSwitching>();
            ws.AddWeapon(2);
            ws.AddAndSetWeapon(1);
            ws.AddWeapon(0);
            GetComponent<PlayerMovement>().enabled = true;
        }
        if (Input.GetKey(KeyCode.Alpha4) && !Input.GetKey(KeyCode.LeftShift))
        {
            GetComponent<PlayerMovement>().enabled = false;
            this.transform.position = positions[3].transform.position;
            this.transform.rotation = positions[3].transform.rotation;
            GetComponent<PlayerMovement>().enabled = true;
        }
        if (Input.GetKey(KeyCode.Alpha5) && !Input.GetKey(KeyCode.LeftShift))
        {
            GetComponent<PlayerMovement>().enabled = false;
            this.transform.position = positions[4].transform.position;
            this.transform.rotation = positions[4].transform.rotation;
            GetComponent<PlayerMovement>().enabled = true;
        }
        if (Input.GetKey(KeyCode.Alpha6) && !Input.GetKey(KeyCode.LeftShift))
        {
            GetComponent<PlayerMovement>().enabled = false;
            this.transform.position = positions[5].transform.position;
            this.transform.rotation = positions[5].transform.rotation;
            GetComponent<PlayerMovement>().enabled = true;
        }
        if (Input.GetKey(KeyCode.Alpha7) && !Input.GetKey(KeyCode.LeftShift))
        {
            GetComponent<PlayerMovement>().enabled = false;
            this.transform.position = positions[6].transform.position;
            this.transform.rotation = positions[6].transform.rotation;
            GetComponent<PlayerMovement>().enabled = true;
        }
        if (Input.GetKey(KeyCode.Alpha0) && !Input.GetKey(KeyCode.LeftShift))
        {
            if(SceneManager.GetActiveScene().name == "Tutorial") {
                SceneManager.LoadScene("LabScene");
                } else if(SceneManager.GetActiveScene().name == "LabScene") {
                    SceneManager.LoadScene("Tutorial");
                }
        }
    }
}
