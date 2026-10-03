using UnityEngine;

/// <summary>
/// Controls the AI dragon's behavior: Idle -> Chase -> Attack, based on
/// distance to the player. Uses the SAME FireAbility/TailAbility scripts
/// and cooldowns as the player — just triggered by code instead of key presses.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class EnemyAI : MonoBehaviour
{
    private enum State { Idle, Chase, Attack }

    [Header("Who to fight")]
    [SerializeField] private Transform player; // drag the PlayerDragon object here in the Inspector

    [Header("Abilities (drag from this same object)")]
    [SerializeField] private FireAbility fireAbility;
    [SerializeField] private TailAbility tailAbility;
    [SerializeField] private FlyAbility flyAbility;

    [Header("Distance Settings")]
    [SerializeField] private float detectionRange = 12f;    // starts chasing when player is this close
    [SerializeField] private float attackRange = 10f;        // stops walking and starts deciding an attack at this distance (matches the farthest attack range below)
    [SerializeField] private float tailUseDistance = 3f;     // close range -> Tail
    [SerializeField] private float flyUseDistance = 7f;      // medium range -> Fly
    [SerializeField] private float fireUseDistance = 10f;    // long range -> Fire

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float rotationSpeed = 8f;

    private State currentState = State.Idle;
    private Rigidbody rb;
    private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Decide which state we should be in
        switch (currentState)
        {
            case State.Idle:
                if (distanceToPlayer <= detectionRange)
                    currentState = State.Chase;
                break;

            case State.Chase:
                if (distanceToPlayer > detectionRange * 1.2f)
                    currentState = State.Idle;
                else if (distanceToPlayer <= attackRange)
                    currentState = State.Attack;
                break;

            case State.Attack:
                if (distanceToPlayer > attackRange * 1.3f)
                    currentState = State.Chase;
                else
                    TryAttack(distanceToPlayer);
                break;
        }

        // Update the walk animation based on whether we're currently chasing
        if (animator != null)
        {
            float speed = (currentState == State.Chase) ? 1f : 0f;
            animator.SetFloat("Speed", speed);
        }
    }

    private void FixedUpdate()
    {
        if (player == null || currentState != State.Chase)
        {
            // Not chasing — stop horizontal movement, keep gravity working
            Vector3 stopped = rb.linearVelocity;
            stopped.x = 0f;
            stopped.z = 0f;
            rb.linearVelocity = stopped;
            return;
        }

        // Walk toward the player
        Vector3 direction = player.position - transform.position;
        direction.y = 0f;
        direction = direction.normalized;

        Vector3 targetVelocity = direction * moveSpeed;
        targetVelocity.y = rb.linearVelocity.y;
        rb.linearVelocity = targetVelocity;

        FaceDirection(direction);
    }

    private void TryAttack(float distance)
    {
        // Keep facing the player while attacking
        Vector3 direction = player.position - transform.position;
        direction.y = 0f;
        FaceDirection(direction.normalized);

        if (distance <= tailUseDistance)
        {
            tailAbility.TryActivate();
        }
        else if (distance <= flyUseDistance)
        {
            flyAbility.TryActivate();
        }
        else if (distance <= fireUseDistance)
        {
            fireAbility.TryActivate();
        }
    }

    private void FaceDirection(Vector3 direction)
    {
        if (direction.sqrMagnitude < 0.001f) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));
    }
}