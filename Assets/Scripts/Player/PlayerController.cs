using UnityEngine;
using UnityEngine.VFX; 

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 5f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;
    public float mouseSensitivity = 2f;
    public float tiltModifier = 1f;
    public float slideSpeedMultiplier = 2f;
    public float slideHeight = 0.5f;
    public float normalHeight = 2f;
    public float cameraSlideOffset = 0.5f; // How much the camera lowers

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    public bool isSliding = false;
    
    [Header ("Particles")]
    public VisualEffect slideVFX;

    [Header("Camera")]
    public Transform playerCamera; // Main camera (with tilt)
    public Transform weaponCamera; // Overlay camera (no tilt)
    private float xRotation = 0f;
    private float zTilt = 0f;
    private Vector3 originalCameraPosition;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Store original camera height
        originalCameraPosition = playerCamera.localPosition;
    }

    void Update()
    {
        HandleMovement();
        HandleMouseLook();
        HandleSlide();
    }

    void HandleMovement()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Normal movement
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        controller.Move(move * speed * Time.deltaTime);

        // Slide movement boost (only forward)
        if (isSliding)
        {
            Vector3 slideMove = transform.forward * speed * slideSpeedMultiplier * Time.deltaTime;
            controller.Move(slideMove);
        }

        // Jumping
        if (Input.GetButtonDown("Jump") && isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Smoothly tilt the primary camera when strafing
        float targetTilt = moveX * tiltModifier;
        zTilt = Mathf.Lerp(zTilt, targetTilt, Time.deltaTime * 20f);
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Rotate the camera up/down
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Apply rotation & tilt to MainCamera
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, -zTilt);

        // Copy rotation but remove Z tilt for WeaponCamera
        weaponCamera.rotation = Quaternion.Euler(xRotation, transform.eulerAngles.y, 0f);

        // Rotate the player left/right
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleSlide()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isSliding = true;
            controller.height = slideHeight;
            playerCamera.localPosition = new Vector3(originalCameraPosition.x, originalCameraPosition.y - cameraSlideOffset, originalCameraPosition.z);
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            isSliding = false;
            controller.height = normalHeight;
            playerCamera.localPosition = originalCameraPosition;
        }

        if(isSliding && controller.isGrounded)
        {
            slideVFX.Play();
        }
        else 
        {
            slideVFX.Stop();
        }
    }
}
