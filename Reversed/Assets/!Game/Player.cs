using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    private List<GameObject> Hand = new List<GameObject>();
    private int CurrentPage = 1;

    [SerializeField] private Transform[] CardPositions;

    [SerializeField] private KeyCode PageNext, PageLast;

    public void AddCard(GameObject card) {
        Hand.Add(card);
    }

    private void DisplayHand() {
        // min = current page number minus 1 times 5 (last page which we don't want to display unless we're on the first page) and then plus one
        // kinda like fibbonacci thing where you go down until 1 ^
        // max = min + 4
        // get range somehow [min, min+1, min+2, min+3, max]
        // use indexes and get the transform of the cards in the hand list and move them to 10,10,0


        int min;

        if (CurrentPage > 1) {
            min = ((CurrentPage - 1) * 5) + 1;
        } else { min = 0; }

        List<int> RangeMM = new List<int>();

        // add cards to RangeMM that should be displayed on page
        for (int i = min; i < min + 5; i++) {
            if (Hand.Count < i) { break; }

            RangeMM.Add(i);
        }

        for (int i = 0; i < RangeMM.Count; i++) {
            print(string.Join(",", RangeMM));
            int MyOtherThing = RangeMM[i];
            GameObject errorpossible = Hand[MyOtherThing];
            errorpossible.transform.position = CardPositions[i].position;
        }

        //int Adder = (CurrentPage - 1) * 5;

        //for(int j = 0; j < 5; j++) {
        //    for(int i = 0; i < 5; i++) {
        //        if (i + Adder >= Hand.Count) { break; }
        //        Hand[i + Adder].transform.position = CardPositions[i].position;
        //    }
        //}
    }

    private void Update() {
        DisplayHand();

        if(Input.GetKeyDown(PageLast) && CurrentPage > 1) {
            CurrentPage--;
        } else if(Input.GetKeyDown(PageNext)) {
            CurrentPage++;
        }
    }
}
