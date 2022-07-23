using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionRoomDoor : MonoBehaviour
{
    public List<GameObject> enemies;
    private DoorOpen door;
    private bool passed;

    void Start(){
        door = GetComponent<DoorOpen>();
    }

    void Update()
    {
        if (!passed)
        {
            passed = true;
            foreach (GameObject enemy in enemies)
            {
                if (!enemy.Equals(null))
                    passed = false;
            }
            if (passed){
                door.Outline();
                door.allowPassage = true;
            }
        }
    }
}