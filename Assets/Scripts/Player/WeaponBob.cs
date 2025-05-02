using UnityEngine;

public class WeaponBob : MonoBehaviour
{
    public CharacterController player; // Assign the player's CharacterController
    public float bobSpeed = 6f; // Speed of the bobbing effect
    public float bobAmount = 0.05f; // Maximum horizontal movement
    private float bobTimer = 0f;
    private float currentBobAmount = 0f;
    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.localPosition;
    }

    void Update()
    {
        bool isMoving = Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0;

        if (isMoving) 
        {
            bobTimer += Time.deltaTime * bobSpeed; // Keep the bobbing timer running smoothly
            currentBobAmount = Mathf.Lerp(currentBobAmount, bobAmount, Time.deltaTime * 6f); // Smoothly start bobbing
        }
        else
        {
            currentBobAmount = Mathf.Lerp(currentBobAmount, 0, Time.deltaTime * 6f); // Smoothly stop bobbing
        }

        float bobOffsetX = Mathf.Sin(bobTimer) * currentBobAmount; // Horizontal sway

        transform.localPosition = originalPosition + new Vector3(bobOffsetX, 0, 0);
    }
}
