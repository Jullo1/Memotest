using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Customize : MonoBehaviour
{
    AudioSource audioS;

    public GameObject secretCanvas;
    public GameObject defaultCanvas;
    public InputField input;
    string password = "147admin";

    void Awake()
    {
        audioS = GetComponent<AudioSource>();
    }

    public void ShowCanvas()
    {
        if (input.text == password)
        {
            secretCanvas.SetActive(true);
            defaultCanvas.SetActive(false);
        }
        audioS.Play();
    }
}