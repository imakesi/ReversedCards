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

    public List<GameObject> SelectedCards = new List<GameObject>();
    public int SelectedLimit = 5;

    [SerializeField] private Transform[] CardPositions;

    [SerializeField] private KeyCode PageNext, PageLast;

    public DigitScript Digit1P1;
    public DigitScript Digit2P1;
    public DigitScript Digit1P2;
    public DigitScript Digit2P2;

    private GameObject TurnIndi1;
    private GameObject TurnIndi2;

    public ThirdPartyPlayer PlayerParty3;

    private void Start() {
        Digit1P1 = GameObject.Find("Digit 1 P1").GetComponent<DigitScript>();
        Digit2P1 = GameObject.Find("Digit 2 P1").GetComponent<DigitScript>();
        Digit1P2 = GameObject.Find("Digit 1 P2").GetComponent<DigitScript>();
        Digit2P2 = GameObject.Find("Digit 2 P2").GetComponent<DigitScript>();

        TurnIndi1 = GameObject.Find("TurnIndi1");
        TurnIndi2 = GameObject.Find("TurnIndi2");

        PlayerParty3 = GameObject.Find("Manager").GetComponent<ThirdPartyPlayer>();
    }

    public void AddCard(GameObject card) {
        if (CompareTag("Player1")) {
            card.gameObject.tag = "Player1";
        }
        else if (CompareTag("Player2")) {
            card.gameObject.tag = "Player2";
        }
        
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

    public void PlayHand() {
        if (PlayerParty3.CurrentTurn == false && CompareTag("Player1") ||
            PlayerParty3.CurrentTurn == true && CompareTag("Player2")) {
            // if our turn, then use our selectedcards
            bool compatible = true;
            GameObject currentcard;
            int lastnumber = -1;
            for(int i = 0; i < SelectedCards.Count; i++) {
                currentcard = SelectedCards[i];
                CardIMGManager curmanage = currentcard.GetComponentInChildren<CardIMGManager>();
                CardData curdata = curmanage.CardObj;

                if (lastnumber != -1) {
                    if (lastnumber != curdata.number && curdata.number-1 != lastnumber) {
                        compatible = false;
                        print("incompatible");
                        break;
                    }
                }
                lastnumber = curdata.number;
            }

            if(compatible) {
                // score the played hand

                float score = 0;
                for(int i = 0; i < SelectedCards.Count; i++) {
                    currentcard = SelectedCards[i];
                    CardIMGManager curmanage = currentcard.GetComponentInChildren<CardIMGManager>();
                    CardData curdata = curmanage.CardObj;

                    // ace -> 13?
                    // 4 -> 5?

                    print($"i: {i}, curdata number: {curdata.number}");

                    if(curdata.number < 11 && curdata.number > 1) {
                        // 2-10
                        score += 0.5f;
                    } else if(curdata.number > 10) {
                        // face card
                        score += 1f;
                    } else if(curdata.number == 1) {
                        // ace
                        score += 2f;
                    }
                }

                score = MathF.Round(score);
                print($"scored hand: {score}");
            }
        }

        PlayerParty3.SwitchTurn();
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

        if(PlayerParty3.CurrentTurn == false)
        {
            TurnIndi1.SetActive(true);
            TurnIndi2.SetActive(false);
        } else
        {
            TurnIndi1.SetActive(false);
            TurnIndi2.SetActive(true);
        }
    }
}












// DOUG DOT PNG