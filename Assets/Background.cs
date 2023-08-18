using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    private float lastUpdatedY = 0f;
    private Transform mainActorTransform;
    private float updateInterval = 15.0f;
    private float destroyThreshold = 30.0f; // 距離閾值，超過這個距離的物件會被摧毀

    // Start is called before the first frame update
    void Start()
    {
        mainActorTransform = GameObject.Find("main_actor").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (mainActorTransform.position.y - lastUpdatedY > updateInterval)
        {
            Debug.Log("update");
            UpdateBackground();
            lastUpdatedY += updateInterval;
        }

        // 使用物件自身的位置計算距離
        if (mainActorTransform.position.y - transform.position.y > destroyThreshold)
        {
            Destroy(gameObject);
        }
    }

    void UpdateBackground()
    {
        transform.parent.GetComponent<BackGroundManager>().SpawnBackground(transform.position);
        transform.parent.GetComponent<BackGroundManager>().SpawnFloor(transform.position);
    }
}