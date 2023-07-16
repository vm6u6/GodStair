using UnityEngine;

public class Main_camera : MonoBehaviour
{
    public Transform target;  // 要跟随的目标对象
    public Vector2 offset;  // 相机和角色之间的偏移量

    void LateUpdate()
    {
        // 计算相机的目标位置
        Vector3 desiredPosition = new Vector3(transform.position.x, 3.8f + target.position.y + offset.y, transform.position.z);

        // // 使用平滑插值过渡到目标位置
        // Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // 更新相机的位置
        transform.position =  desiredPosition;
    }
}