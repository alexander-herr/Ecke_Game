using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private Material _mat;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        LevelDone();
    }

    void LevelDone()
    {
        if (Target.LevelDone)
        {
            foreach (Transform child in transform)
            {
                StartCoroutine(Animations.FadeTo(child.GetComponent<Renderer>().material, 0f, 0.3f));
            }
        }
    }
}