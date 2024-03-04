using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Juice : MonoBehaviour
{
    [SerializeField] float squishTimerMax;
    [SerializeField] AnimationCurve squishCurve;
    [SerializeField] AudioClip hitSFX;

    AudioSource audioSource;

    float squishTimer;
    float scaleYOriginal;
    float scaleXOriginal;

    bool resetScale;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        scaleYOriginal = transform.localScale.y;
        scaleXOriginal = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (squishTimer > 0)
        {
            float scaleY;
            float scaleX;

            scaleY = scaleYOriginal * squishCurve.Evaluate(squishTimer / squishTimerMax);
            scaleX = scaleXOriginal * squishCurve.Evaluate(squishTimer / squishTimerMax);

            transform.localScale = new Vector3(scaleX, scaleY, transform.localScale.z);
            squishTimer -= Time.deltaTime;

            resetScale = true;
        }
        else if (resetScale)
        {
            transform.localScale = new Vector3(scaleXOriginal, scaleYOriginal, transform.localScale.z);
        }
    }

    public void Squish()
    {
        if (squishTimer <= 0)
        {
            squishTimer = squishTimerMax;
        }
    }

    public void Hit()
    {
        if (audioSource != null && hitSFX != null && audioSource.enabled == true)
        {
            audioSource.PlayOneShot(hitSFX);
        }
    }
}
