using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleRoom2Door : MonoBehaviour
{
    public ObjectTrigger triggerOrange;
    public ObjectTrigger triggerBlue;
    public ObjectTrigger triggerGreen;
    private DoorOpen door;
    private bool passed = false;

    void Start()
    {
        door = GetComponent<DoorOpen>();
    }

    void Update()
    {
        if(!passed)
        {
            bool orange = triggerOrange.ContainsObject();
            bool blue = triggerBlue.ContainsObject();
            bool green = triggerGreen.ContainsObject();
            if(orange && blue && green){
                passed = true;
                door.Outline();
                door.allowPassage = true;
            }
        }
    }
}