using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private Material _mat;

    // Start is called before the first frame update
    void Start()
    {
        _mat = GetComponent<Renderer>().material;
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
            StartCoroutine(Animations.FadeTo(_mat, 0f, 0.3f));
        }
    }
}
