using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    [SerializeField] GameObject[] FloorPrefabs;
    
    public void SpawnFloor(Vector3 position)
    {
        // TODO FLOOR update with backgound
        int r = Random.Range(0, FloorPrefabs.Length);
        GameObject floor = Instantiate(FloorPrefabs[r], transform);
        floor.transform.position = new Vector3(Random.Range( -4.866793f, 4.829999f),  position.y + 9.926872f);
    }

}