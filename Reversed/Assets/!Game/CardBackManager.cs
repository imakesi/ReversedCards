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

    //public struct Card() {
        //Card(int suit, int number);
    //}

    private List<GameObject> DeckLeft = new List<GameObject>();

    private void Start() {
        
    }

    private void ResetDeck() {
       
    }

    public void MakeCard() { // runs with button
        increment++;

        GameObject NewCard = Instantiate(CardPrefab, Vector3.zero, Quaternion.identity);
        NewCard.transform.SetParent(PrefabParent.transform);

        // SpriteRenderer CardRenderer = NewCard.transform.GetChild(0).GetComponent<SpriteRenderer>();
        SpriteRenderer CardRenderer = NewCard.GetComponentInChildren<SpriteRenderer>();

        CardRenderer.sortingOrder = increment;
        CardObj.number = Random.Range(1, 13);
        CardObj.suit = Random.Range(1, 4);
    }
}