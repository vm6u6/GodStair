using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundManager : MonoBehaviour
{
    [SerializeField] GameObject[] BackGroundPrefabs;

    public void SpawnBackground(Vector3 position)
    {
        // 生成背景对象
        GameObject background = Instantiate(BackGroundPrefabs[0], transform);
        background.transform.position = new Vector3(position.x, position.y);

        // 生成左边界对象
        GameObject left_bond = Instantiate(BackGroundPrefabs[1], transform);
        left_bond.transform.position = new Vector3(position.x - 8f, position.y);

        // 生成右边界对象
        GameObject right_bond = Instantiate(BackGroundPrefabs[2], transform);
        right_bond.transform.position = new Vector3(position.x + 8f, position.y);
    }
}