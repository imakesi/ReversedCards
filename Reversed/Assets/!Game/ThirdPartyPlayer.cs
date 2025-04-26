using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPartyPlayer : MonoBehaviour
{
    public bool CurrentTurn = false; // false is player1, true is player2

    public Player player1;
    public Player player2;

    private void Start()
    {
        player1 = GameObject.Find("Player1").GetComponent<Player>();
        player2 = GameObject.Find("Player2").GetComponent<Player>();
    }

    public void player1Play() {
        player1.PlayHand();
    } public void player2Play() {
        player2.PlayHand();
    }
    public void SwitchTurn()
    {
        print("Number 1 " + CurrentTurn);
        if (CurrentTurn == false) { CurrentTurn = true; }
        else if (CurrentTurn == true) { CurrentTurn = false; }
        print("number 9 large " + CurrentTurn);
        //CurrentTurn = true;
        //print("wheee");
    }
}
