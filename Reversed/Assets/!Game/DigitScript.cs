using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DigitScript : MonoBehaviour
{
    public int value = 0;

    private void Update() {
        Sprite DigitIMG = Resources.Load<Sprite>($"Sprites/CardsFont/{value.ToString()}");
        GetComponent<Image>().sprite = DigitIMG;
    }
}
