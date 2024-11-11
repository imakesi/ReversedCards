using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CardBackManager : MonoBehaviour
{
    public GameObject CardPrefab;
    public GameObject PrefabParent;

    public CardData CardObj;
    public int increment = 0;

    private void OnMouseDown() {
        increment++;

        GameObject NewCard = Instantiate(CardPrefab, Vector3.zero, Quaternion.identity);
        NewCard.transform.SetParent(PrefabParent.transform);
        
        SpriteRenderer CardRenderer = NewCard.transform.GetChild(0).GetComponent<SpriteRenderer>();

        CardRenderer.sortingOrder = increment;
        CardObj.number = Random.Range(1, 13);
        CardObj.suit = Random.Range(1, 4);
    }
}