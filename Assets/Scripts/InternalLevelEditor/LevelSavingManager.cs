using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class LevelSavingManager : MonoBehaviour
{
    public static LevelSavingManager instance;

    [Header("Editor settings")]
    public List<CustomLevelEditorTile> customTiles;

    public Tilemap tilemap;

    [Header("Play mode settings")]
    [SerializeField] CurrentLevel levelInfo;
    [SerializeField] bool loadLevelOnStart = false;
    [SerializeField] AudioSource musicSource;
    //[SerializeField] MeshFilter levelMesh;
    //[SerializeField] GameObject[] targetObjects;

    List<CoinObject> coins = new List<CoinObject>();

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);

        if (loadLevelOnStart)
        {
            ResetCoinScriptables();
            LoadLevelForPlayMode();
        }
    }

    private void Update()
    {
        if (SceneManager.GetSceneByBuildIndex(
            SceneManager.GetActiveScene().buildIndex).name == "InternalLevelEditor")
        {
            if (Keyboard.current.f1Key.wasPressedThisFrame) SaveLevel();
            if (Keyboard.current.f2Key.wasPressedThisFrame) LoadLevelForEditor();
        }
    }

    void SaveLevel()
    {
        BoundsInt bounds = tilemap.cellBounds;
        LevelData levelData = new LevelData();

        print("Starting level saving!");

        print(bounds);

        for (int x = bounds.min.x; x < bounds.max.x; x++)
        {
            for (int y = bounds.min.y; y < bounds.max.y; y++)
            {
                TileBase temp = tilemap.GetTile(new Vector3Int(x, y));
                CustomLevelEditorTile tempTile = customTiles.Find(t => t.tile == temp);

                if (tempTile != null)
                {
                    levelData.tiles.Add(tempTile.id);
                    levelData.poses.Add(new Vector2Int(x, y));
                    print($"Added {tempTile.id} at {x}, {y}");
                }
            }
        }

        if (levelData.tiles.Count > 0 && levelData.poses.Count > 0)
        {
            string jsonData = JsonUtility.ToJson(levelData);
            File.WriteAllText(Application.dataPath + "/Levels/MyCoolLevel.json", jsonData);
            print("Level successfully saved to " + Application.dataPath + "/Levels/MyCoolLevel.json");
        }
        else
        {
            print("Failed to save the level. :(");
        }
    }

    void LoadLevelForEditor()
    {
        string jsonData = File.ReadAllText(Application.dataPath + "/Levels/MyCoolLevel.json");
        LevelData data = JsonUtility.FromJson<LevelData>(jsonData);

        tilemap.ClearAllTiles();

        for (int i = 0; i < data.tiles.Count; i++)
        {
            Vector3Int setPos = new Vector3Int(data.poses[i].x, data.poses[i].y);

            tilemap.SetTile(setPos, customTiles.Find(t => t.id == data.tiles[i]).tile);
        }
    }

    void LoadLevelForPlayMode()
    {
        string jsonData = levelInfo.currentLevelInfo.levelFile.text;
        LevelData data = JsonUtility.FromJson<LevelData>(jsonData);

        CoinData coinData = new CoinData();
        if (File.Exists(Application.persistentDataPath + $"/CC{levelInfo.currentLevelInfo.levelName}CoinData.json"))
        {
            string coinJsonData = File.ReadAllText(Application.persistentDataPath 
                + $"/CC{levelInfo.currentLevelInfo.levelName}CoinData.json");
            coinData = JsonUtility.FromJson<CoinData>(coinJsonData);
        }

        for (int i = 0; i < data.tiles.Count; i++)
        {
            Vector3Int setPos = new Vector3Int(data.poses[i].x, data.poses[i].y + 1);
            GameObject instantiatedObj = Instantiate(customTiles.Find(t => t.id == data.tiles[i]).gameObj, 
                setPos, Quaternion.identity);

            if (instantiatedObj.TryGetComponent<CoinObject>(out CoinObject coinObject))
                coins.Add(instantiatedObj.GetComponent<CoinObject>());
        }

        foreach (Coin levelInfoCoin in levelInfo.currentLevelInfo.coins)
        {
            print("1");
            foreach (CoinObject coin in coins)
            {
                print("1.1");
                if (coins.IndexOf(coin) == levelInfo.currentLevelInfo.coins.IndexOf(levelInfoCoin))
                {
                    coin.coinScriptableObject = levelInfoCoin;
                    foreach (int collected in coinData.collected)
                    {
                        print("1.2");
                        if (coin.coinScriptableObject.id == levelInfoCoin.id)
                            coin.coinScriptableObject.isCollected = System.Convert.ToBoolean(collected);
                    }
                }
            }
        }

        musicSource.clip = levelInfo.currentLevelInfo.levelMusic;
        musicSource.Play();
    }

    public void SaveCoinData()
    {
        CoinData coinData = new CoinData();

        foreach (CoinObject coin in coins)
        {
            coinData.coinIds.Add(coin.coinScriptableObject.id);
            coinData.collected.Add(System.Convert.ToInt32(coin.coinScriptableObject.isCollected));
        }

        string jsonData = JsonUtility.ToJson(coinData);
        File.WriteAllText(Application.persistentDataPath + 
            $"/CC{levelInfo.currentLevelInfo.levelName}CoinData.json", jsonData);
    }

    public void ResetCoinScriptables()
    {
        foreach (Coin coin in levelInfo.currentLevelInfo.coins)
        {
            coin.isCollected = false;
        }
    }
}

public class LevelData
{
    public List<string> tiles = new List<string>();
    public List<Vector2Int> poses = new List<Vector2Int>();
}

public class CoinData
{
    public List<int> coinIds = new List<int>();
    public List<int> collected = new List<int>();
}