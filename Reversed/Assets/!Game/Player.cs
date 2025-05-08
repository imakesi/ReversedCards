using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.SceneManagement;

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
    private void Start() {
        Digit1P1 = GameObject.Find("Digit 1 P1").GetComponent<DigitScript>();
        Digit2P1 = GameObject.Find("Digit 2 P1").GetComponent<DigitScript>();
        Digit1P2 = GameObject.Find("Digit 1 P2").GetComponent<DigitScript>();
        Digit2P2 = GameObject.Find("Digit 2 P2").GetComponent<DigitScript>();

        TurnIndi1 = GameObject.Find("TurnIndi1");
        TurnIndi2 = GameObject.Find("TurnIndi2");
        PlayHand1 = GameObject.Find("PlayHand1");
        PlayHand2 = GameObject.Find("PlayHand2");

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
            bool straightcompatible = true;
            GameObject currentcard;

            List<int> dataNumbers = new List<int>();
            List<int> dataSuits = new List<int>();
            for (int i = 0; i < SelectedCards.Count; i++) {
                //currentcard = SelectedCards[i];
                //CardIMGManager curmanage = currentcard.GetComponentInChildren<CardIMGManager>();
                //CardData curdata = curmanage.CardObj;

                // get texture?
                currentcard = SelectedCards[i];
                SpriteRenderer currenderer = currentcard.GetComponentInChildren<SpriteRenderer>(); // error, destroyed already
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
            // pretty buggy
            //bool straightboost = false;
            //if(straightcompatible) {
            //    // this hand is a straight and not 1 card
            //    print("wow you just played a straight");
            //    if(dataSuits.Distinct().Count() == 1) {
            //        // all the suits are the same
            //        straightboost = true;
            //    }
            //}

            // logic for pairs 1-4
            if (!(dataNumbers.Distinct().Count() == 1)) {
                compatible = false;
            }

            if (compatible || straightcompatible || flushcompatible) {
                // score the played hand

                string checkJQK = "";
                float score = 0;
                for (int i = 0; i < SelectedCards.Count; i++) {
                    //currentcard = SelectedCards[i];
                    //CardIMGManager curmanage = currentcard.GetComponentInChildren<CardIMGManager>();
                    //CardData curdata = curmanage.CardObj;

                    // get texture?
                    currentcard = SelectedCards[i];
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

                // pretty buggy straightboost
                //if(straightboost) {
                //    score *= 2;
                //}

                if(checkJQK == "JQK" && CanJQK) { score = JQKPair; CanJQK = false; }
                if(score > maxscore) { score = maxscore; }

                score = MathF.Ceiling(score);
                print($"scored hand: {score}");

                string target = null;

                // add cards to the other side
                if (CompareTag("Player1"))
                {
                    //for (int i = 0; i < score; i++)
                    //{
                    //    player2.AddCard(ManagerOfDeck.MakeCard());
                    //}
                    target = "player2";
                }
                else if (CompareTag("Player2"))
                {
                    //for (int i = 0; i < score; i++)
                    //{
                    //    player1.AddCard(ManagerOfDeck.MakeCard());
                    //}
                    target = "player1";
                }

                // change target with reverse mechanic
                // find if other player can use mechanic
                // if so, switch turn without adding cards just yet
                // if they decide to use the mechanic, give cards to original attacking player and don't switch turns
                // if not, give them the cards and switch turns normally
                // special information thing potentially

                for(int i = 0; i < score; i++)
                {
                    if(target == "player1")
                    {
                        player1.AddCard(ManagerOfDeck.MakeCard());
                    } else if(target == "player2")
                    {
                        player2.AddCard(ManagerOfDeck.MakeCard());
                    } else
                    {
                        print("no target");
                    }
                }

                PlayerParty3.SwitchTurn();

                // deselect selected cards

                List<GameObject> destroyLater = new List<GameObject>();
                for (int i = 0; i < SelectedCards.Count; i++)
                {
                    CardScript cardScript = SelectedCards[i].GetComponent<CardScript>();
                    //cardScript.image.transform.position = cardScript.ogpos.position;
                    //cardScript.transform.position = cardScript.ogpos.position;
                    Hand.Remove(SelectedCards[i]);
                    destroyLater.Add(SelectedCards[i]);
                    GameObject destroyThis = cardScript.gameObject;
                    if(destroyThis == null) { print("card " + i + " is a spoooky ghost"); }
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

        print("it's " + PlayerParty3.CurrentTurn);
        if(PlayerParty3.CurrentTurn == false)
        {
            TurnIndi1.SetActive(true);
            TurnIndi2.SetActive(false);
            PlayHand1.SetActive(true);
            PlayHand2.SetActive(false);
            print("it's player 1");
        } else
        {
            TurnIndi1.SetActive(false);
            TurnIndi2.SetActive(true);
            PlayHand1.SetActive(false);
            PlayHand2.SetActive(true);
            print("it's player 2, the sequel");
        }
    }
}












// DOUG DOT PNG