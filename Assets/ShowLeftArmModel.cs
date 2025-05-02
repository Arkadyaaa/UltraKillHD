using UnityEngine;

public class ShowLeftArmModel : StateMachineBehaviour
{
    public string armTag = "LeftArm"; // Tag the arm model with this in the Unity Editor

    // Called when entering the punch animation state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Find all renderers under the model tagged "LeftArm"
        GameObject arm = GameObject.FindWithTag(armTag);
        if (arm != null)
        {
            foreach (Renderer r in arm.GetComponentsInChildren<Renderer>())
            {
                r.enabled = true;
            }
        }
    }

    // Called when exiting the punch animation state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject arm = GameObject.FindWithTag(armTag);
        if (arm != null)
        {
            foreach (Renderer r in arm.GetComponentsInChildren<Renderer>())
            {
                r.enabled = false;
            }
        }
    }
}
