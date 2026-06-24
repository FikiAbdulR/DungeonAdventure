using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Patrol,
    Chase,
    Return
}

public class enemy_ai : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private Transform player;

    [Header("Patrol Settings")]
    [SerializeField] private float patrolRadius = 8f;
    [SerializeField] private float patrolWaitTime = 2f;
    [SerializeField] private float patrolSpeed = 2f;

    [Header("Chase Settings")]
    [SerializeField] private float chaseRadius = 6f;
    [SerializeField] private float chaseSpeed = 4f;
    [SerializeField] private float loseChaseDistance = 9f; // jarak supaya enemy berhenti ngejar

    [Header("Gizmo Colors")]
    [SerializeField] private Color patrolColor = Color.green;
    [SerializeField] private Color chaseColor = Color.red;

    private NavMeshAgent agent;
    private EnemyState currentState = EnemyState.Patrol;

    private Vector3 spawnPosition;
    private Vector3 currentPatrolTarget;
    private float waitTimer = 0f;
    private bool isWaiting = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        spawnPosition = transform.position;
    }

    private void Start()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
            else
                Debug.LogWarning("Player dengan tag 'Player' tidak ditemukan di scene!");
        }

        SetNewPatrolTarget();
    }

    private void Update()
    {
        switch (currentState)
        {
            case EnemyState.Patrol:
                HandlePatrol();
                CheckForPlayer();
                break;

            case EnemyState.Chase:
                HandleChase();
                break;

            case EnemyState.Return:
                HandleReturn();
                break;
        }

        UpdateAnimation();
    }

    private void HandlePatrol()
    {
        agent.speed = patrolSpeed;

        if (isWaiting)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= patrolWaitTime)
            {
                isWaiting = false;
                SetNewPatrolTarget();
            }
            return;
        }

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            isWaiting = true;
            waitTimer = 0f;
        }
    }

    private void SetNewPatrolTarget()
    {
        Vector3 randomPoint = spawnPosition + Random.insideUnitSphere * patrolRadius;
        randomPoint.y = spawnPosition.y;

        if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, patrolRadius, NavMesh.AllAreas))
        {
            currentPatrolTarget = hit.position;
            agent.SetDestination(currentPatrolTarget);
        }
    }


    private void CheckForPlayer()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= chaseRadius)
        {
            currentState = EnemyState.Chase;
            isWaiting = false;
        }
    }

    private void HandleChase()
    {
        if (player == null)
        {
            currentState = EnemyState.Return;
            return;
        }

        agent.speed = chaseSpeed;
        agent.SetDestination(player.position);

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance >= loseChaseDistance)
        {
            currentState = EnemyState.Return;
        }

    }


    private void HandleReturn()
    {
        agent.speed = patrolSpeed;
        agent.SetDestination(spawnPosition);

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            currentState = EnemyState.Patrol;
            SetNewPatrolTarget();
        }

        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            if (distance <= chaseRadius)
            {
                currentState = EnemyState.Chase;
            }
        }
    }

    private void UpdateAnimation()
    {
        if (animator != null)
            animator.SetFloat("Speed", agent.velocity.magnitude, 0.1f, Time.deltaTime);
    }


    private void OnDrawGizmosSelected()
    {
        Vector3 origin = Application.isPlaying ? spawnPosition : transform.position;

        Gizmos.color = patrolColor;
        Gizmos.DrawWireSphere(origin, patrolRadius);

        Gizmos.color = chaseColor;
        Gizmos.DrawWireSphere(transform.position, chaseRadius);
    }
}