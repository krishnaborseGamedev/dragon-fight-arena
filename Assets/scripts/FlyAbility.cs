using System.Collections;
using UnityEngine;

/// <summary>
/// Dragon takes off, hovers briefly, then lands and deals area damage
/// around the landing point (like a slam attack).
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class FlyAbility : Ability
{
    [Header("Fly Settings")]
    [SerializeField] private float flyHeight = 4f;
    [SerializeField] private float ascendDuration = 0.4f;
    [SerializeField] private float hoverDuration = 0.3f;
    [SerializeField] private float descendDuration = 0.4f;
    [SerializeField] private float impactRadius = 2.5f;
    [SerializeField] private GameObject landingVFXPrefab;
    [SerializeField] private float landingVFXLifetime = 2f;

    private Rigidbody rb;
    private Animator animator; // model is a child object
    private PlayerMovement playerMovement; // null for AI dragon — that's fine

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        actionDuration = ascendDuration + hoverDuration + descendDuration; // lock until the whole flight finishes
    }

    protected override void Execute()
    {
        StartCoroutine(FlySequence());
    }

    private IEnumerator FlySequence()
    {
        // Prevent WASD input and gravity from fighting our manual movement mid-flight
        if (playerMovement != null) playerMovement.enabled = false;
        rb.useGravity = false;

        Vector3 groundPosition = transform.position;
        Vector3 upPosition = groundPosition + Vector3.up * flyHeight;

        if (animator != null) animator.CrossFade("Take Off", 0.15f);
        yield return MoveOverTime(groundPosition, upPosition, ascendDuration);

        if (animator != null) animator.CrossFade("Fly Float", 0.15f);
        yield return new WaitForSeconds(hoverDuration);

        if (animator != null) animator.CrossFade("Land", 0.15f);
        yield return MoveOverTime(upPosition, groundPosition, descendDuration);

        rb.useGravity = true;
        if (playerMovement != null) playerMovement.enabled = true;

        DealImpactDamage();
    }

    private IEnumerator MoveOverTime(Vector3 from, Vector3 to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = to;
    }

    private void DealImpactDamage()
    {
        if (landingVFXPrefab != null)
        {
            GameObject vfx = Instantiate(landingVFXPrefab, transform.position, Quaternion.identity);
            Destroy(vfx, landingVFXLifetime);
        }

        Collider[] hits = Physics.OverlapSphere(transform.position, impactRadius);
        foreach (Collider hit in hits)
        {
            Health targetHealth = hit.GetComponentInParent<Health>();
            if (targetHealth != null && targetHealth.gameObject != gameObject)
            {
                targetHealth.TakeDamage(damage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, impactRadius);
    }
}