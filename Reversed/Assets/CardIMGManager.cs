using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardIMGManager : MonoBehaviour
{
    private SpriteRenderer CardRenderer;

    public Sprite CardIMG;
    
    private void Awake() {
        CardRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start() {
        CardRenderer.sprite = CardIMG;
    }
}
