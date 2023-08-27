using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class BackGroundManager : MonoBehaviour
{
    [SerializeField] GameObject[] BackGroundPrefabs;


    public void SpawnBackground(float y)
    {
        Debug.Log(y);

        if (BackGroundPrefabs[0] == null || BackGroundPrefabs[1] == null || BackGroundPrefabs[2] == null)
        {
            Debug.LogError("One of the BackGroundPrefabs is missing!");
        }
        InstantiateBackground(BackGroundPrefabs[0], -1.474f, y + 7.7177f);
        InstantiateBackground(BackGroundPrefabs[1], - 5.55f, y + 7.7177f);
        InstantiateBackground(BackGroundPrefabs[2],   2.6f, y + 7.7177f);
    }

    void InstantiateBackground(GameObject prefab, float x, float y)
    {
        GameObject instance = Instantiate(prefab, transform);
        Background backgroundComponent = instance.GetComponent<Background>();
        backgroundComponent.Initialize(y);
        instance.transform.position = new Vector3(x, y);
    }
}
