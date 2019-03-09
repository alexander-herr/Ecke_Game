using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Target : MonoBehaviour
{
    public static bool LevelDone;
    public static int ChildTargetsHit;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        LevelDoneFunc();
        ReloadChild();
        DestroyAllChilds();
    }

    void OnCollisionEnter()
    {
    }

    public void CollisionDetected(ChildTarget childTarget)
    {
        //Debug.Log("child collided");
    }

    void LevelDoneFunc()
    {
        if (ChildTargetsHit == transform.childCount)
        {
            LevelDone = true;
            StartCoroutine(Animations.ChangeScene());
        }
    }

    void ReloadChild()
    {
        if (FindObjectOfType<Sphere>().CheckOffScreen() && !LevelDone || DrawLine.RestartNewLine)
        {
            ChildTargetsHit = 0;
            foreach (Transform child in transform)
            {
                if (!child.GetComponent<BoxCollider>().enabled)
                {
                    child.GetComponent<BoxCollider>().enabled = true;
                    StartCoroutine(Animations.FadeTo(child.GetComponent<Renderer>().material, 1, 0.3f));
                    StartCoroutine(Animations.Enlarge(child.transform, ChildTarget.OriginalSize, 0.3f));
                    //StartCoroutine(Animations.Enlarge(child.transform, new Vector3(child.transform.localScale.x / 1.5f, child.transform.localScale.y / 1.5f, child.transform.localScale.z / 1.5f), 0.3f));
                }
            }
        }
    }

    void DestroyAllChilds()
    {
        if (LevelDone)
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }
    }

    public static void ReloadLevel()
    {
        ChildTargetsHit = 0;
    }
}