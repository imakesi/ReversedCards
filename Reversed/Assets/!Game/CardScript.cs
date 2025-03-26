using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CardScript : MonoBehaviour {
    public Player player1;
    public Player player2;
    public Player mainplayer;

    private Transform ogpos;

    private void Awake() {
        ogpos = this.transform;

        player1 = GameObject.Find("Player1").GetComponent<Player>();
        player2 = GameObject.Find("Player2").GetComponent<Player>();

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

    private void OnMouseDown() {
        if(mainplayer.SelectedCards.Contains(this.gameObject)) {
            mainplayer.SelectedCards.Remove(this.gameObject);
        } else if(!mainplayer.SelectedCards.Contains(this.gameObject) &&
            mainplayer.SelectedCards.Count < mainplayer.SelectedLimit) {
            mainplayer.SelectedCards.Add(this.gameObject);
        }

        SpriteRenderer image = transform.GetComponentInChildren<SpriteRenderer>();
        if (mainplayer.SelectedCards.Contains(this.gameObject))
        {
            image.transform.Translate(0, 1, 0);
            transform.Translate(0, 1, 0);
            image.color = Color.green;
        }
        else
        {
            image.transform.position = ogpos.position;
            transform.position = ogpos.position;
            image.color = Color.white;
        }
    }
}
