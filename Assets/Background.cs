using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Background : MonoBehaviour
{
    private Transform mainActorTransform;
    private float updateInterval = 7.7177f;
    private float destroyThreshold = 10.0f;
    private bool canUpdateBackground = true;
    private float lastUpdatedY_Background = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Background Start method called!");
        mainActorTransform = GameObject.Find("main_actor").transform;
    }

    public void Initialize(float lastUpdatedValue)
    {
        lastUpdatedY_Background = lastUpdatedValue;
    }

    // Update is called once per frame
    void Update()
    {
        float actorBackground_dis = mainActorTransform.position.y - lastUpdatedY_Background;
        if (actorBackground_dis >= -3 && canUpdateBackground && gameObject.tag == "background")
        {
            transform.parent.GetComponent<BackGroundManager>().SpawnBackground(lastUpdatedY_Background);
            lastUpdatedY_Background = lastUpdatedY_Background + updateInterval;
            canUpdateBackground = false; 
        }
        else if (mainActorTransform.position.y - lastUpdatedY_Background < -3 || mainActorTransform.position.y - lastUpdatedY_Background > 0)
        {
            canUpdateBackground = true;  
        }

        if (mainActorTransform.position.y - transform.position.y > destroyThreshold)
        {
            Destroy(gameObject);
        }
    }
}

