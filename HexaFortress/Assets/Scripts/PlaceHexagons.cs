using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlaceHexagons : MonoBehaviour
{
    [SerializeField] private GameObject[] tiles;
    [SerializeField] private int size;

    private void Start()
    {
        SpawnTile();
    }
    [Button("Spawn Tile")]
    private void SpawnTile()
    {
        int tileCount = 1;
        float tileOffset = 0.86f;
        float xOffset = 0;
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                var tile = Instantiate(tiles[Random.Range(0, tiles.Length)]);
                float x = (tileOffset + tileOffset) * j;
                x += xOffset;
                float z = i * 0.5f;
                tile.transform.position = new Vector3(x, 0, z);
                tile.transform.rotation = Quaternion.Euler(0, -90, 0);
                tile.name = tileCount.ToString();
                tileCount++;
            }
            xOffset = xOffset == 0 ? tileOffset : 0;
        }
    }
    [Button("Remove parent")]
    private void RemoveParent()
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            foreach (Transform item in tiles[i].transform)
            {
                item.transform.parent = null;
            }
        }
    }
}

