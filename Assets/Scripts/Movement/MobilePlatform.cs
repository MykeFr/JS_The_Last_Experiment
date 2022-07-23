using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobilePlatform : MonoBehaviour
{
    public List<Transform> positions;
    public float speed;
    public bool waitForPlayer = false;

    private Vector3 goToPosition;
    private int currentIndex;

    private CharacterController characterController;
    private bool playerHasEntered = false;

    void Start()
    {
        this.transform.position = positions[0].position;
        goToPosition = positions[1].position;
        currentIndex = 1;
    }

    void Update()
    {
        if (waitForPlayer && !playerHasEntered)
            return;

        // get movement delta
        float step = speed * Time.deltaTime;
        Vector3 move = Vector3.zero;
        Vector3 a = goToPosition - transform.position;
        float magnitude = a.magnitude;
        if (magnitude > step && magnitude != 0f)
            move = a / magnitude * step;

        // apply movement delta to platform and player
        transform.position = transform.position + move;
        if (characterController)
            characterController.Move(move);

        // threshold to change destination
        if (Vector3.Distance(transform.position, goToPosition) < step)
            this.transform.position = goToPosition;

        // change destination
        if (this.transform.position == goToPosition)
            goToPosition = positions[(++currentIndex) % positions.Count].position;
    }

    void OnTriggerEnter(Collider other)
    {
        CharacterController c = other.GetComponent<CharacterController>();
        if (c)
        {
            playerHasEntered = true;
            characterController = c;
        }
        //other.gameObject.transform.SetParent(this.transform, true);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<CharacterController>())
            characterController = null;
        //other.gameObject.transform.SetParent(null);
    }
}