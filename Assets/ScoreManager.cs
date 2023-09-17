using UnityEngine;
using System.IO;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class ScoreManager : MonoBehaviour
{
    private string dataFilePath = "Assets/gameDataRecord.txt"; // Corrected file path
    [SerializeField] private TextMeshProUGUI textMeshPro_score;
    private int best = 0;
    private int score = 0;
    private main_actor MainScript;


    void Start(){
       MainScript = FindObjectOfType<main_actor>();
       best = int.Parse(File.ReadAllText(dataFilePath));
    }

    void Update(){
        score = MainScript.max_floor;
        if (score > best)
        {
            best = score;
            File.WriteAllText(dataFilePath, best.ToString());
        }
        textMeshPro_score.text = best.ToString("D4") + "F";
    }
}