using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    private List<float> lastUpdatedY_stairs = new List<float>();    
    private Transform mainActorTransform;
    private float updateInterval = 7.7177f;
    private float destroyThreshold = 7.7177f * 2;
    private bool canUpdateStairs = true;

    int cnt_floor_num = 0; 

    // Start is called before the first frame update
    void Start()
    {
        mainActorTransform = GameObject.Find("main_actor").transform;

        for (int i = -2; i < 5; i++)
        {
            lastUpdatedY_stairs.Add(i);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (cnt_floor_num >= lastUpdatedY_stairs.Count) 
        cnt_floor_num = 0;

        // Debug.Log(mainActorTransform.position.y);   
        // Debug.Log(cnt_floor_num);
        if (mainActorTransform.position.y - lastUpdatedY_stairs[cnt_floor_num] >= -2 && gameObject.tag == "stair" && canUpdateStairs)
        {
            //Debug.Log("update Stairs...");
            lastUpdatedY_stairs[cnt_floor_num] += updateInterval;
            transform.parent.GetComponent<FloorManager>().SpawnFloor_entry(lastUpdatedY_stairs[cnt_floor_num]);
            cnt_floor_num++;
            canUpdateStairs = false;  
        }
        else if (mainActorTransform.position.y - lastUpdatedY_stairs[cnt_floor_num] < -2 || mainActorTransform.position.y - lastUpdatedY_stairs[cnt_floor_num] > 0)
        {
            canUpdateStairs = true;  
        }

        if (mainActorTransform.position.y - transform.position.y > destroyThreshold)
        {
            Destroy(gameObject);
        }
        
    }
}
