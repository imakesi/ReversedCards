using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardIMGManager : MonoBehaviour
{
    public Sprite CardIMG;    
    public CardData CardObj;                   
    
    private void Awake() {
        CardIMG = Resources.Load<Sprite>($"CardAssets/{CardObj.number}.{CardObj.suit}");
        GetComponent<SpriteRenderer>().sprite = CardIMG;
    }
}
