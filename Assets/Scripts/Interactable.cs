using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float radius = 3f;
    public DestinationTargets interactionTargets;

    public float interactDelay = 1f;

    public virtual void Interact(Person person)
    {
        // THis method is meant to be overwritten
    }
}

[System.Serializable]
public class DestinationTargets
{
    public Transform[] interactableTargets;

    public Transform GetClosestTarget(Transform a_Transform)
    {
        float minDist = float.MaxValue;
        Transform closestTrans = null;

        Vector3 currentPos = a_Transform.position;

        for (int i = 0; i < interactableTargets.Length; i++)
        {
            float dist = Vector3.Distance(interactableTargets[i].position, currentPos);

            if (dist < minDist)
            {
                minDist = dist;
                closestTrans = interactableTargets[i];
            }
        }

        return closestTrans;
    }
}
