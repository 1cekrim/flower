using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DrawNavMesh : MonoBehaviour
{
    public NavMeshAgent agent;
    private LineRenderer lineRenderer;
    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }
    private void Update()
    {   
        if (agent.path.corners != null && agent.path.corners.Length > 0)
        {
            lineRenderer.positionCount = agent.path.corners.Length;
            for (int i = 0; i < agent.path.corners.Length; ++i)
            {
                lineRenderer.SetPosition(i, agent.path.corners[i]);
            }
        }
    }
}
