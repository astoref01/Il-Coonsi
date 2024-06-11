using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovement : MonoBehaviour
{
    public Transform Player;
    [SerializeField]
    private Animator Animator;
    public float UpdateRate = 2.5f;
    private NavMeshAgent Agent;
    public float StoppingDistance = 1.0f;
    private const string IsWalking = "IsWalking";
    private const string Jump = "Jump";
    private const string Landed = "Landed";

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        Agent.stoppingDistance = StoppingDistance;
    }

    private void Start()
    {
        StartCoroutine(FollowTarget());
    }

    private void HandleLinkStart()
    {
        //Animator.SetTrigger(Jump);
    }

    private void HandleLinkEnd()
    {
        //Animator.SetTrigger(Landed);
    }

    private void Update()
    {
        Animator.SetBool(IsWalking, Agent.velocity.magnitude > 0.01f);
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
