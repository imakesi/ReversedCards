using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public GameObject CardPrefab;
    public GameObject PrefabParent;

    public Player Player1;
    public Player Player2;

    public CardData CardObj;

    private SpriteRenderer LastCardRenderer;

    public struct CardStruct {
        public CardStruct(int suit, int number) {}
    }

    private List<int[]> DeckLeft = new List<int[]>(); // int[suit,number] = card data but array

    private void Start() {
        ResetDeck();

        for(int i = 0; i < 8; i++) {
            GameObject NewCard1 = MakeCard();
            Player1.AddCard(NewCard1);
            GameObject NewCard2 = MakeCard();
            Player2.AddCard(NewCard2);
        }
    }

    private void ResetDeck() {
        DeckLeft = new List<int[]>();
        for (int suit = 1; suit < 5; suit++) {
            for (int number = 1; number < 14; number++) {
                int[] NewCard = {suit, number};
                DeckLeft.Add(NewCard);
            }
        }
    }

    public GameObject MakeCard() { // runs with button
        GameObject NewCard = Instantiate(CardPrefab, Vector3.zero, Quaternion.identity);
        NewCard.transform.SetParent(PrefabParent.transform);

        // SpriteRenderer CardRenderer = NewCard.transform.GetChild(0).GetComponent<SpriteRenderer>();
        SpriteRenderer CardRenderer = NewCard.GetComponentInChildren<SpriteRenderer>();
        if (LastCardRenderer) {
            LastCardRenderer.sortingOrder = 0;
        }

        CardRenderer.sortingOrder = 1;

        if(DeckLeft.Count == 0) {
            Debug.Log("ran out of cards");
            ResetDeck();
        }

        int[] CardPair = DeckLeft[Random.Range(0, DeckLeft.Count)];
        DeckLeft.Remove(CardPair);

        CardObj.suit = CardPair[0];
        CardObj.number = CardPair[1];

        LastCardRenderer = NewCard.GetComponentInChildren<SpriteRenderer>();

        return NewCard;
    }
}