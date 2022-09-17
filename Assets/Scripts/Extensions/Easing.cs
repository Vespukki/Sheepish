using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Easing
{
    public static float SmoothStart2(float input)
    {
        return input * input;
    }
    public static float SmoothStart3(float input)
    {
        return input * input * input;
    }
    public static float SmoothStart4(float input)
    {
        return input * input * input * input;
    }


    public static float SmoothStop2(float input)
    {
        return 1 - ((1 - input) * (1 - input));
    }
    public static float SmoothStop3(float input)
    {
        return 1 - ((1 - input) * (1 - input) * (1 - input));
    }
    public static float SmoothStop4(float input)
    {
        return 1 - ((1 - input) * (1 - input) * (1 - input) * (1 - input));
    }

    public static float Mix(float a, float b, float ratio)
    {
        return (a * ratio) + b * (1 - ratio);
    }

    static float fadeRate = .0075f;

    /// <summary>
    /// only works if camera has image as a child
    /// </summary>
    /// <returns></returns>
    public static IEnumerator ScreenFadeOut()
    {
        float alpha = 0;
        Image image = Camera.main.GetComponentInChildren<Image>();
        while (alpha < 1)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, SmoothStop3(alpha));
            alpha += fadeRate;
            yield return null;
        }
    }

    /// <summary>
    /// only works if camera has image as a child
    /// </summary>
    /// <returns></returns>
    public static IEnumerator ScreenFadeIn()
    {
        float alpha = 1;
        Image image = Camera.main.GetComponentInChildren<Image>();
        while (alpha > 0)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, SmoothStart3(alpha));
            alpha -= fadeRate;
            yield return null;
        }
    }

    public static void SetBlackScreen()
    {
        Image image = Camera.main.GetComponentInChildren<Image>();
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
    }
}