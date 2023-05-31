using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestParticleSystem : MonoBehaviour
{
    public ParticleSystem ps;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            ps.Play();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            ps.Stop();
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            ps.Pause();
        }
    }
}
