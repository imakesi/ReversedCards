using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardIMGManager : MonoBehaviour
{
    private SpriteRenderer CardRenderer;

    public Sprite CardIMG;
    public SpawnManagerScriptableObject CardData;                   
    
    private void Awake() {
        CardIMG = Resources.Load<Sprite>("CardAssets/1.1");
        CardRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start() {
        CardRenderer.sprite = CardIMG;
    }
}
