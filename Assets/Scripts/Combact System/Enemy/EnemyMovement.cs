using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovement : MonoBehaviour
{
    public Transform Player;
    public float UpdateRate = 0.1f;
    private NavMeshAgent Agent;

    public float DestinationOffset = 5.0f;

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
            // Aggiungi un offset casuale alla destinazione del giocatore
            Vector3 randomOffset = new Vector3(
                Random.Range(-DestinationOffset, DestinationOffset),
                0,
                Random.Range(-DestinationOffset, DestinationOffset)
            );

            Vector3 destination = Player.position + randomOffset;

            Agent.SetDestination(destination);

            yield return Wait;
        }
    }
}
