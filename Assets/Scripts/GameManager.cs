using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    AudioSource audioSource;
    public List<Sprite> spriteList = new List<Sprite>();
    GameObject[] slots = new GameObject[12];
    GameObject[] slotButtons = new GameObject[12];
    int[] assignedNumber = new int[6];
    bool[] takenSprites = new bool[6];
    int[] assignedSprite = new int[12];
    public GameObject endButtons;

    public Text timerOutput;
    float gameTimer;
    int outputTime;

    public Text scoreOutput;
    int gameCount; //tarjetas encontradas
    int maxGameCount;

    int chosenSlot1 = -1;
    int chosenSlot2 = -1;

    public int flipCount;
    public bool gameOver;
    bool canClick;
    int completedSlots;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = GameObject.FindGameObjectWithTag("Slots").transform.GetChild(i).gameObject;
            slotButtons[i] = GameObject.FindGameObjectWithTag("Slot Buttons").transform.GetChild(i).gameObject;
        }

        for (int i = 0; i < 6; i++)
        {
            if (Menu.photosChanged[i])
            {
                spriteList.RemoveAt(i);
                spriteList.Insert(i, Menu.customPhotos[i]);
            }
        }
    }

    void Start()
    {
        canClick = true;
        gameCount = 0;
        maxGameCount = slots.Length/2;
        GameSetUp();

        if (Menu.difficultyMode == 0)
            gameTimer = 0;
        else if (Menu.difficultyMode == 1)
            gameTimer = 300;
        else if (Menu.difficultyMode == 2)
            gameTimer = 100;
    }

    void Update()
    {
        UpdateUI();

        if (flipCount >= 2)
        {
            if (assignedSprite[chosenSlot1] == assignedSprite[chosenSlot2])
                StartCoroutine(RemoveSlots(slots[chosenSlot1], slotButtons[chosenSlot1], slots[chosenSlot2], slotButtons[chosenSlot2]));
            else
                StartCoroutine(UnFlipSlots());

            chosenSlot1 = -1;
            chosenSlot2 = -1;
            flipCount = 0;
        }

        if (completedSlots >= spriteList.Count && !gameOver)
        {
            gameOver = true;
            endButtons.SetActive(true);
            StartCoroutine(SendScore((120 - outputTime) * 8));
        }
    }

    IEnumerator SendScore(int value)
    {
        Debug.Log(value);
        WWWForm form = new WWWForm();
        form.AddField("game", "Memotest");
        form.AddField("score", value);

        WWW www = new WWW("https://julianlerej.com/app/views/sendScore.php", form);
        yield return www;
    }

    public void GameSetUp()
    {
        HideSlots();
        int toAssign;

        for (int i = 0; i < slots.Length; i++)
        {
            do
            {
                toAssign = Random.Range(0, 6);
                slots[i].GetComponent<SpriteRenderer>().sprite = spriteList[toAssign];
                assignedSprite[i] = toAssign;
            } while (takenSprites[toAssign]);

            assignedNumber[toAssign]++;
            if (assignedNumber[toAssign] >= 2)
                takenSprites[toAssign] = true;
        }
    }

    void HideSlots()
    {
        for (int i = 0; i < slots.Length; i++)
            slots[i].transform.Rotate(Vector3.up*90f);
    }

    public void FlipSlot(int slotNumber)
    {
        if (canClick)
        {
            slots[slotNumber].transform.Rotate(Vector3.up * -90f);
            slotButtons[slotNumber].GetComponent<Slot>().flipped = true;
            slots[slotNumber].GetComponent<SpriteRenderer>().sortingOrder = 3;

            if (chosenSlot1 == -1)
                chosenSlot1 = slotNumber;
            else if (chosenSlot1 != -1 && chosenSlot2 == -1)
                chosenSlot2 = slotNumber ;

            flipCount++;
            audioSource.Play();
        }
    }

    IEnumerator UnFlipSlots()
    {
        canClick = false;
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < slots.Length; i++)
            if (slots[i].gameObject != null && slotButtons[i].gameObject != null)
            {
                if (slotButtons[i].GetComponent<Slot>().flipped)
                {
                    slots[i].transform.Rotate(Vector3.up * 90f);
                    slotButtons[i].GetComponent<Slot>().flipped = false;
                }
                slots[i].GetComponent<SpriteRenderer>().sortingOrder = 1;
            }
        canClick = true;
    }

    IEnumerator RemoveSlots(GameObject slot1, GameObject button1, GameObject slot2, GameObject button2)
    {
        yield return new WaitForSeconds(1f);
        Destroy(button1);
        Destroy(button2);
        completedSlots++;
        gameCount++;
    }

    void UpdateUI()
    {
        if (!gameOver)
        {
            if (Menu.difficultyMode == 0)
                gameTimer += Time.deltaTime;
            else if (Menu.difficultyMode == 1 || Menu.difficultyMode == 2)
                gameTimer -= Time.deltaTime;
        }
        
        if (gameTimer < 0f)
        {
            endButtons.SetActive(true);
            gameOver = true;
        }

        outputTime = (int)gameTimer;
        timerOutput.text = outputTime.ToString();
        scoreOutput.text = gameCount + " / " + maxGameCount;
    }

    public void PlaySound()
    {
        audioSource.Play();
    }
}