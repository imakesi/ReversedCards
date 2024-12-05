using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private List<GameObject> Hand = new List<GameObject>();

    public void AddCard(GameObject card) {
        Hand.Add(card);
    }
}
