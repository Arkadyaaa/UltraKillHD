using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    public PlayerController playerController;

    public AudioClip[] footstepSounds; 
    public float stepRate = 0.5f;

    public AudioClip slidingSound;

    private CharacterController controller;
    private AudioSource audioSource;
    private float stepCooldown;

    private bool isSlidingSoundPlaying = false; // Track sliding sound state

    void Start()
    {
        controller = GetComponentInParent<CharacterController>(); // Get player controller
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        HandleFootsteps();
        HandleSlidingSound();
    }

    void HandleFootsteps()
    {
        if (IsMoving() && !IsSliding() && controller.isGrounded)
        {
            if (Time.time >= stepCooldown)
            {
                PlayFootstep();
                stepCooldown = Time.time + stepRate;
            }
        }
    }

    void HandleSlidingSound()
    {
        if (IsSliding() && controller.isGrounded)
        {
            if (!isSlidingSoundPlaying) // Prevent constant replaying
            {
                audioSource.clip = slidingSound;
                audioSource.loop = true;
                audioSource.Play();
                isSlidingSoundPlaying = true;
            }
        }
        else
        {
            if (isSlidingSoundPlaying)
            {
                audioSource.Stop();
                isSlidingSoundPlaying = false;
            }
        }
    }

    bool IsMoving()
    {
        return Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0;
    }

    bool IsSliding()
    {
        return playerController.isSliding;
    }

    void PlayFootstep()
    {
        if (footstepSounds.Length > 0)
        {
            int randomIndex = Random.Range(0, footstepSounds.Length);
            audioSource.PlayOneShot(footstepSounds[randomIndex]);
        }
    }
}
