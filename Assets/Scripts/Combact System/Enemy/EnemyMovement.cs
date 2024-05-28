using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovement : MonoBehaviour
{
    public Transform Player;
    public float UpdateRate = 0.1f;
    public float StoppingDistance = 2.0f; // Distance from the player to stop

    private NavMeshAgent Agent;

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        StartCoroutine(FollowTarget());
    }

    private IEnumerator FollowTarget()
    {
        WaitForSeconds Wait = new WaitForSeconds(UpdateRate);

        while (gameObject.activeSelf)
        {
            Vector3 directionToPlayer = Player.position - transform.position;
            Vector3 destination = Player.position - directionToPlayer.normalized * StoppingDistance;

            Agent.SetDestination(destination);

            yield return Wait;
        }
    }
}
