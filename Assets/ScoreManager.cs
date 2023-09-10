using UnityEngine;
using System.IO;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class ScoreManager : MonoBehaviour
{
    private string dataFilePath = "gameData.json";
    private Dictionary<string, int> gameData = new Dictionary<string, int>();
    [SerializeField] private TextMeshProUGUI textMeshPro_score;
    private int highestValue = 0;

    private void Start()
    {
        LoadGameData();
    }


    public void SaveGameData(int floor_cnt)
    {
        string key = "Floor_" + floor_cnt;
        gameData[key] = floor_cnt;

        var sortedData = gameData.OrderByDescending(kv => kv.Value).ToDictionary(kv => kv.Key, kv => kv.Value);

        string json = JsonUtility.ToJson(gameData);
        File.WriteAllText(dataFilePath, json);
    }

    public void LoadGameData()
    {
        if (File.Exists(dataFilePath))
        {
            string json = File.ReadAllText(dataFilePath);
            gameData = JsonUtility.FromJson<Dictionary<string, int>>(json);

            
            foreach (var kvp in gameData)
            {
                if (kvp.Value > highestValue)
                {
                    highestValue = kvp.Value;
                }
            }
        }
        textMeshPro_score.text = highestValue.ToString("D4") + "F";
    }
}