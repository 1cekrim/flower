using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class NavMeshAgentCallbacks : MonoBehaviour
{
    public NavMeshAgent agent;
    public UnityEvent CompleteEvent;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void StartMove()
    {
        StartCoroutine(CheckComplete());
    }

    private IEnumerator CheckComplete()
    {
        while (true)
        {
            yield return null;
            if (!agent.pathPending)
            {

                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                    {
                        Debug.Log("Move Complete");
                        CompleteEvent?.Invoke();
                        CompleteEvent?.RemoveAllListeners();
                        yield break;
                    }
                }
            }
        }
    }
}
