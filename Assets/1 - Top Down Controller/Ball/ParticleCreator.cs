using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCreator : MonoBehaviour
{
    [SerializeField] GameObject particle;

    public void CreateParticles(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject newPart = GameObject.Instantiate(particle, transform.position, Quaternion.Euler(Vector3.zero));
        }
    }
}
