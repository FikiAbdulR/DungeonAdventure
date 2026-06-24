using System.Collections;
using UnityEngine;

public class simple_player_controller : MonoBehaviour
{
    public static simple_player_controller Instance;

    private UnityEngine.AI.NavMeshAgent agent;
    private Camera mainCamera;

    [SerializeField] private Animator animator;
    [SerializeField] private LayerMask groundLayer;

    public bool CanMove { get; private set; } = true;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float rotationSpeed = 10f;

    private Transform rotateTarget;
    private Vector3 moveDirection;

    private void Awake()
    {
        Instance = this;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        mainCamera = Camera.main;
        agent.updateRotation = false;
    }

    private void Start()
    {
        // Tunda 1 frame supaya semua RoadBlock/NavMeshObstacle sempat di-setup duluan
        StartCoroutine(WarpToSavedPosition());
    }

    private IEnumerator WarpToSavedPosition()
    {
        // Tunggu akhir frame pertama, memastikan semua Awake() script lain (termasuk RoadBlock) sudah jalan
        yield return new WaitForEndOfFrame();

        Vector3 targetPos = data_world_state_manager.Instance.PlayerPosition;

        // Pastikan posisi valid di NavMesh sebelum warp
        if (UnityEngine.AI.NavMesh.SamplePosition(targetPos, out var hit, 5f, UnityEngine.AI.NavMesh.AllAreas))
        {
            agent.Warp(hit.position);
        }
        else
        {
            agent.Warp(targetPos);
            Debug.LogWarning("Posisi tersimpan tidak valid di NavMesh, warp paksa ke posisi asli.");
        }
    }

    private void Update()
    {
        if (CanMove)
        {
            HandleInput();
            HandleMovement();
        }

        if (rotateTarget != null)
        {
            RotateTowardsTarget();
        }

        UpdateAnimation();
    }

    private void HandleInput()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 camForward = mainCamera.transform.forward;
        Vector3 camRight = mainCamera.transform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        moveDirection = (camForward * v + camRight * h);

        if (moveDirection.sqrMagnitude > 1f)
            moveDirection.Normalize();
    }

    private void HandleMovement()
    {
        if (moveDirection.sqrMagnitude > 0.0001f)
        {
            agent.Move(moveDirection * moveSpeed * Time.deltaTime);

            rotateTarget = null;
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }

    public void EnableMovement()
    {
        agent.Warp(transform.position);
        agent.ResetPath();
        CanMove = true;
    }

    public void DisableMovement()
    {
        CanMove = false;
        agent.ResetPath();
        moveDirection = Vector3.zero;
    }

    public void RotateToFace(Transform target)
    {
        rotateTarget = target;
    }

    public void StopRotating()
    {
        rotateTarget = null;
    }

    private void RotateTowardsTarget()
    {
        Vector3 direction = rotateTarget.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.0001f)
        {
            rotateTarget = null;
            return;
        }

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );

        if (Quaternion.Angle(transform.rotation, targetRotation) < 1f)
        {
            rotateTarget = null;
        }
    }

    private void UpdateAnimation()
    {
        float speed = moveDirection.sqrMagnitude > 0.0001f ? moveSpeed : 0f;
        animator.SetFloat("Speed", speed, 0.1f, Time.deltaTime);
    }
}