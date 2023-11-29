using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    BoxCollider2D col;
    GameManager game;

    public bool flipped;

    void Awake()
    {
        col = GetComponent<BoxCollider2D>();
        game = FindObjectOfType<GameManager>();
    }

    void OnMouseDown()
    {
        if(!flipped)
            game.FlipSlot(int.Parse(gameObject.name) - 1);
    }
}
