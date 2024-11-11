using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CardBackManager : MonoBehaviour
{
    public GameObject CardPrefab;
    public GameObject PrefabParent;

    private void OnMouseDown() {
        Debug.Log("clicked");
        GameObject NewCard = Instantiate(CardPrefab, Vector3.zero, Quaternion.identity);
        NewCard.transform.SetParent(PrefabParent.transform);
    }
}
