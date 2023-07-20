using UnityEngine;

[CreateAssetMenu(fileName = "Current Level", menuName = "Scriptable Objects/Current Level (there should only be one of this)")]
public class CurrentLevel : ScriptableObject
{
    public Level currentLevelInfo;
}
