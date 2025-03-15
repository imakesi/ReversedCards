using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class CardScript : MonoBehaviour {
    public Player player1;
    public Player player2;
    public Player mainplayer;

    private List<GameObject> thelist;

    private void Awake() {
        player1 = GameObject.Find("Player1").GetComponent<Player>();
        player2 = GameObject.Find("Player2").GetComponent<Player>();

        if(player1 == null) { print("player1 is dead"); }
        if(player2 == null) { print("player2 is dead"); }
    }

    private void Start() {
        if (CompareTag("Player1")) {
            mainplayer = player1;
        }
        else if (CompareTag("Player2")) {
            mainplayer = player2;
        }

        thelist = mainplayer.SelectedCards;
    }

    private void OnMouseDown() {
        if(thelist.Contains(this.gameObject)) {
            thelist.Remove(this.gameObject);
        } else {
            thelist.Add(this.gameObject);
        }
    }

    private void Update() {
        SpriteRenderer image = transform.GetComponentInChildren<SpriteRenderer>();
        if (thelist.Contains(this.gameObject)) {
            image.color = Color.green;
        } else
        {
            image.color = Color.white;
        }
    }
}
