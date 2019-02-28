using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animations : MonoBehaviour
{
    // Define an enumerator to perform our fading.
    // Pass it the material to fade, the opacity to fade to (0 = transparent, 1 = opaque),
    // and the number of seconds to fade over.
    public static IEnumerator FadeTo(Material material, float targetOpacity, float duration)
    {
        // Cache the current color of the material, and its initiql opacity.
        Color color = material.color;
        float startOpacity = color.a;

        // Track how many seconds we've been fading.
        float t = 0;

        while (t < duration)
        {
            // Step the fade forward one frame.
            t += Time.deltaTime;
            // Turn the time into an interpolation factor between 0 and 1.
            float blend = Mathf.Clamp01(t / duration);

            // Blend to the corresponding opacity between start & target.
            color.a = Mathf.Lerp(startOpacity, targetOpacity, blend);

            // Apply the resulting color to the material.
            material.color = color;

            // Wait one frame, and repeat.
            yield return null;
        }
    }

    public static IEnumerator Enlarge(Transform transform, Vector3 targetScale, float duration)
    {
        Transform trans = transform;
        Vector3 startScale = transform.localScale;

        // Track how many seconds we've been fading.
        float t = 0;

        while (t < duration)
        {
            // Step the fade forward one frame.
            t += Time.deltaTime;
            // Turn the time into an interpolation factor between 0 and 1.
            float blend = Mathf.Clamp01(t / duration);

            // Blend to the corresponding opacity between start & target.
            trans.localScale = new Vector3(Mathf.Lerp(startScale.x, targetScale.x, blend),
                Mathf.Lerp(startScale.y, targetScale.y, blend), Mathf.Lerp(startScale.z, targetScale.z, blend));

            // Apply the resulting color to the material.
            transform.localScale = trans.localScale;

            // Wait one frame, and repeat.
            yield return null;
        }
    }

    public static IEnumerator WaitForLevelEnd()
    {
        yield return new WaitForSeconds(2);
    }
}