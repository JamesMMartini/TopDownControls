using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraJuice : MonoBehaviour
{
    [SerializeField] float offsetXMax;
    [SerializeField] float offsetYMax;
    [SerializeField] float rotationMax;
    [SerializeField] float shakeTimerMax;
    
    bool traumaUsed;
    float shakeTimer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (shakeTimer > 0)
        {
            float trauma = 1f;
            //if (traumaUsed)
            //{
            //    trauma = (1 + shakeTimer) * (1 + shakeTimer);
            //}

            //float offsetX = trauma * Random.Range(-offsetXMax, offsetXMax);
            //float offsetY = trauma * Random.Range(-offsetYMax, offsetYMax);

            float offsetX = trauma * (Mathf.PerlinNoise1D(Time.time) * offsetXMax);
            float offsetY = trauma * (Mathf.PerlinNoise1D(Time.time) * offsetYMax);

            float angle = trauma * (Mathf.PerlinNoise1D(Time.time) * rotationMax);

            transform.localPosition = new Vector3(offsetX, offsetY, 0f);
            transform.localRotation = Quaternion.Euler(new Vector3(0, 0, angle));

            shakeTimer -= Time.deltaTime;

            if (shakeTimer < 0)
            {
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.Euler(Vector3.zero);
            }
        }
    }

    public void Shake()
    {
        if (shakeTimer <= 0)
        {
            shakeTimer = shakeTimerMax;
        }
        else
        {
            traumaUsed = true;
        }
    }
}
