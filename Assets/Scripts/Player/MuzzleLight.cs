using UnityEngine;
using System.Collections;


public class MuzzleLight : MonoBehaviour
{
    public Light muzzleFlash;  // Assign in Inspector
    public float flashDuration = 0.05f;  // Duration of light effect

    private void Start()
    {
        muzzleFlash.enabled = false; // Ensure light is off at start
    }

    public void Shoot()
    {
        StartCoroutine(FlashLight());
    }

    private IEnumerator FlashLight()
    {
        muzzleFlash.enabled = true;
        yield return new WaitForSeconds(flashDuration);
        muzzleFlash.enabled = false;
    }
}
