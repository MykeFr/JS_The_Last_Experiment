using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleRoom3Door : MonoBehaviour
{
    public Health buttonSmash;
    public Text numbersText;
    private DoorOpen door;
    private bool passed = false;

    void Start()
    {
        door = GetComponent<DoorOpen>();
    }

    void Update()
    {
        if (!passed)
        {
            numbersText.text = "" + (int)(buttonSmash.health > 0 ? buttonSmash.health : 0);
            if (!buttonSmash.isAlive())
            {
                passed = true;
                door.Outline();
                door.allowPassage = true;
            }
        }
    }
}