using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    private float ori_position;
    private Transform mainActorTransform;

    // Start is called before the first frame update
    void Start()
    {
        ori_position = transform.position.y;
        mainActorTransform = GameObject.Find("main_actor").transform;
        //Debug.Log(ori_position);
    }

    // Update is called once per frame
    void Update()
    {
        if (mainActorTransform.position.y - ori_position > 6f )
        {
            if ( gameObject.tag == "BackGround" )
            {
                transform.parent.GetComponent<BackGroundManager>().SpawnBackground(transform.position);
            }
        }

        if (mainActorTransform.position.y - ori_position > 10f)
        {
            Destroy(gameObject);
        }
    }
}