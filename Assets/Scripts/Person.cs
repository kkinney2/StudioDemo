using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Person : Interactable
{
    public KneemanManager owner;

    public NavMeshAgent agent;

    public string myName = "Pawn";
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
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f); // TODO: This is only for temp while no model difference

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

        // Delay/ProgressBar
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

        Interact();

        EndTask();

        yield return null;
    }

    private void Interact()
    {
        if (interactable)
        {
            interactable.Interact(this);
        }

    }

    public override void Interact(Person person)
    {
        base.Interact(person);

        if (person == interactable) // If they are both searching for each other...
        {
            Debug.Log("Matched!");
            person.EndTask();
            EndTask();
            owner.Mating(gameObject.transform, person.transform);
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
