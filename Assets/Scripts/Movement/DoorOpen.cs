using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    public bool allowPassage = false;
    public Transform openPosition;
    public float speed;

    private Vector3 closePosition;
    private bool opening = false;
    private bool closing = false;
    private ObjectOutline outline;

    void Start()
    {
        closePosition = this.transform.position;
        outline = GetComponent<ObjectOutline>();
        if (outline)
            outline.enabled = false;
    }

    void Update()
    {
        if (opening)
            Open();
        if (closing)
            Close();
    }

    public void Outline()
    {
        if (outline)
            outline.enabled = true;
    }

    bool Move(Vector3 goToPosition)
    {
        // get movement delta
        float step = speed * Time.deltaTime;
        Vector3 move = Vector3.zero;
        Vector3 a = goToPosition - transform.position;
        float magnitude = a.magnitude;
        if (magnitude > step && magnitude != 0f)
            move = a / magnitude * step;

        // apply movement delta
        transform.position = transform.position + move;

        // threshold to stop moving
        if (Vector3.Distance(transform.position, goToPosition) < step)
            this.transform.position = goToPosition;

        return this.transform.position != goToPosition;
    }

    void Open()
    {
        opening = Move(openPosition.position);
        closing = false;
    }

    public void Close()
    {
        closing = Move(closePosition);
        opening = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && allowPassage)
        {
            Open();
            if (outline && outline.enabled)
                outline.enabled = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Close();
        }
    }
}
