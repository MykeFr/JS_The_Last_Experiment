using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomDoor : MonoBehaviour
{
    public BossHealthBarHover boss;
    private DoorOpen door;
    private bool passed;

    void Start(){
        door = GetComponent<DoorOpen>();
    }

    void Update()
    {
        if (!passed)
        {
            passed = !boss.isAlive();
            if (passed){
                door.Outline();
                door.allowPassage = true;
            }
        }
    }
}
