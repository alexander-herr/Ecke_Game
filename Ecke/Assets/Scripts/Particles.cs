using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particles : MonoBehaviour
{
    public ParticleSystem PS;

    // Start is called before the first frame update
    void Start()
    {
        PS.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        PS.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            PS.Play();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            PS.Stop();
        }
    }
}
