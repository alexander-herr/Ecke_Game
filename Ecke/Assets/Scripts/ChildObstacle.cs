using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildObstacle : MonoBehaviour
{
    private Vector3 originalSize;
    private Vector3 enlargedSize;

    // Start is called before the first frame update
    void Start()
    {
        originalSize = transform.localScale;
        enlargedSize = transform.localScale * 1.1f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        StartCoroutine(Animations.Enlarge(transform,
            new Vector3(enlargedSize.x, enlargedSize.y, enlargedSize.z), 0.1f));
        StartCoroutine(Animations.Enlarge(transform,
            new Vector3(originalSize.x, originalSize.y, originalSize.z), 0.1f));
    }
}
