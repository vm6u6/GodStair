using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    private float ori_position;
    

    // Start is called before the first frame update
    void Start()
    {
        ori_position = transform.position.y;
        Debug.Log(ori_position);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(transform.position.y);
        if (transform.position.y - ori_position > 8f)
        {
            Destroy(gameObject);
            transform.parent.GetComponent<BackGroundManager>().SpawnBackground(transform.position);
        }
    }
}