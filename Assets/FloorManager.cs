using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{

    [SerializeField] GameObject[] FloorPrefabs;
    float floor_Width = 1.4f;
    // 使用HashSet存儲已生成物體的Y軸座標
    private HashSet<float> generatedYPositions = new HashSet<float>();


    public void click_trigger(){
        for (float i = -2.0f; i < 5.0f; i++)
        {
            SpawnFloor_entry(i);
        }
    }
    
    public void SpawnFloor_entry(float position)
    {
        if (FloorPrefabs[0] == null)
        {
            Debug.LogError("FloorPrefab is missing!");
            return;
        }

        // 檢查這個Y軸座標是否已存在於HashSet中
        if (generatedYPositions.Contains(position))
        {
            return;
        }

        // 將這個Y軸座標添加到HashSet中
        generatedYPositions.Add(position);

        List<float> usedPositions = new List<float>();
        int numOfFloorsThisLevel = Random.Range(1, 3);
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
            if (Mathf.Abs(pos - newPos) < floor_Width) // 若新位置和已使用的位置之間的差距小於階梯寬度，則認為它們重疊
                return true;
        }
        return false;
    }
}
