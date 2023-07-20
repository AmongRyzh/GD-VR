using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class LevelEditor : MonoBehaviour
{
    [SerializeField] Tilemap currentTilemap;
    [SerializeField] float cameraMovementSpeed;
    [SerializeField] Text currentTileText;

    TileBase currentTile
    {
        get
        {
            return LevelSavingManager.instance.customTiles[_selectedTileIndex].tile;
        }
    }

    int _selectedTileIndex;

    private void Update()
    {
        Vector3Int pos = currentTilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()));

        if (Mouse.current.leftButton.isPressed && pos.y >= -1) ChangeTilemap(pos, false);
        if (Mouse.current.rightButton.isPressed && pos.y >= -1) ChangeTilemap(pos, true);

        if (Keyboard.current.leftShiftKey.wasPressedThisFrame)
        {
            _selectedTileIndex++;
            if (_selectedTileIndex >= LevelSavingManager.instance.customTiles.Count) _selectedTileIndex = 0;
            currentTileText.text = "Tile: " + currentTile.name;
        }
        if (Keyboard.current.rightShiftKey.wasPressedThisFrame)
        {
            _selectedTileIndex--;
            if (_selectedTileIndex < 0) _selectedTileIndex = LevelSavingManager.instance.customTiles.Count - 1;
            currentTileText.text = "Tile: " + currentTile.name;
        }

        Vector3 camMovement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Keyboard.current.leftCtrlKey.isPressed)
            Camera.main.transform.position += camMovement.normalized * Time.deltaTime * cameraMovementSpeed * 2;
        else
            Camera.main.transform.position += camMovement.normalized * Time.deltaTime * cameraMovementSpeed;
    }

    void ChangeTilemap(Vector3Int pos, bool placeNull)
    {
        if (!placeNull)
            currentTilemap.SetTile(pos, currentTile);
        else
            currentTilemap.SetTile(pos, null);
    }
}
