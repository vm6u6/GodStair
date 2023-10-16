using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    private List<float> lastUpdatedY_stairs = new List<float>();   
    private List<float> moving_stairs = new List<float>();  
    private Transform mainActorTransform;
    private float updateInterval = 7.7177f;
    private float destroyThreshold = 7.7177f * 2;
    private bool canUpdateStairs = true;
    
    private int cnt_floor_num = 0; 
    private char[] delimiter = { '_' };
    public float moveSpeed = 0.5f;
    private float originalY;
    private float minY;
    private float maxY;

    // Start is called before the first frame update
    void Start()
    {
        mainActorTransform = GameObject.Find("main_actor").transform;
        for (int i = -2; i < 5; i++)
        {
            lastUpdatedY_stairs.Add(i);
        }
        originalY = transform.position.y;
        minY = originalY;
        maxY = originalY + 1.0f; 
    }

    // Update is called once per frame
    void Update()
    {
        LevelManager levelManager = FindObjectOfType<LevelManager>();
        int levelOptionCnt = levelManager.level_option_cnt;
        if (cnt_floor_num >= lastUpdatedY_stairs.Count) 
        cnt_floor_num = 0;

        // Debug.Log(mainActorTransform.position.y);   
        // Debug.Log(cnt_floor_num);
        if (mainActorTransform.position.y - lastUpdatedY_stairs[cnt_floor_num] >= -2 && canUpdateStairs)
        {
            //Debug.Log("update Stairs...");
            lastUpdatedY_stairs[cnt_floor_num] += updateInterval;
            if (levelOptionCnt == 0){
                transform.parent.GetComponent<FloorManager>().SpawnFloor_entry(lastUpdatedY_stairs[cnt_floor_num]);
            }
            else if (levelOptionCnt == 1){
                transform.parent.GetComponent<FloorManager>().SpawnFloor_medium(lastUpdatedY_stairs[cnt_floor_num]);
            }
            else if (levelOptionCnt == 2){
                transform.parent.GetComponent<FloorManager>().SpawnFloor_hard(lastUpdatedY_stairs[cnt_floor_num]);
            }

            cnt_floor_num++;
            canUpdateStairs = false;  
        }
        else if (mainActorTransform.position.y - lastUpdatedY_stairs[cnt_floor_num] < -2 || mainActorTransform.position.y - lastUpdatedY_stairs[cnt_floor_num] > 0)
        {
            canUpdateStairs = true;  
        }

        // TODO Moving Floor
        if (levelOptionCnt == 2){
            // Debug.Log(transform.position);
            // Debug.Log(gameObject.tag);
            if (gameObject.tag == "stair_move" || 
                gameObject.tag == "jump_stair_move" || 
                gameObject.tag == "acc_leftstair_move" || 
                gameObject.tag == "acc_right_stair_move" ){
                    
                    float newY = Mathf.PingPong(Time.time * moveSpeed, maxY - minY) + minY;
                    Vector3 newPosition = transform.position;
                    newPosition.y = newY;
                    transform.position = newPosition;
                }
        }

        if (mainActorTransform.position.y - transform.position.y > destroyThreshold)
        {
            Destroy(gameObject);
        }
        
    }
}
