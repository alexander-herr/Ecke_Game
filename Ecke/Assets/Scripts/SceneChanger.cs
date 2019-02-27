using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void GotoNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex < SceneManager.sceneCount - 1)
        {
            StartCoroutine(WaitEnumerator());
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
    }

    IEnumerator WaitEnumerator()
    {
        yield return new WaitForSeconds(2);
    }
}
