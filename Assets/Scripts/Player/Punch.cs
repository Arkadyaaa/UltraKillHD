using UnityEngine;

public class Punch : MonoBehaviour
{

    
    [Header("Exhaustion Settings")]
    public float maxExhaustion = 4f;
    private float currentExhaustion = 4f;
    public float feedbackerExhaustion = 2f;
    public float knuckleblasterExhaustion = 3f;
    public float exhaustionRecoveryRate = 2f; // in seconds

    [Header("Arms")]
    public string[] armList;
    private string currentArm;

    [Header("Audio Settings")]
    public AudioSource punchAudioSource;
    public AudioClip[] punchSounds;

    [Header("References")]
    public Animator animator;
    public HandScript handScript; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        currentArm = armList[0];
        Debug.Log("Feedbacker Selected");
    }

    // Update is called once per frame
    void Update()
    {

        if (currentExhaustion < maxExhaustion)
        {
            currentExhaustion += exhaustionRecoveryRate * Time.deltaTime;
            currentExhaustion = Mathf.Min(currentExhaustion, maxExhaustion);
        }


        if (Input.GetButtonDown("Punch"))
        {
            TryPunch();
        }
    }

    void TryPunch()
    {
        float requiredExhaustion = GetExhaustionForArm(currentArm);

        if (currentExhaustion - requiredExhaustion < 0)
        {
            Debug.Log("Too exhausted to punch!");
            return;
        }

        currentExhaustion -= requiredExhaustion;
        PerformPunch();
    }

    void PerformPunch(){
        PlayPunchSound();

        if (handScript != null)
        {
            handScript.PlayPunchAnimation();
        }
        Debug.Log("Punched with " + currentArm + ". Current Exhaustion: " + currentExhaustion);
    }

    float GetExhaustionForArm(string arm)
    {
        if (arm == "feedbacker")
            return feedbackerExhaustion;
        else if (arm == "knuckleblaster")
            return knuckleblasterExhaustion;
        else
            return 2f; // Default exhaustion
    }

    void PlayPunchSound()
    {
        if (punchSounds.Length > 0)
        {
            punchAudioSource.PlayOneShot(punchSounds[0]);
        }
    }
}
