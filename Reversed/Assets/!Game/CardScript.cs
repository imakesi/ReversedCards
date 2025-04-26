using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CardScript : MonoBehaviour {
    public Player player1;
    public Player player2;
    public Player mainplayer;
    public ThirdPartyPlayer PlayerParty3;

    public Transform ogpos;
    public SpriteRenderer image;

    private void Awake() {
        image = transform.GetComponentInChildren<SpriteRenderer>();
        ogpos = this.transform;

        player1 = GameObject.Find("Player1").GetComponent<Player>();
        player2 = GameObject.Find("Player2").GetComponent<Player>();
        PlayerParty3 = GameObject.Find("Manager").GetComponent<ThirdPartyPlayer>();

        if(player1 == null) { print("player1 is dead"); }
        if(player2 == null) { print("player2 is dead"); }
    }

    private void Start() {
        if (CompareTag("Player1"))
        {
            mainplayer = player1;
        }
        else if (CompareTag("Player2"))
        {
            mainplayer = player2;
        }
    }

    public void OnMouseDown() {
        if((!PlayerParty3.CurrentTurn && mainplayer == player2)
            || (PlayerParty3.CurrentTurn && mainplayer == player1))
        {
            return;
        }

        if(mainplayer.SelectedCards.Contains(this.gameObject)) {
            mainplayer.SelectedCards.Remove(this.gameObject);
        } else if(!mainplayer.SelectedCards.Contains(this.gameObject) &&
            mainplayer.SelectedCards.Count < mainplayer.SelectedLimit) {
            mainplayer.SelectedCards.Add(this.gameObject);
        }

        if (mainplayer.SelectedCards.Contains(this.gameObject))
        {
            int changer = 1;
            if(mainplayer == player1)
            {
                changer = -1;
            }
            image.transform.Translate(0, changer, 0);
            transform.Translate(0, changer, 0);
        }
        else
        {
            image.transform.position = ogpos.position;
            transform.position = ogpos.position;
        }
    }
}
