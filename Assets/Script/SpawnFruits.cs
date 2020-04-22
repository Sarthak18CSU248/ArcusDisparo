using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFruits : MonoBehaviour
{
    [SerializeField]
    private GameObject prefabToSpawn;

    [SerializeField]
    private float spawnInterval, objectMinX, objectMaxX, objectY,objectMinZ,objectMaxZ;

    [SerializeField]
    private Sprite[] objectSprites;

    GameObject newObject;

    // Use this for initialization
    void Start()
    {
        InvokeRepeating("spawnFruits", this.spawnInterval, this.spawnInterval);
        
    }

    private void spawnFruits()
    {
        newObject = Instantiate(this.prefabToSpawn);
        newObject.transform.position = new Vector3(Random.Range(this.objectMinX, this.objectMaxX), this.objectY,Random.Range(this.objectMinZ,this.objectMaxZ));

        Sprite objectSprite = objectSprites[Random.Range(0, this.objectSprites.Length)];
        newObject.GetComponent<SpriteRenderer>().sprite = objectSprite;
        newObject.GetComponent<Transform>().localScale = new Vector3(0.2f, 0.2f, 0.2f);
    }
    
}
