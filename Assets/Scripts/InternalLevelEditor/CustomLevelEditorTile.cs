using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Custom Level Editor Tile", menuName = "LevelEditor/Custom Tile")]
public class CustomLevelEditorTile : ScriptableObject
{
    public TileBase tile;
    public string id;
    public GameObject gameObj;
}