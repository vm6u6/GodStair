using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    private List<float> lastUpdatedY_stairs = new List<float>();    // 儲存樓梯的Y軸位置
    private Transform mainActorTransform;
    private float updateInterval = 7.7177f;
    private float destroyThreshold = 10.0f;
    private bool canUpdateStairs = true;

    int cnt_floor_num = 0; 

    // Start is called before the first frame update
    void Start()
    {
        mainActorTransform = GameObject.Find("main_actor").transform;

        // 初始化樓梯的Y軸位置
        for (int i = -1; i < 5; i++)
        {
            lastUpdatedY_stairs.Add(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (cnt_floor_num >= lastUpdatedY_stairs.Count)  // 使用>=是更安全的，以防止索引超出範圍
            cnt_floor_num = 0;

        // Debug.Log(mainActorTransform.position.y);   
        // Debug.Log(cnt_floor_num);
        if (mainActorTransform.position.y - lastUpdatedY_stairs[cnt_floor_num] >= -2 && gameObject.tag == "stair" && canUpdateStairs)
        {
            //Debug.Log("update Stairs...");
            transform.parent.GetComponent<FloorManager>().SpawnFloor(lastUpdatedY_stairs[cnt_floor_num]);
            lastUpdatedY_stairs[cnt_floor_num] += updateInterval;
            cnt_floor_num++;
            canUpdateStairs = false;  // 關閉更新樓梯的機會，直到玩家遠離該位置
        }
        else if (mainActorTransform.position.y - lastUpdatedY_stairs[cnt_floor_num] < -2 || mainActorTransform.position.y - lastUpdatedY_stairs[cnt_floor_num] > 0)
        {
            canUpdateStairs = true;  // 當玩家遠離-2的距離時，重新開啟更新樓梯的機會
        }

        if (mainActorTransform.position.y - transform.position.y > destroyThreshold)
        {
            Destroy(gameObject);
        }
    }
}
