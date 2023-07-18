using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    private float ori_position;

    // Start is called before the first frame update
    void Start()
    {
        ori_position = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y - ori_position > 8f)
        {
            Destroy(gameObject);
            transform.parent.GetComponent<FloorManager>().SpawnFloor();
        }
    }   
}

