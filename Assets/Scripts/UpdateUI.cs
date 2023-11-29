using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UpdateUI : MonoBehaviour
{
    GameManager game;
    Image backgroundImage;
    GameObject[] logoImages = new GameObject[12];
    //Image topLogo;
    public Image topBar;
    //public Image gameOverBackground;

    void Awake()
    {
        if (SceneManager.GetActiveScene().name == "Main")
        {
            game = FindObjectOfType<GameManager>().GetComponent<GameManager>();

            for (int i = 0; i < 12;  i++)
            {
                logoImages[i] = GameObject.FindGameObjectWithTag("Slot Buttons").transform.GetChild(i).gameObject;
            }
            //topLogo = GameObject.FindGameObjectWithTag("Logo").GetComponent<Image>();
        }
    }

    void Start()
    {
        if (Menu.backgroundChanged)
        {
            backgroundImage = GameObject.FindGameObjectWithTag("Background").GetComponent<Image>();
            backgroundImage.sprite = Menu.customBackground;
        }

        if (SceneManager.GetActiveScene().name == "Main")
        {
            if (Menu.logoChanged)
            {
                for (int i = 0; i < 12; i++)
                    logoImages[i].GetComponent<SpriteRenderer>().sprite = Menu.customLogo;

                //topLogo.sprite = Menu.customLogo;
            }

            byte r = (byte) Menu.colorCode.r;
            byte g = (byte)Menu.colorCode.g;
            byte b = (byte)Menu.colorCode.b;

            //topBar.color = new Color32(255, 255, 255, 255);
            //gameOverBackground.color = new Color32(255, 255, 255, 255);
        }
    }
}
