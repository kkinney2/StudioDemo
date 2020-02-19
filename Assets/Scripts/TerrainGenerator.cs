using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TerrainGenerator : MonoBehaviour
{
    private GameManager gameManager;

    public GameObject Ground;

    public ObjectToSpawn[] objsToSpawn;

    [Space(10)]

    public bool generateTerrain = false;

    private void Awake()
    {
        gameManager = GameObject.Find("@GameManager").GetComponent<GameManager>();
    }

    private void Start()
    {
        //StartCoroutine(Setup());
    }

    private void Update()
    {
        if (generateTerrain)
        {
            //StartCoroutine(Setup());
            Setup();
            generateTerrain = false;
        }
    }

    private void Setup()
    {
        Vector2 halfBoundingObjScale = new Vector2(Ground.transform.localScale.x / 2f, Ground.transform.localScale.z / 2f);

        foreach (ObjectToSpawn objectToSpawn in objsToSpawn)
        {
            if (objectToSpawn.folder != null)
            {
                objectToSpawn.folder.name += "_DELETE";
                objectToSpawn.folder.SetActive(false);
            }


            objectToSpawn.folder = new GameObject();
            objectToSpawn.folder.name = objectToSpawn.obj.name + "s";

            for (int i = 0; i < objectToSpawn.numToSpawn; i++)
            {
                Vector3 spawnPos = new Vector3
                (
                    Ground.transform.position.x + Random.Range(-halfBoundingObjScale.x+1, halfBoundingObjScale.x-1),
                    0,
                    Ground.transform.position.y + Random.Range(-halfBoundingObjScale.y+1, halfBoundingObjScale.y-1)
                );
                GameObject temp = Instantiate(objectToSpawn.obj, objectToSpawn.folder.transform);
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
    public GameObject folder;
}
