using UnityEngine;

[CreateAssetMenu(fileName="NewCardData", menuName="Card Data", order=1)] // magic
public class CardData : ScriptableObject
{
    public int suit;     // 1,2,3,4 Hearts,Diamonds,Spades,Clubs
    public int number;   // 1-10 = Ace-10, then 11 12 & 13
}
