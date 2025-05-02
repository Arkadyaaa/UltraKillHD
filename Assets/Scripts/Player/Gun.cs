using UnityEngine;
using System.Collections;
using UnityEngine.VFX; 

public class Gun : MonoBehaviour
{
    [Header("Gun Settings")]
    public float fireRate = 0.2f;
    private float nextFireTime = 0f;

    [Header("Laser Settings")]
    public LineRenderer laserLine;
    public float laserDuration = 0.1f;
    public float laserStartWidth = 0.05f;

    [Header("Impact Settings")]
    public GameObject impactEffect;
    public float impactLifetime = 0.05f;

    [Header ("Particles")]
    public VisualEffect muzzleFlash;

    [Header("Audio Settings")]
    public AudioSource gunAudioSource;
    public AudioClip[] gunSounds;

    [Header("References")]
    public Transform shootPoint;
    public Camera playerCamera;
    public HandScript handScript; 

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        PlayGunSound();

        if (handScript != null)
        {
            handScript.PlayShootAnimation();
        }

        muzzleFlash.Play();
        GetComponent<MuzzleLight>().Shoot();

        RaycastHit hit;
        Vector3 rayOrigin = playerCamera.transform.position;
        Vector3 rayDirection = playerCamera.transform.forward;

        if (Physics.Raycast(rayOrigin, rayDirection, out hit))
        {
            GameObject impact = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            impact.transform.SetParent(hit.collider.transform);
            Destroy(impact, impactLifetime);

            StartCoroutine(FireLaser(hit.point));
        }
        else
        {
            StartCoroutine(FireLaser(rayOrigin + rayDirection * 50f));
        }
    }

    IEnumerator FireLaser(Vector3 hitPosition)
    {
        laserLine.SetPosition(0, shootPoint.position);
        laserLine.SetPosition(1, hitPosition);
        laserLine.enabled = true;

        float elapsedTime = 0f;
        float startWidth = laserStartWidth;
        float endWidth = 0f;

        while (elapsedTime < laserDuration)
        {
            float t = elapsedTime / laserDuration;
            float newWidth = Mathf.Lerp(startWidth, endWidth, t);
            laserLine.startWidth = newWidth;
            laserLine.endWidth = newWidth;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        laserLine.enabled = false;
    }

    void PlayGunSound()
    {
        if (gunSounds.Length > 0)
        {
            gunAudioSource.PlayOneShot(gunSounds[Random.Range(0, gunSounds.Length)]);
        }
    }
}
