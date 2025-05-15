using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.SceneManagement;
using System.Net.Mail;
using Unity.VisualScripting;

public class Player : MonoBehaviour
{
    private List<GameObject> Hand = new List<GameObject>();
    private int CurrentPage = 1;

    public List<GameObject> SelectedCards = new List<GameObject>();
    public int SelectedLimit = 5;

    [SerializeField] private Transform[] CardPositions;

    [SerializeField] private KeyCode PageNext, PageLast;

    public int maxscore = 8; // the maximum score you can get

    public float RegularCard = 0.5f;
    public float FaceCard = 1f;
    public float AceCard = 2f;

    public bool CanJQK = true;
    public float JQKPair = 6f;

    private bool straightboost = false;

    public float cardsDue = 0;

    public DigitScript Digit1P1;
    public DigitScript Digit2P1;
    public DigitScript Digit1P2;
    public DigitScript Digit2P2;

    private GameObject TurnIndi1;
    private GameObject TurnIndi2;
    private GameObject PlayHand1;
    private GameObject PlayHand2;

    public ThirdPartyPlayer PlayerParty3;
    public DeckManager ManagerOfDeck;
    public Player player1;
    public Player player2;

    private GameObject Title1;
    private GameObject Title2;
    public bool PublicCanReverse = false;
    public List<int> PublicReversableNumbers = new List<int>();

    private void Awake() {
        Digit1P1 = GameObject.Find("Digit 1 P1").GetComponent<DigitScript>();
        Digit2P1 = GameObject.Find("Digit 2 P1").GetComponent<DigitScript>();
        Digit1P2 = GameObject.Find("Digit 1 P2").GetComponent<DigitScript>();
        Digit2P2 = GameObject.Find("Digit 2 P2").GetComponent<DigitScript>();

        TurnIndi1 = GameObject.Find("TurnIndi1");
        TurnIndi2 = GameObject.Find("TurnIndi2");
        PlayHand1 = GameObject.Find("PlayHand1");
        PlayHand2 = GameObject.Find("PlayHand2");

        Title1 = GameObject.Find("Title1");
        Title2 = GameObject.Find("Title2");

        PlayerParty3 = GameObject.Find("Manager").GetComponent<ThirdPartyPlayer>();
    }

    private void Start() {
        Title1.SetActive(false);
        Title2.SetActive(false);
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

    private float ScoreHand(List<GameObject> cards) {
        string checkJQK = "";
        float score = 0;
        for (int i = 0; i < cards.Count; i++) {
            GameObject currentcard = cards[i];
            SpriteRenderer currenderer = currentcard.GetComponentInChildren<SpriteRenderer>();
            int dataNumber = Int32.Parse(currenderer.sprite.name.Split(".")[0]);

            if(dataNumber == 11) { checkJQK += "J"; }
            if(dataNumber == 12) { checkJQK += "Q"; }
            if(dataNumber == 13) { checkJQK += "K"; }

            if (dataNumber < 11 && dataNumber > 1)
            {
                // 2-10
                score += RegularCard;
            }
            else if (dataNumber > 10)
            {
                // face card
                score += FaceCard;
            }
            else if (dataNumber == 1)
            {
                // ace
                score += AceCard;
            }
        }

        if(straightboost) {
            score *= 1.5f;
        }

        if(checkJQK == "JQK" && CanJQK) { score = JQKPair; CanJQK = false; }
        if(score > maxscore) { score = maxscore; }

        score = MathF.Ceiling(score);

        return score;
    }

    public (bool, List<int>) CanReverse(List<GameObject> cards, List<GameObject> cards2, List<int> datanum) {
        List<int> reversableNums = new List<int>();
        PublicReversableNumbers.Clear();

        bool canReverse = false;
        string mode = "";

        // logic for straights
        bool straightcompatible = true;
        if(datanum.Count < 3 || datanum.Count > 5) {
            straightcompatible = false;
        }
        datanum.Sort();
        for (int i = 1; i < datanum.Count; i++) {
            if (datanum[i] != datanum[i - 1] + 1) {
                straightcompatible = false;
                break;
            }
        }

        if (datanum.Distinct().Count() == 1) {
            mode = "match";
        } if(straightcompatible) {
            mode = "straight";
        }

        GameObject currentcard;
        for (int i = 0; i < cards.Count; i++) {
            currentcard = cards[i];
            SpriteRenderer currenderer = currentcard.GetComponentInChildren<SpriteRenderer>();
            int dataNumber = Int32.Parse(currenderer.sprite.name.Split(".")[0]);

            if(mode == "match") {
                // check compatibility
                if(datanum.Distinct().First() == dataNumber) {
                    canReverse = true;
                    reversableNums.Add(datanum.Distinct().First());
                    break;
                }
            } else if(mode == "straight") {
                // check if you can add on without breaking the straight
                GameObject first = cards2.First();
                GameObject last = cards2.Last();
                SpriteRenderer firstrenderer = first.GetComponentInChildren<SpriteRenderer>();
                SpriteRenderer lastrenderer = last.GetComponentInChildren<SpriteRenderer>();
                int firstNumber = Int32.Parse(firstrenderer.sprite.name.Split(".")[0]);
                int lastNumber = Int32.Parse(lastrenderer.sprite.name.Split(".")[0]);

                int debug1 = firstNumber-1;
                int debug2 = lastNumber+1;
                print("DEBUG straight" + debug1 + " AND " + debug2);

                if(dataNumber == firstNumber-1) {
                    canReverse = true;
                    reversableNums.Add(firstNumber-1);
                    break;
                } else if(dataNumber == lastNumber+1) {
                    canReverse = true;
                    reversableNums.Add(lastNumber+1);
                    break;
                }
            }
        }

        if(cards2.Count >= 5) {
            canReverse = false;
        }

        if(canReverse) {
            float score = ScoreHand(cards2);
            cardsDue += score;
        }

        return (canReverse, reversableNums);
    }

    public void PlayHand() {

        bool reversecompatible = false;
        if (PlayerParty3.CurrentTurn == false && CompareTag("Player1") ||
            PlayerParty3.CurrentTurn == true && CompareTag("Player2")) {
            
            if(PublicCanReverse) {
                reversecompatible = true;
                for(int i = 0; i < SelectedCards.Count; i++) {
                    SpriteRenderer currenderer = SelectedCards[i].GetComponentInChildren<SpriteRenderer>();
                    int datanum = Int32.Parse(currenderer.sprite.name.Split(".")[0]);
                    if(!PublicReversableNumbers.Contains(datanum)) {
                        reversecompatible = false;
                    }
                }
            }

            bool compatible = true;
            bool straightcompatible = true;
            GameObject currentcard;

            List<int> dataNumbers = new List<int>();
            List<int> dataSuits = new List<int>();
            for (int i = 0; i < SelectedCards.Count; i++) {
                // get texture and information
                currentcard = SelectedCards[i];
                SpriteRenderer currenderer = currentcard.GetComponentInChildren<SpriteRenderer>();
                int dataNumber = Int32.Parse(currenderer.sprite.name.Split(".")[0]);
                int dataSuit = Int32.Parse(currenderer.sprite.name.Split(".")[1]);
                dataNumbers.Add(dataNumber);
                dataSuits.Add(dataSuit);
            }

            bool flushcompatible = false;
            if(dataSuits.Distinct().Count() == 1 &&
            SelectedCards.Count == 5) {
                flushcompatible = true;
            }

            dataNumbers = dataNumbers.OrderBy(num => num).ToList();

            // logic for straights
            int lastint = -1;
            for (int i = 0; i < dataNumbers.Count; i++) {
                if (lastint != -1) {
                    if (!(dataNumbers[i] == lastint - 1 ||
                        dataNumbers[i] == lastint + 1)) {
                        straightcompatible = false;
                    }
                }
                lastint = dataNumbers[i];
            }

            if(SelectedCards.Count < 3) {
                straightcompatible = false;
            }

            if(straightcompatible && dataSuits.Distinct().Count() == 1) {
                straightboost = true;
            }

            // logic for pairs 1-4
            if (!(dataNumbers.Distinct().Count() == 1)) {
                compatible = false;
            }

            if (compatible || straightcompatible || flushcompatible || reversecompatible) {
                // score the played hand

                Title1.SetActive(false);
                Title2.SetActive(false);

                float score = ScoreHand(SelectedCards);

                string target = null;

                // add cards to the other side
                if (CompareTag("Player1"))
                {
                    target = "player2";
                }
                else if (CompareTag("Player2"))
                {
                    target = "player1";
                }

                (bool, List<int>) functionResult;
                bool canreverse = false;
                if (target == "player1") {
                    functionResult = player1.CanReverse(player1.Hand, SelectedCards, dataNumbers);
                    canreverse = functionResult.Item1;
                    if(canreverse) {
                        Title1.SetActive(true);
                        player1.PublicCanReverse = true;
                        player1.PublicReversableNumbers = functionResult.Item2;
                    } else {
                        player1.PublicCanReverse = false;
                    }
                }
                else if (target == "player2") {
                    functionResult = player2.CanReverse(player2.Hand, SelectedCards, dataNumbers);
                    canreverse = functionResult.Item1;
                    if(canreverse) {
                        Title2.SetActive(true); 
                        player2.PublicCanReverse = true;
                        player2.PublicReversableNumbers = functionResult.Item2;
                    } else {
                        player2.PublicCanReverse = false;
                    }
                } else {
                    print("no target");
                }

                if(reversecompatible) {
                    // we ARE reversing
                    score += cardsDue;
                    cardsDue = 0;
                } else if(PublicCanReverse) {
                    // we CAN reverse and AREN'T
                    // note that canreverse is for our target
                    for(int i = 0; i < cardsDue; i++) {
                        AddCard(ManagerOfDeck.MakeCard());
                    }
                }

                if (!canreverse)
                {
                    for (int i = 0; i < score; i++)
                    {
                        if (target == "player1")
                        {
                            player1.AddCard(ManagerOfDeck.MakeCard());
                        }
                        else if (target == "player2")
                        {
                            player2.AddCard(ManagerOfDeck.MakeCard());
                        }
                        else
                        {
                            print("no target");
                        }
                    }
                }

                PlayerParty3.SwitchTurn();

                // deselect selected cards

                List<GameObject> destroyLater = new List<GameObject>();
                for (int i = 0; i < SelectedCards.Count; i++)
                {
                    CardScript cardScript = SelectedCards[i].GetComponent<CardScript>();
                    Hand.Remove(SelectedCards[i]);
                    destroyLater.Add(SelectedCards[i]);
                    GameObject destroyThis = cardScript.gameObject;
                    Destroy(destroyThis);
                }

                for(int i = 0; i < destroyLater.Count; i++)
                {
                    SelectedCards.Remove(destroyLater[i]);
                }
                destroyLater.Clear();
                CurrentPage = 1;
            }
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
            if (Hand.Count == 0) {
                SceneManager.LoadScene("WinPlayer1", LoadSceneMode.Single);
            }
            Digit1P1.value = digdig1;
            Digit2P1.value = digdig2;
        } else if (CompareTag("Player2")) {
            if (Hand.Count == 0) {
                SceneManager.LoadScene("WinPlayer2", LoadSceneMode.Single);
            }
            Digit1P2.value = digdig1;
            Digit2P2.value = digdig2;
        }

        if(PlayerParty3.CurrentTurn == false)
        {
            TurnIndi1.SetActive(true);
            TurnIndi2.SetActive(false);
            PlayHand1.SetActive(true);
            PlayHand2.SetActive(false);
        } else
        {
            TurnIndi1.SetActive(false);
            TurnIndi2.SetActive(true);
            PlayHand1.SetActive(false);
            PlayHand2.SetActive(true);
        }
    }
}












// DOUG DOT PNG