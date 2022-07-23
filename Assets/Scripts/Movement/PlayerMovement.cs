using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private GameMaster gameMaster;
    public CharacterController controller;
    public Animator animator2;

    public float maxSpeed = 40f;
    public Vector3 velocity;
    public Vector3 _velocity;
    private bool stopGravity = false;

    // Normal movement
    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public Transform groundCheck;
    public Transform roofCheck;
    public float groudDistance = 0.2f;
    public LayerMask groundMask;
    public bool isGrounded = false;

    // Wall running
    public bool canWallRun = true;
    public bool isWallRunning = false;

    // camera transitions
    public Camera FPScamera;
    public float transitionSpeed = 2f;
    public float cameraRotation = 13f;
    public float fieldOfView = 100f;
    public Transform firstPersonTransform;
    public Transform thirdPersonTransform;
    private Transform currentCameraTransform;
    public GameObject thirdPersonModel;

    // Double Jump
    public bool canDoubleJump = true;
    private bool hasDoubleJumped = false;

    public GlobalVariables gv;

    void ClampVelocity()
    {
        if (_velocity.magnitude > maxSpeed)
        {
            Vector3 newVelocity = _velocity.normalized;
            newVelocity *= maxSpeed;
            _velocity = newVelocity;
        }
    }

    void Start()
    {
        if(gameMaster.check)
        {
            transform.position = gameMaster.restartPos;
            transform.rotation = gameMaster.restartRot;
        }
        if (thirdPersonModel)
            thirdPersonModel.SetActive(false);
        SwitchToFirstPerson();
    }

    void Update()
    {
        if (!gv.freeLookCamera)
        {
            NormalMovement();
            if (canWallRun)
                WallRun();
        }

    }

    public void Jump(float jumpMultiplier)
    {
        _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity * jumpMultiplier);
    }

    void NormalMovement()
    {
        if (controller.enabled)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groudDistance, groundMask, QueryTriggerInteraction.Ignore);

            (float x, float z) = InputHandler.GetMoveInput();
            Vector3 move = transform.right * x + transform.forward * z;

            // apply a lower gravity when grounded
            if (isGrounded && _velocity.y < 0)
            {
                _velocity.x = Mathf.Lerp(_velocity.x, 0f, Time.deltaTime * 3.0f);
                _velocity.z = Mathf.Lerp(_velocity.z, 0f, Time.deltaTime * 3.0f);
                _velocity.y = -2f;
            }

            if (animator2)
            {
                GrapplingHook gh = GetComponentInChildren<GrapplingHook>();
                if (gh != null)
                {
                    animator2.SetBool("isGrapling", gh.tethered);
                }

                animator2.SetBool("isGrounded", isGrounded);
                animator2.SetBool("isFalling", !isGrounded && _velocity.y < 0);
                animator2.SetBool("isRunning", move != Vector3.zero);
                animator2.SetBool("jumped", false);
            }

            controller.Move(move * speed * Time.deltaTime);

            if (InputHandler.JumpInputDown() && isGrounded)
                Jump(1f);

            if (canDoubleJump)
                DoubleJump();

            if (!stopGravity)
                _velocity.y += gravity * Time.deltaTime;

            _velocity += velocity;
            ClampVelocity();
            controller.Move(_velocity * Time.deltaTime);
        }
    }

    void DoubleJump()
    {
        if (isGrounded || (isWallRunning && canWallRun))
            hasDoubleJumped = false;

        if (!hasDoubleJumped && InputHandler.JumpInputDown())
        {
            Jump(1f);
            if (animator2)
            {
                animator2.SetBool("jumped", true);
            }

            hasDoubleJumped = true;
        }
    }

    void OnControllerColliderHit(ControllerColliderHit other)
    {
        if (other.collider.tag == "RunnableWall" && InputHandler.IsMovingForward())
        {
            isWallRunning = true;
        }
        else
        {
            // stop when hitting the roof or a wall
            bool roofHit = Physics.CheckSphere(roofCheck.position, groudDistance, groundMask, QueryTriggerInteraction.Ignore);
            if (roofHit)
                _velocity.y = 0f;
            else
            {
                _velocity.x = 0f;
                _velocity.z = 0f;
            }
        }
    }

    void WallRun()
    {
        if (!isWallRunning || isGrounded)
        {
            isWallRunning = false;
            stopGravity = false;
        }
        // movement
        if (isWallRunning)
        {
            // disable gravity when wall running
            stopGravity = true;

            // only apply gravity if a jump occured or something else
            if (_velocity.y > 0f)
                _velocity.y += gravity * Time.deltaTime / 2f;

            if (InputHandler.JumpInputDown())
                Jump(1.5f);

            // disable gravity on wall
            if (_velocity.y < 0f)
                _velocity.y = 0f;
        }

        // camera
        if (isWallRunning)
        {
            FPScamera.fieldOfView = Mathf.Lerp(FPScamera.fieldOfView, fieldOfView, Time.deltaTime * transitionSpeed);
            if (Physics.Raycast(transform.position, transform.right, 1f, groundMask))
            {
                // rotate camera to the left
                FPScamera.transform.rotation = Quaternion.Slerp(FPScamera.transform.rotation,
                    Quaternion.Euler(FPScamera.transform.eulerAngles.x, FPScamera.transform.eulerAngles.y, cameraRotation), Time.deltaTime * transitionSpeed);
                // to avoid cliping through the wall
                FPScamera.transform.position = Vector3.Lerp(FPScamera.transform.position,
                    firstPersonTransform.position + firstPersonTransform.right / -6f, Time.deltaTime * transitionSpeed);
            }
            if (Physics.Raycast(transform.position, -transform.right, 1f, groundMask))
            {
                // rotate camera to the right
                FPScamera.transform.rotation = Quaternion.Slerp(FPScamera.transform.rotation,
                    Quaternion.Euler(FPScamera.transform.eulerAngles.x, FPScamera.transform.eulerAngles.y, -cameraRotation), Time.deltaTime * transitionSpeed);
                // to avoid cliping through the wall
                FPScamera.transform.position = Vector3.Lerp(FPScamera.transform.position,
                    firstPersonTransform.position + firstPersonTransform.right / 6f, Time.deltaTime * transitionSpeed);
            }
        }
        else
        {
            // Lerp camera to correct place
            FPScamera.fieldOfView = Mathf.Lerp(FPScamera.fieldOfView, 90, Time.deltaTime * transitionSpeed);
            FPScamera.transform.position = Vector3.Lerp(FPScamera.transform.position, currentCameraTransform.position, Time.deltaTime * transitionSpeed);
            FPScamera.transform.rotation = Quaternion.Slerp(FPScamera.transform.rotation,
                    Quaternion.Euler(FPScamera.transform.eulerAngles.x, FPScamera.transform.eulerAngles.y, 0f), Time.deltaTime * transitionSpeed);

            // disable 3d model when switching to first person
            if (thirdPersonModel && Vector3.Distance(FPScamera.transform.position, firstPersonTransform.position) < 0.3
                && currentCameraTransform.position == firstPersonTransform.position)
                thirdPersonModel.SetActive(false);
        }

        isWallRunning = false;
    }

    public void SwitchToThirdPerson()
    {
        if (thirdPersonModel)
            thirdPersonModel.SetActive(true);
        currentCameraTransform = thirdPersonTransform;
    }

    public void SwitchToFirstPerson()
    {
        currentCameraTransform = firstPersonTransform;
    }
}
