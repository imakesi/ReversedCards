using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigitScript : MonoBehaviour
{
    public int value = 0;

    private void Start() {
        var DigitIMG = Resources.Load<Sprite>($"Sprites/CardsFont/{value}");
        GetComponent<SpriteRenderer>().sprite = DigitIMG;
    }
}
