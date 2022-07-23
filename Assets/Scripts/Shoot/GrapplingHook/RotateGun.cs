using UnityEngine;

public class RotateGun : MonoBehaviour {

    private Quaternion desiredRotation;
    private float rotationSpeed = 5f;
    private GrapplingHook grappling;

    void Start(){
        grappling = GetComponent<GrapplingHook>();
    }

    void Update() {
        if (!grappling.isTethered())
            desiredRotation = transform.parent.rotation;
        else
            desiredRotation = Quaternion.LookRotation(grappling.grapplePoint() - transform.position);

        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.deltaTime * rotationSpeed);
    }
}
