using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Comment out for testing purposes
        transform.GetComponent<BoxCollider>().enabled = false;
        transform.GetComponent<MeshRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            transform.GetComponent<MeshRenderer>().enabled = true;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            transform.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}