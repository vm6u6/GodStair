using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    private bool Entery_level = true;
    private bool Medium_level = false;
    private bool Hard_level = false;
    public int level_option_cnt = 0; 
    private string level_txt = "";
    [SerializeField] private TextMeshProUGUI textMeshPro_butLevel;
    [SerializeField] private TextMeshProUGUI textMeshPro_ShowLevel;

    void Start()
    {
        
    }

    public void ChangerLevel(){
        level_option_cnt += 1;
        if (level_option_cnt > 2){
            level_option_cnt = 0;
        }

        if (level_option_cnt == 0){
            Entery_level = true;
            Medium_level = false; 
            Hard_level = false;
            level_txt = "Game Level: Entery";
            textMeshPro_ShowLevel.text = "Entry";
        }
        if (level_option_cnt == 1){
            Entery_level = false;
            Medium_level = true; 
            Hard_level = false;
            level_txt = "Game Level: Medium";
            textMeshPro_ShowLevel.text = "Medium";
        }
        if (level_option_cnt == 2){
            Entery_level = false;
            Medium_level = false; 
            Hard_level = true;
            level_txt = "Game Level:   Hard";
            textMeshPro_ShowLevel.text = "Hard";
        }
        textMeshPro_butLevel.text = level_txt;
    }
}
