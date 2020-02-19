using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KneemanManager : MonoBehaviour
{
    public GameObject Prefab_Kneeman;
    private GameObject kneemen;

    public List<Kneeman> kneemenList;

    public GameObject target;

    public List<Resource> trees;

    public int wood = 0;
    public int stone = 0;

    public Text Text_Wood;  // TODO: Delegate for UI
    public Text Text_Stone; // TODO: Rock or Stone

    // Start is called before the first frame update
    void Start()
    {
        trees = new List<Resource>();

        kneemenList = new List<Kneeman>();
        kneemen = new GameObject();
        kneemen.name = "Kneemen";

        StartCoroutine(Jobs());
    }

    // Update is called once per frame
    void Update()
    {
        Text_Wood.text = wood + "";
        Text_Stone.text = stone + "";
    }

    public void SpawnKneeman(JobType jobType)
    {
        kneemenList.Add(Instantiate(Prefab_Kneeman, kneemen.transform).GetComponent<Kneeman>());
        kneemenList[kneemenList.Count - 1].myJob = jobType;
        kneemenList[kneemenList.Count - 1].owner = this;
    }

    public void SpawnKneeman(JobType jobType, Vector3 a_Pos)
    {
        kneemenList.Add(Instantiate(Prefab_Kneeman, kneemen.transform).GetComponent<Kneeman>());
        kneemenList[kneemenList.Count - 1].agent.Warp(a_Pos);
        kneemenList[kneemenList.Count - 1].myJob = jobType;
        kneemenList[kneemenList.Count - 1].owner = this;
    }

    public void SpawnKneeman_Idle()
    {
        SpawnKneeman(JobType.Idle);
    }

    public void SpawnKneeman_Mating()
    {
        SpawnKneeman(JobType.Mating);
    }

    public void SpawnKneeman_Lumberjack()
    {
        SpawnKneeman(JobType.Lumberjack);
    }

    public void SpawnKneeman_Mason()
    {
        SpawnKneeman(JobType.Mason);
    }

    public void Mating(Transform kneeman1, Transform kneeman2)
    {
        Vector3 newPos = (kneeman1.position + kneeman2.position) / 2f;
        SpawnKneeman(JobType.Child, newPos);
    }

    IEnumerator Jobs()
    {
        while (true)
        {

            for (int i = 0; i < kneemenList.Count; i++)
            {

                if (kneemenList[i].hasTask)
                    continue;

                switch (kneemenList[i].myJob)
                {
                    case JobType.Child:
                        break;
                    case JobType.Idle:
                        // TODO: If idle too long, switch to 'Mating'
                        break;
                    case JobType.Mating:
                        Task(kneemenList[i], "Kneeman", true);
                        break;
                    case JobType.Lumberjack:
                        Task(kneemenList[i], "Tree", true);
                        break;
                    case JobType.Mason:
                        Task(kneemenList[i], "Rock", true);
                        break;
                    case JobType.Farmer:
                        break;
                    default:
                        break;
                }
            }

            yield return new WaitForEndOfFrame();
            //yield return new WaitForSeconds(1f);
        }
    }

    private void Task(Kneeman kneeman, string target_Str, bool shouldInteract)
    {
        Interactable tempTarget = FindClosestObj(kneeman.gameObject.transform, target_Str, 500);
        //Debug.Log("Interactable Found: " + tempTarget);
        if (tempTarget != null)
        {
            target = tempTarget.gameObject;
            Vector3 destination = tempTarget.interactionTargets.GetClosestTarget(kneeman.transform).position;
            if (destination != kneeman.agent.destination || !kneeman.agent.isStopped)
            {
                kneeman.agent.SetDestination(destination);
                kneeman.interactable = tempTarget;
                kneeman.shouldInteract = shouldInteract;
                kneeman.hasTask = true;
            }
        }
        //else
        //Debug.Log("No Target Found");
    }

    #region Find
    public Collider[] Find(Transform kneeman, string mask_Str, float radius)
    {
        int oldLayer = kneeman.gameObject.layer;
        kneeman.gameObject.layer = 2;

        LayerMask mask = LayerMask.GetMask(mask_Str);
        Collider[] hitColliders = Physics.OverlapSphere(kneeman.position, radius, mask, QueryTriggerInteraction.Collide);

        kneeman.gameObject.layer = oldLayer;

        if (hitColliders.Length != 0)
        {
            return hitColliders;
        }
        else
            return null; // TODO: Should this become recursive and increase radius until it finds something?
    }

    public Interactable GetClosestObj(Transform kneeman, Collider[] possibleObjs)
    {
        Interactable closestObj = null;
        float minDist = float.MaxValue;
        Vector3 currentPos = kneeman.position;

        if (possibleObjs != null)
        {
            foreach (Collider c in possibleObjs)
            {
                if (c.gameObject == gameObject)
                    continue;

                Interactable an_Interactable = c.gameObject.GetComponent<Interactable>();

                if (an_Interactable != null)
                {
                    float dist = Mathf.Abs(Vector3.Distance(c.gameObject.transform.position, currentPos));

                    if (dist < minDist)
                    {
                        closestObj = an_Interactable;
                        minDist = dist;
                    }

                }

            }
            return closestObj;
        }
        else
            return null;
    }

    public Interactable FindClosestObj(Transform kneeman, string mask_Str, float radius)
    {
        // Pick out the closest one
        Interactable closestObj = GetClosestObj(kneeman, Find(kneeman, mask_Str, radius));

        if (closestObj != null)
        {
            //Debug.Log(closestObj);
            return closestObj;
        }
        else
        {
            //Debug.Log("Was unable to find closest obj");
            return null;
        }
    }
    #endregion

    IEnumerator Tracking()
    {
        while (true)
        {
            for (int i = 0; i < kneemenList.Count; i++)
            {
                kneemenList[i].agent.SetDestination(target.transform.position);
            }
            yield return new WaitForEndOfFrame();
            //yield return new WaitForSeconds(1f);
        }
    }
}

public enum JobType
{
    Child,
    Mating,
    Idle,
    Lumberjack,
    Mason,
    Farmer
}
