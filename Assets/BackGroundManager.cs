using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundManager : MonoBehaviour
{
    [SerializeField] GameObject[] BackGroundPrefabs;

    public void SpawnBackground(Vector3 position)
    {
        GameObject background = Instantiate(BackGroundPrefabs[0], transform);
        background.transform.position = new Vector3(position.x, position.y + 19.853744f);

        GameObject left_bond = Instantiate(BackGroundPrefabs[1], transform);
        left_bond.transform.position = new Vector3(position.x - 4.866793f, position.y + 19.853744f);

        GameObject right_bond = Instantiate(BackGroundPrefabs[2], transform);
        right_bond.transform.position = new Vector3(position.x + 4.829999f, position.y + 19.853744f);
    }
}