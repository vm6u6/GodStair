using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{

    [SerializeField] GameObject[] FloorPrefabs;
    float floor_Width = 2f;
    // 使用HashSet存儲已生成物體的Y軸座標
    private HashSet<float> generatedYPositions = new HashSet<float>();
    //private int levelOptionCnt = 0;

    // Initial the floor
    public void click_trigger(){
        LevelManager levelManager = FindObjectOfType<LevelManager>();
        if (levelManager != null){
            int levelOptionCnt = levelManager.level_option_cnt;
            //Debug.Log("level_option_cnt: " + levelOptionCnt);

            for (float i = -2.0f; i < 5.0f; i++)
            {
                // Debug.Log("level_option_cnt: " + levelOptionCnt);
                if (levelOptionCnt == 0){
                    SpawnFloor_entry(i);
                }
                else if(levelOptionCnt == 1){
                    SpawnFloor_medium(i);
                }
            }
        }
    }

    public void SpawnFloor_medium(float position)
    {
        if (generatedYPositions.Contains(position))
        {
            return;
        }

        generatedYPositions.Add(position);
        List<float> usedPositions = new List<float>();
        int numOfFloorsThisLevel = Random.Range(1, 4);
        if (numOfFloorsThisLevel > 0)
            for (int j = 0; j < numOfFloorsThisLevel; j++)
            {
                int randomValue = Random.Range(0, 5);
                Debug.Log(randomValue);
                GameObject floor = Instantiate(FloorPrefabs[randomValue], transform);
                float xPos;
                int attempts = 0;
                 
                do
                {
                    xPos = Random.Range(-5.35f + floor_Width / 2, 2.4f - floor_Width / 2);
                    attempts++;
                    
                    if (attempts > 10)
                        break;
                } 
                while (IsOverlap(usedPositions, xPos));
                
                if (attempts <= 10)
                {
                    usedPositions.Add(xPos);
                    floor.transform.position = new Vector3(xPos, position);
                }
            }

        usedPositions.Clear();
    }
    public void SpawnFloor_entry(float position)
    {
        if (FloorPrefabs[0] == null)
        {
            Debug.LogError("FloorPrefab is missing!");
            return;
        }


        if (generatedYPositions.Contains(position))
        {
            return;
        }

        generatedYPositions.Add(position);

        List<float> usedPositions = new List<float>();
        int numOfFloorsThisLevel = Random.Range(1, 4);
        if (numOfFloorsThisLevel > 0)
            for (int j = 0; j < numOfFloorsThisLevel; j++)
            {
                GameObject floor = Instantiate(FloorPrefabs[0], transform);
                float xPos;
                int attempts = 0;
                
                do
                {
                    xPos = Random.Range(-5.35f + floor_Width / 2, 2.4f - floor_Width / 2);
                    attempts++;
                    
                    if (attempts > 10)
                        break;
                } 
                while (IsOverlap(usedPositions, xPos));
                
                if (attempts <= 10)
                {
                    usedPositions.Add(xPos);
                    floor.transform.position = new Vector3(xPos, position);
                }
            }

        usedPositions.Clear();
    }

    bool IsOverlap(List<float> usedPositions, float newPos)
    {
        foreach (float pos in usedPositions)
        {
            if (Mathf.Abs(pos - newPos) <= floor_Width) // 若新位置和已使用的位置之間的差距小於階梯寬度，則認為它們重疊
                return true;
        }
        return false;
    }
}
