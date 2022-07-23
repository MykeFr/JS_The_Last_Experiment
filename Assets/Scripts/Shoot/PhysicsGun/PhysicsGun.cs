using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PhysicsGun : Gun
{
    private Vector3 _oneVector3 = Vector3.one;
    private Vector3 _zeroVector2 = Vector2.zero;
    private Vector3 _zeroVector3 = Vector3.zero;

    private Vector3 _forward;
    private Vector3 _up;
    private Vector3 _right;

    private Vector3 _rotationInput = Vector3.zero;
    [SerializeField] private float _rotationSenstivity = 1.5f;
    public float maxVelocity = 50f;

    [SerializeField] private Camera _camera;
    [SerializeField] private LayerMask _grabLayer;

    private bool _userRotation;

    /// <summary>The rigidbody we are currently holding</summary>
    public Rigidbody _grabbedRigidbody;
    /// <summary>The transfor of the rigidbody we are holding</summary>
    private Transform _grabbedTransform;
    /// <summary>The offset vector from the object's position to hit point, in local space</summary>
    private Vector3 _hitOffsetLocal;
    /// <summary>The interpolation state when first grabbed</summary>
    private RigidbodyInterpolation _initialInterpolationSetting;
    /// <summary>The difference between player & object rotation, updated when picked up or when rotated by the player</summary>
    private Quaternion _rotationDifference;
    /// <summary>The distance we are holding the object at</summary>
    private float _currentGrabDistance;

    /// <summary>The start point for the Laser. This will typically be on the end of the gun</summary>
    [SerializeField] private Transform _laserStartPoint;

    public Transform PlayerTransform;

    private Vector3 _scrollWheelInput = Vector3.zero;
    private float _scrollWheelSensitivity = 20f;

    private float _minObjectDistance = 2.5f;
    [SerializeField] private float _maxGrabDistance = 50f;
    private bool _distanceChanged;

    private bool _wasKinematic;

    private float _rotationSpeed = 5f;
    private Quaternion _desiredRotation = Quaternion.identity;

    //public properties for the Line Renderer
    public Vector3 StartPoint { get; private set; }
    public Vector3 MidPoint { get; private set; }
    public Vector3 EndPoint { get; private set; }

    public GlobalVariables gv;

    public class GrabEvent : UnityEvent<GameObject> { };
    public GrabEvent OnObjectGrabbed;

    public AudioClip audioBegin;
    public AudioClip audioMiddle;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public override void Disable()
    {
        if (_grabbedRigidbody)
            ReleaseObject();
    }

    // Update is called once per frame
    void Update()
    {
        if (!audioSource.isPlaying && _grabbedRigidbody)
        {
            audioSource.clip = audioMiddle;
            audioSource.loop = true;
            audioSource.volume = 1;
            audioSource.Play();
        }
        if (audioSource.isPlaying && !_grabbedRigidbody)
        {
            audioSource.volume = Mathf.Lerp(audioSource.volume, 0, Time.deltaTime * 3f);
        }

        if (!gv.freeLookCamera)
        {
            if (InputHandler.Fire1Down() && !_grabbedRigidbody)
            {
                Ray ray = CenterRay();
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, _maxGrabDistance, _grabLayer, QueryTriggerInteraction.Ignore))
                {
                    // Don't pick up kinematic rigidbodies (they can't move)
                    if (hit.rigidbody != null /*&& !hit.rigidbody.isKinematic*/)
                    {
                        audioSource.clip = audioBegin;
                        audioSource.loop = false;
                        audioSource.volume = 1;
                        audioSource.Play();

                        // Track rigidbody's initial information
                        _grabbedTransform = hit.transform;
                        _grabbedRigidbody = hit.rigidbody;
                        _wasKinematic = _grabbedRigidbody.isKinematic;
                        _grabbedRigidbody.isKinematic = false;
                        _grabbedRigidbody.freezeRotation = true;
                        _initialInterpolationSetting = _grabbedRigidbody.interpolation;
                        _rotationDifference = Quaternion.Inverse(PlayerTransform.rotation) * _grabbedRigidbody.rotation;
                        _hitOffsetLocal = hit.transform.InverseTransformVector(hit.point - hit.transform.position);
                        _currentGrabDistance = hit.distance; // Vector3.Distance(ray.origin, hit.point);
                        _grabbedTransform = _grabbedRigidbody.transform;
                        // Set rigidbody's interpolation for proper collision detection when being moved by the player
                        _grabbedRigidbody.interpolation = RigidbodyInterpolation.Interpolate;

                        var outline = _grabbedRigidbody.gameObject.GetComponent<ObjectOutline>();
                        if (!outline)
                        {
                            outline = _grabbedRigidbody.gameObject.AddComponent<ObjectOutline>();
                            outline.OutlineMode = ObjectOutline.Mode.OutlineAll;
                            outline.OutlineColor = Color.blue;
                            outline.OutlineWidth = 6f;
                        }
                        outline.enabled = true;
                        Knockback knockback = hit.transform.gameObject.GetComponent<Knockback>();
                        if (knockback != null)
                            knockback.Grab();
                    }
                }
            }
            if (InputHandler.Fire1Pressed() && _grabbedRigidbody)
            {
                Interaction();
            }

            if (InputHandler.Fire1Up() && _grabbedRigidbody)
            {
                ReleaseObject();
            }
        }

    }

    void Interaction()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            _grabbedTransform.rotation = Quaternion.identity;
            _rotationDifference = Quaternion.Inverse(PlayerTransform.rotation) * _grabbedRigidbody.rotation;
        }
        if (Input.GetKey(KeyCode.E))
        {
            _grabbedTransform.Rotate(PlayerTransform.forward * _rotationSenstivity * Time.deltaTime, Space.World);
            _rotationDifference = Quaternion.Inverse(PlayerTransform.rotation) * _grabbedRigidbody.rotation;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            _grabbedTransform.Rotate(-PlayerTransform.forward * _rotationSenstivity * Time.deltaTime, Space.World);
            _rotationDifference = Quaternion.Inverse(PlayerTransform.rotation) * _grabbedRigidbody.rotation;
        }
        if (Input.GetKey(KeyCode.R))
        {
            _grabbedTransform.Rotate(PlayerTransform.right * _rotationSenstivity * Time.deltaTime, Space.World);
            _rotationDifference = Quaternion.Inverse(PlayerTransform.rotation) * _grabbedRigidbody.rotation;
        }
        if (Input.GetKey(KeyCode.F))
        {
            _grabbedTransform.Rotate(-PlayerTransform.right * _rotationSenstivity * Time.deltaTime, Space.World);
            _rotationDifference = Quaternion.Inverse(PlayerTransform.rotation) * _grabbedRigidbody.rotation;
        }

        var direction = Input.GetAxisRaw("Mouse ScrollWheel");

        if (Mathf.Abs(direction) > 0 && CheckObjectDistance(direction))
        {
            _distanceChanged = true;
            _scrollWheelInput = PlayerTransform.forward * _scrollWheelSensitivity * direction;
        }
    }

    private void FixedUpdate()
    {
        if (!gv.freeLookCamera)
        {
            if (_grabbedRigidbody)
            {
                Ray ray = CenterRay();

                var intentionalRotation = Quaternion.AngleAxis(_rotationInput.z, _forward) * Quaternion.AngleAxis(_rotationInput.y, _right) * Quaternion.AngleAxis(-_rotationInput.x, _up) * _desiredRotation;
                var relativeToPlayerRotation = PlayerTransform.rotation * _rotationDifference;

                _desiredRotation = _userRotation ? intentionalRotation : relativeToPlayerRotation;

                // Remove all torque, reset rotation input & store the rotation difference for next FixedUpdate call
                _grabbedRigidbody.angularVelocity = _zeroVector3;
                _rotationInput = _zeroVector2;
                _rotationDifference = Quaternion.Inverse(PlayerTransform.rotation) * _desiredRotation;

                // Calculate object's center position based on the offset we stored
                // NOTE: We need to convert the local-space point back to world coordinates
                // Get the destination point for the point on the object we grabbed
                var holdPoint = ray.GetPoint(_currentGrabDistance) + _scrollWheelInput;
                _scrollWheelInput = _zeroVector3;
                var centerDestination = holdPoint - _grabbedTransform.TransformVector(_hitOffsetLocal);

#if UNITY_EDITOR
                Debug.DrawLine(ray.origin, holdPoint, Color.blue, Time.fixedDeltaTime);
#endif
                // Find vector from current position to destination
                var toDestination = centerDestination - _grabbedTransform.position;

                // Calculate force
                var force = toDestination / Time.fixedDeltaTime * 0.3f / _grabbedRigidbody.mass;

                if (force.magnitude > maxVelocity)
                    force = force.normalized * maxVelocity;

                //force += _scrollWheelInput;
                // Remove any existing velocity and add force to move to final position
                _grabbedRigidbody.velocity = _zeroVector3;
                _grabbedRigidbody.AddForce(force, ForceMode.Impulse);

                //Rotate object
                RotateGrabbedObject();

                //We need to recalculte the grabbed distance as the object distance from the player has been changed
                if (_distanceChanged)
                {
                    _distanceChanged = false;
                    _currentGrabDistance = Vector3.Distance(ray.origin, holdPoint);
                }

                //Update public properties
                StartPoint = _laserStartPoint.transform.position;
                MidPoint = holdPoint;
                EndPoint = _grabbedTransform.TransformPoint(_hitOffsetLocal);
            }
        }


    }

    private void RotateGrabbedObject()
    {
        if (_grabbedRigidbody == null)
            return;

        _grabbedRigidbody.MoveRotation(Quaternion.Lerp(_grabbedRigidbody.rotation, _desiredRotation, Time.fixedDeltaTime * _rotationSpeed));
    }

    private Ray CenterRay()
    {
        return _camera.ViewportPointToRay(_oneVector3 * 0.5f);
    }

    //Check distance is within range when moving object with the scroll wheel
    private bool CheckObjectDistance(float direction)
    {
        var pointA = PlayerTransform.position;
        var pointB = _grabbedRigidbody.position;

        var distance = Vector3.Distance(pointA, pointB);

        if (direction > 0)
            return distance <= _maxGrabDistance;

        if (direction < 0)
            return distance >= _minObjectDistance;

        return false;
    }

    private void ReleaseObject()
    {
        var outline = _grabbedRigidbody.gameObject.GetComponent<ObjectOutline>();
        outline.enabled = false;

        Knockback knockback = _grabbedRigidbody.gameObject.GetComponent<Knockback>();
        if (knockback != null)
            knockback.Release();

        //Move rotation to desired rotation in case the lerp hasn't finished
        // Reset the rigidbody to how it was before we grabbed it
        _grabbedRigidbody.isKinematic = _wasKinematic;
        _grabbedRigidbody.interpolation = _initialInterpolationSetting;
        _grabbedRigidbody.freezeRotation = false;
        _grabbedRigidbody = null;
        _scrollWheelInput = _zeroVector3;
        _grabbedTransform = null;
        _userRotation = false;
        StartPoint = _zeroVector3;
        MidPoint = _zeroVector3;
        EndPoint = _zeroVector3;
    }
}
