using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ChildTarget : MonoBehaviour
{
    public static Vector3 OriginalSize;

    public static Vector3 EnlargedSize;

    private static Vector2 _screenSize;


    // Start is called before the first frame update
    void Start()
    {
        _screenSize = new Vector2(Camera.main.aspect * Camera.main.orthographicSize, Camera.main.orthographicSize);
        OriginalSize = transform.localScale;
        EnlargedSize = new Vector3(1.5f * OriginalSize.x, 1.5f * OriginalSize.y, 1.5f * OriginalSize.z);

    }

    // Update is called once per frame
    void Update()
    {
        Rotation();
        MoveUpDown();
        MoveLeftRight();
    }

    void OnCollisionEnter(Collision collision)
    {
        //transform.parent.GetComponent<Target>().CollisionDetected(this);
        if (GetComponent<BoxCollider>() != null)
        {
            GetComponent<BoxCollider>().enabled = false;
        }
        else if (GetComponent<MeshCollider>() != null)
        {
            GetComponent<MeshCollider>().enabled = false;
        }
        StartChildAnimation();
        Target.ChildTargetsHit++;
    }
    
    void StartChildAnimation()
    {
        StartCoroutine(Animations.Enlarge(transform, EnlargedSize, 0.3f));
        //StartCoroutine(Animations.Enlarge(transform, new Vector3(transform.localScale.x * 1.5f, transform.localScale.y * 1.5f, transform.localScale.z * 1.5f), 0.3f));
        StartCoroutine(Animations.FadeTo(GetComponent<Renderer>().material, 0, 0.3f));
    }

    void Rotation()
    {
        if (transform.tag == "Rotate")
        {
            transform.Rotate(0, 0, -1);
        }
    }

    void MoveUpDown()
    {
        if (transform.tag == "MoveUpDown")
        {
            float topBorder = _screenSize.y - transform.localScale.y * 2;
            float bottomBorder = -_screenSize.y + transform.localScale.y * 2;

            transform.position = new Vector3(transform.position.x, Mathf.Lerp(bottomBorder, topBorder, Mathf.PingPong(Time.time / 2f, 1)), transform.position.z);
        }
    }

    void MoveLeftRight()
    {
        if (transform.tag == "MoveLeftRight")
        {
            float leftBorder = -_screenSize.x + transform.localScale.x * 2;
            float rightBorder = _screenSize.x - transform.localScale.x * 2;

            transform.position = new Vector3(Mathf.Lerp(leftBorder, rightBorder, Mathf.PingPong(Time.time / 2f, 1)), transform.position.y, transform.position.z);
        }
    }
}