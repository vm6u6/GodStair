using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    private float ori_position;
    private Transform mainActorTransform;
    private Transform backgroundTransform;

    // Start is called before the first frame update
    void Start()
    {
        ori_position = transform.position.y;
        mainActorTransform = GameObject.Find("main_actor").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (mainActorTransform.position.y - ori_position > 6f)
        {
            transform.parent.GetComponent<FloorManager>().SpawnFloor(transform.position);
        }
         if (mainActorTransform.position.y - ori_position > 10f)
        {
            Destroy(gameObject);
        }
    }   
}

