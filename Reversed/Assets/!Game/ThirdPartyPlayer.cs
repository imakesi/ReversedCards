using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPartyPlayer : MonoBehaviour
{
    public bool CurrentTurn = false; // false is player1, true is player2

    public void SwitchTurn()
    {
        CurrentTurn = !CurrentTurn;
    }
}
