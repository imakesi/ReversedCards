using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigitScript : MonoBehaviour
{
    public int value = 0;

    private void Update() {
        Sprite DigitIMG = Resources.Load<Sprite>($"Sprites/CardsFont/{value}");
        GetComponent<SpriteRenderer>().sprite = DigitIMG;
    }
}
