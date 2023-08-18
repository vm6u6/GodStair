using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundManager : MonoBehaviour
{
    [SerializeField] GameObject[] BackGroundPrefabs;
    [SerializeField] GameObject[] FloorPrefabs;
    float floor_init_y = -4.6f;
    float floor_Width = 1.4f;

    public void SpawnBackground(Vector3 position)
    {
        if (BackGroundPrefabs[0] == null || BackGroundPrefabs[1] == null || BackGroundPrefabs[2] == null)
        {
            Debug.LogError("One of the BackGroundPrefabs is missing!");
            return;
        }

        InstantiateBackground(BackGroundPrefabs[0], 0, position.y + 30f);
        InstantiateBackground(BackGroundPrefabs[1], - 4.8f, position.y + 30f);
        InstantiateBackground(BackGroundPrefabs[2],  4.8f, position.y + 30f);
    }

    void InstantiateBackground(GameObject prefab, float x, float y)
    {
        GameObject instance = Instantiate(prefab, transform);
        instance.transform.position = new Vector3(x, y);
    }


    public void SpawnFloor(Vector3 position)
    {
        if (FloorPrefabs[0] == null)
        {
            Debug.LogError("FloorPrefab is missing!");
            return;
        }
        List<float> usedPositions = new List<float>();
        for (int i=0; i<4; i++)
        {
            int numOfFloorsThisLevel = Random.Range(1, 3);  // 每個Y位置隨機選擇1到3個階梯

            for (int j = 0; j < numOfFloorsThisLevel; j++)
            {
                GameObject floor = Instantiate(FloorPrefabs[0], transform);
                float xPos;
                int attempts = 0;
                
                do
                {
                    xPos = Random.Range(-4.8f + floor_Width / 2, 4.8f - floor_Width / 2);
                    attempts++;
                    
                    if (attempts > 10)
                        break;
                } 
                while (IsOverlap(usedPositions, xPos));
                
                if (attempts <= 10)
                {
                    usedPositions.Add(xPos);
                    floor.transform.position = new Vector3(xPos, floor_init_y + 30f);
                }
            }

            floor_init_y += 2;  // 每次迴圈後增加1
            usedPositions.Clear();  
        }
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
