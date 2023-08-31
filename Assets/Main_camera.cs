using UnityEngine;

public class Main_camera : MonoBehaviour
{
    public Transform target;  
    private float distance = 3.8f + -2.151625f;


    void LateUpdate() 
    {
        Vector3 desiredPosition = new Vector3(transform.position.x, distance, transform.position.z);

        if(target.position.y >= distance + 1.0f) 
        {
            distance += target.position.y - distance - 1.0f; 
        }

        transform.position = desiredPosition;
    }
}





// [20230831] Previous version___________________________________________

// public class Main_camera : MonoBehaviour
// {
//     public Transform target;  
//     public Vector2 offset;  
//     private float distance = 3.8f;

//     void LateUpdate()
//     { 
//         // if (target.position.y >= 1 && distance < 4.8){
//         //     distance = distance + 0.0001f;
//         //     Vector3 desiredPosition = new Vector3(transform.position.x, distance + target.position.y + offset.y, transform.position.z);
//         //     transform.position = desiredPosition;
            
//         // }
//         // else{
//         //     Vector3 desiredPosition = new Vector3(transform.position.x, distance + target.position.y + offset.y, transform.position.z);
//         //     transform.position = desiredPosition;
//         // }
//         // Debug.Log(distance);

//         Vector3 desiredPosition = new Vector3(transform.position.x, distance + target.position.y + offset.y, transform.position.z);
//         transform.position = desiredPosition;
//     }
// }