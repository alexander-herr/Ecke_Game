using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Target : MonoBehaviour
{
    public static bool LevelDone;
    public static int ChildTargetsHit;
    public static bool NeverDone = true;

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
            if (NeverDone)
            {
                StartCoroutine(Animations.WaitForLevelEnd());
                //SceneChanger.GotoNextLevel();
                NeverDone = false;
            }
        }
    }

    void ReloadChild()
    {
        if (Sphere.CheckOffScreen() && !LevelDone || DrawLine.RestartNewLine)
        {
            ChildTargetsHit = 0;
            foreach (Transform child in transform)
            {
                if (!child.GetComponent<BoxCollider>().enabled)
                {
                    child.GetComponent<BoxCollider>().enabled = true;
                    StartCoroutine(Animations.FadeTo(child.GetComponent<Renderer>().material, 1, 0.3f));
                    StartCoroutine(Animations.Enlarge(child.transform,
                        new Vector3(ChildTarget.OriginalSize.x, ChildTarget.OriginalSize.y, ChildTarget.OriginalSize.z), 0.3f));
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

    void TestScale(Vector3 targetScale, float speed)
    {
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, speed * Time.deltaTime);
    }
}