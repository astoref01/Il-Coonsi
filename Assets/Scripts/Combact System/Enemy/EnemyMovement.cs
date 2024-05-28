using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovement : MonoBehaviour
{
    public Transform Player;
    public float UpdateRate = 0.1f;
    public float StoppingDistance = 9.0f; // Distanza desiderata da mantenere dal giocatore
    private NavMeshAgent Agent;

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        Agent.stoppingDistance = StoppingDistance; // Imposta la distanza di arresto del NavMeshAgent
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
            Vector3 targetPosition = Player.position - directionToPlayer.normalized * StoppingDistance;

            if (directionToPlayer.magnitude > StoppingDistance)
            {
                Agent.SetDestination(targetPosition);
            }
            yield return Wait;
        }
    }
}
