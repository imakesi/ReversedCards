using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    private List<GameObject> Hand = new List<GameObject>();
    private int CurrentPage = 1; // q & e to switch?
    private double thingy;

    [SerializeField] private Transform[] CardPositions;

    public void AddCard(GameObject card) {
        Hand.Add(card);
    }

    private void DisplayHand() {
        thingy = Hand.Count / 5;
        for (int j = 0; j < Math.Floor(thingy); j++) {
            for (int i = 0; i < 5; i++) {
                Hand[i].transform.position = CardPositions[i].position;
            } for (int i = 0; i < Hand.Count % 5; i++) {
                Hand[i].transform.position = CardPositions[i].position;
            }
        }
    }

    private void Update() {
        DisplayHand();
    }
}
