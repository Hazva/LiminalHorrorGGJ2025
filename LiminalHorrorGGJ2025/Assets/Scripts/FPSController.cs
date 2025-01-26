using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

//https://www.sharpcoderblog.com/blog/unity-3d-fps-controller
public class SC_FPSController : MonoBehaviour
{
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Camera playerCamera;
    public Camera renderTexCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    [HideInInspector]
    public bool canMove = true;

    // For performance
    private bool lastMoved = false;
    private bool lastRunning = false;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // We are grounded, so recalculate move direction based on axes
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        // Press Left Shift to run
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        if (isRunning && !lastRunning)
        {
            AkSoundEngine.SetState("Movement", "Sprint");
            if (AudioManager.Instance.stressLevel != 3)
            {
                AkSoundEngine.SetState("Stress", "Med");
            }
            lastRunning = isRunning;
        }
        else if (!isRunning && lastRunning)
        {
            AkSoundEngine.SetState("Movement", "Walk");
            if (AudioManager.Instance.stressLevel != 3)
            {
                AkSoundEngine.SetState("Stress", "Small");
            }
            lastRunning = isRunning;
        }
        float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpSpeed;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);
        if (lastMoved && curSpeedX == 0 && curSpeedY == 0)
        {
            AudioManager.Instance.PostEvent("Stop_Walk");
            lastMoved = false;
        }
        else if (!lastMoved && (curSpeedX != 0 || curSpeedY != 0))
        {
            AudioManager.Instance.PostEvent("Play_Walk");
            lastMoved = true;
        }

        // Player and Camera rotation
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            renderTexCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }

        if (lastMoved)
        {
            DetermineSurface();
        }
    }

    private void DetermineSurface()
    {
        if (AudioManager.Instance.stressLevel == 3)
        {
            AkSoundEngine.SetState("Surface", "Water");
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 2.0f))
        {
            if (hit.collider.CompareTag("Carpet") && AudioManager.Instance.surface != AudioManager.Surface.Carpet)
            {
                AkSoundEngine.SetState("Surface", "Carpet");
                AudioManager.Instance.surface = AudioManager.Surface.Carpet;
            }
            else if (hit.collider.CompareTag("Shallow_Water") && AudioManager.Instance.surface != AudioManager.Surface.Shallow_Water)
            {
                AkSoundEngine.SetState("Surface", "Shallow_Water");
                AudioManager.Instance.surface = AudioManager.Surface.Shallow_Water;
            }
            else if (AudioManager.Instance.surface != AudioManager.Surface.Tile)
            {
                AkSoundEngine.SetState("Surface", "Tile");
                if (AudioManager.Instance.surface == AudioManager.Surface.Shallow_Water)
                {
                    AudioManager.Instance.PostEvent("Exit_Water");
                }
                AudioManager.Instance.surface = AudioManager.Surface.Tile;
            }
        }
    }
}