using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    private GameManager gameManager;

    public GameObject Ground;

    public ObjectToSpawn[] objsToSpawn;

    private void Awake()
    {
        gameManager = GameObject.Find("@GameManager").GetComponent<GameManager>();
    }

    private void Start()
    {
        StartCoroutine(Setup());
    }

    IEnumerator Setup()
    {
        yield return new WaitForEndOfFrame();

        Vector2 halfBoundingObjScale = new Vector2(Ground.transform.localScale.x / 2f, Ground.transform.localScale.z / 2f);

        foreach (ObjectToSpawn objectToSpawn in objsToSpawn)
        {
            GameObject folder = new GameObject();
            folder.name = objectToSpawn.obj.name + "s";

            for (int i = 0; i < objectToSpawn.numToSpawn; i++)
            {
                Vector3 spawnPos = new Vector3
                (
                    Ground.transform.position.x + Random.Range(-halfBoundingObjScale.x, halfBoundingObjScale.x),
                    0,
                    Ground.transform.position.y + Random.Range(-halfBoundingObjScale.y, halfBoundingObjScale.y)
                );
                GameObject temp = Instantiate(objectToSpawn.obj, folder.transform);
                temp.transform.position = spawnPos;
            }
        }
    }
}

[System.Serializable]
public class ObjectToSpawn
{
    public int numToSpawn;
    public GameObject obj;
}
