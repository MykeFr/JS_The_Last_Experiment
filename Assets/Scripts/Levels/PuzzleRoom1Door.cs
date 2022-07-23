using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleRoom1Door : MonoBehaviour
{
    public ObjectTrigger trigger;
    public List<ObjectOutline> coloredFrames;
    private DoorOpen door;
    private bool passed = false;

    void Start()
    {
        door = GetComponent<DoorOpen>();
        foreach (ObjectOutline obj in coloredFrames)
            obj.OutlineColor = Color.red;
    }

    void Update()
    {
        if (!passed)
        {
            passed = trigger.ContainsObject();
            if (passed)
            {
                door.Outline();
                foreach (ObjectOutline obj in coloredFrames)
                    obj.OutlineColor = Color.green;
                door.allowPassage = true;
            }
        }
    }
}