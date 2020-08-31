using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    FOVDetection fov;
    Transform player;
    NavMeshAgent agent;
    private void Start()
    {
        fov = GetComponent<FOVDetection>();
        player = fov.player;
        agent = GetComponent<NavMeshAgent>();
    }
    void Update()
    {
        if(fov.isInFov)
        agent.SetDestination(player.position);
    }
}
