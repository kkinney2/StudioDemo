using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Kneeman : Interactable
{
    public KneemanManager owner;

    public NavMeshAgent agent;

    public string myName = "Kneeman";
    public JobType myJob;

    public bool hasTask = false;

    public bool shouldInteract = false;
    public Interactable interactable;

    public Slider progressBar;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // TODO: randomize 'stats', but also create inheritance?
        interactDelay = Random.Range(1, 10); // Where 1 gives a short period to match, and 10 gives a long period to match
    }

    // Update is called once per frame
    void Update()
    {
        if (myJob == JobType.Child)
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        
        if (agent.remainingDistance < 0.5f && agent.hasPath)
        {
            agent.destination = transform.position;

            if (shouldInteract)
            {
                shouldInteract = false;
                StartCoroutine(InteractIn(interactable.interactDelay));
            }
        }
    }

    IEnumerator InteractIn(float value)
    {
        //yield return new WaitForSeconds(value);
        float timePassed = 0f;

        progressBar.maxValue = value;

        while (timePassed < value)
        {
            if (interactable == null)
            {
                EndTask();
            }

            timePassed += Time.deltaTime;
            progressBar.value = timePassed;
            yield return new WaitForEndOfFrame();
        }

        interactable.Interact(this);


        EndTask();

        yield return null;
    }

    public override void Interact(Kneeman kneeman)
    {
        base.Interact(kneeman);

        if (kneeman == interactable) // If they are both searching for each other...
        {
            Debug.Log("Matched!");
            kneeman.EndTask();
            EndTask();
            owner.Mating(gameObject.transform, kneeman.transform);
        }
    }

    public void ResetProgressBar()
    {
        progressBar.value = 0;
        progressBar.maxValue = 1;
    }

    public void EndTask()
    {
        agent.ResetPath();
        interactable = null;
        ResetProgressBar();
        StopAllCoroutines();
        hasTask = false;
    }
}
