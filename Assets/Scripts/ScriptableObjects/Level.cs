using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Scriptable Objects/New Level")]
public class Level : ScriptableObject
{
    public string levelName;
    public TextAsset levelFile;
    public AudioClip levelMusic;
    public List<Coin> coins = new List<Coin>();
}
