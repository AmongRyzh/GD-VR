using UnityEngine;

[CreateAssetMenu(fileName = "New Coin", menuName = "Scriptable Objects/New Coin")]
public class Coin : ScriptableObject
{
    public bool isCollected;
    public int id;
}
