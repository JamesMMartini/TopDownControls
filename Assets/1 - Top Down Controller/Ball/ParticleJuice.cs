using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleJuice : MonoBehaviour
{
    float timerMax;
    float timer;
    Vector3 velocity;
    Material mat;

    // Start is called before the first frame update
    void Start()
    {
        timerMax = Random.Range(0.5f, 1.5f);
        timer = timerMax;
        velocity = new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), 0f);

        mat = GetComponent<SpriteRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= 0)
            Destroy(gameObject);

        transform.position = transform.position + velocity * Time.deltaTime;
        mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, timer / timerMax);

        timer -= Time.deltaTime;
    }
}
