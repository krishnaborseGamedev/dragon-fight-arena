using UnityEngine;
 

public class TailAbility : Ability
{
    [Header("Tail Settings")]
    [SerializeField] private Transform tailPoint;
    [SerializeField] private float radius = 1.5f;
 
    protected override void Execute()
    {
        Transform origin = tailPoint != null ? tailPoint : transform;
        Collider[] hits = Physics.OverlapSphere(origin.position, radius);
 
        foreach (Collider hit in hits)
        {
            Health targetHealth = hit.GetComponentInParent<Health>();
            if (targetHealth != null && targetHealth.gameObject != gameObject)
            {
                targetHealth.TakeDamage(damage);
                break; // one swing hits one target
            }
        }
    }
 
    // Draws a yellow sphere in the Scene view when this object is selected,
    // so you can visually confirm the hit range without guessing
    private void OnDrawGizmosSelected()
    {
        Transform origin = tailPoint != null ? tailPoint : transform;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(origin.position, radius);
    }
}