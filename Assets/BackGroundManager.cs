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
        InstantiateBackground(BackGroundPrefabs[0],      0, y + 10f);
        InstantiateBackground(BackGroundPrefabs[1], - 4.8f, y + 10f);
        InstantiateBackground(BackGroundPrefabs[2],   4.8f, y + 10f);
    }

    void InstantiateBackground(GameObject prefab, float x, float y)
    {
        GameObject instance = Instantiate(prefab, transform);
        Background backgroundComponent = instance.GetComponent<Background>();
        backgroundComponent.Initialize(y);
        instance.transform.position = new Vector3(x, y);
    }
}
