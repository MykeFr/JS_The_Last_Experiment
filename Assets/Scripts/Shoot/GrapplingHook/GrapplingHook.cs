using UnityEngine;

public class GrapplingHook : Gun
{
    public bool tethered = false;
    private bool traveling = false;

    private float tetherLength;
    private Vector3 tetherPoint;
    private GameObject tetheredObject;
    private Vector3 tetheredObjectTransformOld;

    public PlayerMovement pm;
    public Camera cam;
    public Transform gunTip;

    public float maxDistance = 70f;
    public float minDistance = 4f;
    public float speed = 20f;
    public float minSpeed = 10f;
    public float fieldOfViewDiff = 30f;
    public float transitionSpeed = 2f;
    public float ropeTravelSpeed = 100f;

    private LineRenderer lr;
    private float old_FOV;
    private Vector3 ropeTip;

    public GameObject SpeedLines;

    public GlobalVariables gv;

    public AudioClip audioBegin;
    public AudioClip audioMiddle;
    private AudioSource audioSource;
    private bool loopMiddle;

    void Start()
    {
        lr = GetComponent<LineRenderer>();
        audioSource = GetComponent<AudioSource>();
        loopMiddle = audioSource.loop;
    }

    public override void Disable()
    {
        if (tethered || traveling)
            EndGrapple();
    }

    void Update()
    {
        if (!gv.freeLookCamera)
        {
            if (!audioSource.isPlaying && tethered)
            {
                audioSource.clip = audioMiddle;
                audioSource.loop = loopMiddle;
                audioSource.Play();
            }

            if (InputHandler.Fire1Down() && !tethered && !traveling)
                BeginGrapple();
            else if (InputHandler.Fire1Up() && (traveling || tethered))
                EndGrapple();

            if (Vector3.Distance(gunTip.position, tetherPoint) < minDistance && (traveling || tethered))
                EndGrapple();

            if (traveling)
            {
                UpdateTetherPoint();
                TravelRope();
            }

            if (tethered)
            {
                ApplyCameraTransitions();
                DrawRope();
            }
        }

    }

    void FixedUpdate()
    {
        if (tethered) ApplyGrapplePhysics();
    }

    void BeginGrapple()
    {
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, maxDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            traveling = true;

            tetherPoint = hit.point;
            ropeTip = gunTip.position;
            tetherLength = Vector3.Distance(tetherPoint, cam.transform.position);
            tetheredObject = hit.transform.gameObject.gameObject;
            tetheredObjectTransformOld = tetheredObject.transform.position;

            old_FOV = cam.fieldOfView;

            lr.positionCount = 2;

            if (Vector3.Distance(gunTip.position, tetherPoint) >= minDistance)
            {
                audioSource.clip = audioBegin;
                audioSource.loop = false;
                audioSource.Play();
            }
        }
    }

    void EndGrapple()
    {
        if (audioSource.clip != audioBegin)
            audioSource.Stop();

        tethered = false;
        traveling = false;
        pm.velocity = Vector3.zero;
        lr.positionCount = 0;

        cam.fieldOfView = old_FOV;

        if (SpeedLines)
            SpeedLines.SetActive(false);
    }

    void TravelRope()
    {
        // same as moving platform
        float step = ropeTravelSpeed * Time.deltaTime;
        Vector3 move = Vector3.zero;
        Vector3 a = tetherPoint - ropeTip;
        float magnitude = a.magnitude;
        if (magnitude > step && magnitude != 0f)
            move = a / magnitude * step;

        ropeTip = ropeTip + move;

        if (Vector3.Distance(ropeTip, tetherPoint) < step)
        {
            traveling = false;
            tethered = true;
            if (SpeedLines)
                SpeedLines.SetActive(true);
        }

        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, ropeTip);
    }

    void DrawRope()
    {
        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, tetherPoint);
    }

    void ApplyCameraTransitions()
    {
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, old_FOV + fieldOfViewDiff, Time.deltaTime * transitionSpeed);
    }

    void UpdateTetherPoint()
    {
        tetherPoint += tetheredObject.transform.position - tetheredObjectTransformOld;
        tetheredObjectTransformOld = tetheredObject.transform.position;
    }

    void ApplyGrapplePhysics()
    {
        UpdateTetherPoint();
        Vector3 directionToGrapple = Vector3.Normalize(tetherPoint - transform.position);
        float currentDistanceToGrapple = Vector3.Distance(tetherPoint, transform.position);
        float vel = Mathf.Max((currentDistanceToGrapple / tetherLength) * speed, minSpeed);

        pm.velocity = directionToGrapple * vel;
    }

    public bool isTethered()
    {
        return tethered;
    }

    public Vector3 grapplePoint()
    {
        return tetherPoint;
    }
}