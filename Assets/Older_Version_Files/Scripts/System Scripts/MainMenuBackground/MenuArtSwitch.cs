using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuArtSwitch : MonoBehaviour
{
    public Image default_Image;
    public Sprite[] menuArts;
    int imageCount;

    void GetRandomImageNumber()
    {
        imageCount = Random.Range(0, menuArts.Length);
        while (menuArts[imageCount] == default_Image.sprite)
        {
            imageCount = Random.Range(0, menuArts.Length);
        }
        default_Image.sprite = menuArts[imageCount];
    }
}