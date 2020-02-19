using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceType
{
    Wood, Rock
}

public class Resource : Interactable
{
    public ResourceType resourceType;

    Coroutine markedForDestory;

    public GameObject resourcePrefab;
    private int resourceValue = 1;

    // Start is called before the first frame update
    void Start()
    {
        resourceValue = Random.Range(2, 5);
    }


    public override void Interact(Kneeman kneeman)
    {
        if (markedForDestory == null)
        {
            base.Interact(kneeman);

            RemoveFromResource();
            switch (resourceType)
            {
                case ResourceType.Wood:
                    kneeman.owner.wood += 1;
                    break;
                case ResourceType.Rock:
                    kneeman.owner.stone += 1;
                    break;
                default:
                    break;
            }
        }
    }

    private void RemoveFromResource()
    {
        resourceValue--;
        if (resourceValue <= 0)
        {
            markedForDestory = StartCoroutine(OnEmpty());
        }
    }

    public ResourceType GetResourceType()
    {
        return resourceType;
    }

    IEnumerator OnEmpty() // TODO: Insert animation instead of dropping out of existence
    {
        gameObject.GetComponent<Collider>().enabled = false;

        yield return new WaitForSeconds(5f);

        Destroy(gameObject);
    }
}
