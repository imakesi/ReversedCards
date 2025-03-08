using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    private List<GameObject> Hand = new List<GameObject>();
    private int CurrentPage = 1;

    [SerializeField] private Transform[] CardPositions;

    [SerializeField] private KeyCode PageNext, PageLast;

    public DigitScript Digit1P1;
    public DigitScript Digit2P1;
    public DigitScript Digit1P2;
    public DigitScript Digit2P2;

    private void Start() {
        Digit1P1 = GameObject.Find("Digit 1 P1").GetComponent<DigitScript>();
        Digit2P1 = GameObject.Find("Digit 2 P1").GetComponent<DigitScript>();
        Digit1P2 = GameObject.Find("Digit 1 P2").GetComponent<DigitScript>();
        Digit2P2 = GameObject.Find("Digit 2 P2").GetComponent<DigitScript>();
    }

    public void AddCard(GameObject card) {
        Hand.Add(card);
    }

    public void IncPage() {
        if (Hand.Count % 5 == 0) {
            if (CurrentPage < Hand.Count / 5) { CurrentPage++; }
        } else {
            if (CurrentPage < Hand.Count / 5+1) { CurrentPage++; }
        }
    }
    public void DecPage() { if(CurrentPage > 1) { CurrentPage--; } }

    private void DisplayHand() {
        foreach(GameObject card in Hand) {
            card.SetActive(false);
        }

        int firstCardIndex = (CurrentPage - 1) * 5;
        int lastCardIndex = firstCardIndex + 5;
        int x = 0;
        for (int i = firstCardIndex; i < lastCardIndex; i++) {
            if (i >= Hand.Count) { break; }
            Hand[i].SetActive(true);
            Hand[i].transform.position = CardPositions[x++].transform.position;
        }
    }

    private void Update() {
        DisplayHand();

        if(Input.GetKeyDown(PageLast)) {
            DecPage();
        } else if(Input.GetKeyDown(PageNext)) {
            IncPage();
        }

        int count = Hand.Count;
        int digdig2 = count % 10;
        int digdig1 = (count / 10) % 10;

        if (CompareTag("Player1")) {
            Digit1P1.value = digdig1;
            Digit2P1.value = digdig2;
        } else if (CompareTag("Player2")) {
            Digit1P2.value = digdig1;
            Digit2P2.value = digdig2;
        }
    }
}
