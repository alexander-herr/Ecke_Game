using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public static bool LevelDone;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter()
    {
        LevelDone = true;
        //transform.localScale += new Vector3(1, 1, 1);
        StartCoroutine(Animations.Enlarge(transform,
            new Vector3(transform.localScale.x + 1, transform.localScale.y + 1, transform.localScale.z + 1), 0.3f));
        StartCoroutine(Animations.FadeTo(GetComponent<Renderer>().material, 0, 0.3f));
    }
}
