using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class Menu : MonoBehaviour
{
    AudioSource audioS;
    public GameObject[] difficultyButtons = new GameObject[3];

    public static Sprite customBackground;
    public static bool backgroundChanged;

    public static Sprite customLogo;
    public static bool logoChanged;

    public static Sprite[] customPhotos = new Sprite[6];
    public static bool[] photosChanged = new bool[6];

    public Text colorFieldR;
    public Text colorFieldG;
    public Text colorFieldB;
    public static Color colorCode;

    public static int difficultyMode;

    public void GameStart()
    {
        SceneManager.LoadScene("Main");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void Restart()
    {
        SceneManager.LoadScene("Menu");
    }

    public void LoadBackground()
    {
        if (File.Exists(Application.dataPath + "/fondo.jpg"))
        {
            Texture2D tempTexture = new Texture2D(10, 10);
            tempTexture.LoadImage(File.ReadAllBytes(Application.dataPath + "/fondo.jpg"));
            customBackground = Sprite.Create(tempTexture, new Rect(0, 0, tempTexture.width, tempTexture.height), Vector2.zero);
            backgroundChanged = true;
            SetUpLoad(0);
        }
    }

    public void LoadPhotos()
    {
        Texture2D tempTexture;

        for (int i = 1; i <= 6; i++)
        {
            if (File.Exists(Application.dataPath + "/" + i + ".png"))
            {
                tempTexture = new Texture2D(260, 260);
                tempTexture.LoadImage(File.ReadAllBytes(Application.dataPath + "/" + i + ".png"));
                tempTexture = ScaleTexture(tempTexture, 260, 260);
                customPhotos[i - 1] = Sprite.Create(tempTexture, new Rect(0, 0, tempTexture.width, tempTexture.height), Vector2.one * 0.5f);
                photosChanged[i - 1] = true;
            }
            else if (File.Exists(Application.dataPath + "/" + i + ".jpg"))
            {
                tempTexture = new Texture2D(260, 260);
                tempTexture.LoadImage(File.ReadAllBytes(Application.dataPath + "/" + i + ".jpg"));
                tempTexture = ScaleTexture(tempTexture, 260, 260);
                customPhotos[i - 1] = Sprite.Create(tempTexture, new Rect(0, 0, tempTexture.width, tempTexture.height), Vector2.one * 0.5f);
                photosChanged[i - 1] = true;
            }
        }
        SetUpLoad(1);
    }

    public void LoadLogos()
    {
        if (File.Exists(Application.dataPath + "/logo.jpg"))
        {
            Texture2D tempTexture = new Texture2D(200, 200);
            tempTexture.LoadImage(File.ReadAllBytes(Application.dataPath + "/logo.jpg"));
            tempTexture = ScaleTexture(tempTexture, 200, 200);
            customLogo = Sprite.Create(tempTexture, new Rect(0, 0, tempTexture.width, tempTexture.height), Vector2.one * 0.5f);
            logoChanged = true;
            SetUpLoad(2);
        }
        else if (File.Exists(Application.dataPath + "/logo.png"))
        {
            Texture2D tempTexture = new Texture2D(200, 200);
            tempTexture.LoadImage(File.ReadAllBytes(Application.dataPath + "/logo.png"));
            tempTexture = ScaleTexture(tempTexture, 200, 200);
            customLogo = Sprite.Create(tempTexture, new Rect(0, 0, tempTexture.width, tempTexture.height), Vector2.one * 0.5f);
            logoChanged = true;
            SetUpLoad(2);
        }
    }

    public void ApplyColor()
    {
        File.WriteAllText(Application.dataPath + "/1", colorFieldR.text);
        File.AppendAllText(Application.dataPath + "/1", "\n" + colorFieldG.text);
        File.AppendAllText(Application.dataPath + "/1", "\n" + colorFieldB.text);
        SetUpLoad(3);
    }

    void LoadColor()
    {
        string[] lines = File.ReadAllLines(Application.dataPath + "/1");
        float[] colors = new float[3];
        int count = 0;
        foreach(string line in lines)
        {
            colors[count] = float.Parse(line);
            count++;
        }
        colorCode = new Color(colors[0], colors[1], colors[2]);
    }

    public void DeleteConfig()
    {
        File.WriteAllText(Application.dataPath + "/0", "0\n0\n0\n0");
        audioS.Play();
    }

    void SetUpLoad(int line) // 0 = background; 1 = photos; 2 = logo; 3 = colors
    {
        string[] previousText = File.ReadAllLines(Application.dataPath + "/0");

        switch (line)
        {
            case 0: //change first line
                File.WriteAllText(Application.dataPath + "/0", "1");
                File.AppendAllText(Application.dataPath + "/0", "\n" + previousText[1]);
                File.AppendAllText(Application.dataPath + "/0", "\n" + previousText[2]);
                File.AppendAllText(Application.dataPath + "/0", "\n" + previousText[3]);
                break;
            case 1: //change second line
                File.WriteAllText(Application.dataPath + "/0", previousText[0]);
                File.AppendAllText(Application.dataPath + "/0", "\n" + "1");
                File.AppendAllText(Application.dataPath + "/0", "\n" + previousText[2]);
                File.AppendAllText(Application.dataPath + "/0", "\n" + previousText[3]);
                break;
            case 2: //and so on..
                File.WriteAllText(Application.dataPath + "/0", previousText[0]);
                File.AppendAllText(Application.dataPath + "/0", "\n" + previousText[1]);
                File.AppendAllText(Application.dataPath + "/0", "\n" + "1");
                File.AppendAllText(Application.dataPath + "/0", "\n" + previousText[3]);
                break;
            case 3:
                File.WriteAllText(Application.dataPath + "/0", previousText[0]);
                File.AppendAllText(Application.dataPath + "/0", "\n" + previousText[1]);
                File.AppendAllText(Application.dataPath + "/0", "\n" + previousText[2]);
                File.AppendAllText(Application.dataPath + "/0", "\n" + "1");
                break;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F7))
            SceneManager.LoadScene("Customize");
    }

    void Awake()
    {
        audioS = GetComponent<AudioSource>();
        InitializeConfigFiles();

        string[] configLines = File.ReadAllLines(Application.dataPath + "/0"); //to read config
        for (int i = 0; i < configLines.Length; i++)
        {

            if (configLines[i] == "1")
            {
                switch (i)
                {
                    case 0:
                        LoadBackground();
                        break;
                    case 1:
                        LoadPhotos();
                        break;
                    case 2:
                        LoadLogos();
                        break;
                    case 3:
                        LoadColor();
                        break;
                }
            }
        }
    }

    public void DifficultyNoLimit()
    {
        ClearButtonColors();
        difficultyMode = 0;
        difficultyButtons[0].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        audioS.Play();
    }

    public void DifficultyEasy()
    {
        ClearButtonColors();
        difficultyMode = 1;
        difficultyButtons[1].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        audioS.Play();
    }

    public void DifficultyNormal()
    {
        ClearButtonColors();
        difficultyMode = 2;
        difficultyButtons[2].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        audioS.Play();
    }

    void ClearButtonColors()
    {
        for (int i = 0; i < 3; i++)
            difficultyButtons[i].GetComponent<Image>().color = new Color32(5, 97, 170, 255);
    }

    void InitializeConfigFiles()
    {
        if (!File.Exists(Application.dataPath + "/0"))
            File.WriteAllText(Application.dataPath + "/0", "0\n0\n0\n0");
        if (!File.Exists(Application.dataPath + "/1"))
            File.WriteAllText(Application.dataPath + "/1", "50\n50\n50");
    }

    Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
    {
        Texture2D result = new Texture2D(targetWidth, targetHeight, source.format, true);
        Color[] rpixels = result.GetPixels(0);
        float incX = (1.0f / (float)targetWidth);
        float incY = (1.0f / (float)targetHeight);
        for (int px = 0; px < rpixels.Length; px++)
        {
            rpixels[px] = source.GetPixelBilinear(incX * ((float)px % targetWidth), incY * ((float)Mathf.Floor(px / targetWidth)));
        }
        result.SetPixels(rpixels, 0);
        result.Apply();
        return result;
    }
}