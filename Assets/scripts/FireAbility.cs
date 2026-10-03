using UnityEngine;
 
/// <summary>
/// Ranged fire breath — raycasts forward from FirePoint and damages
/// the first Health component it hits (that isn't this dragon).
/// </summary>
public class FireAbility : Ability
{
    [Header("Fire Settings")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private float range = 8f;
 
    protected override void Execute()
    {
        Transform origin = firePoint != null ? firePoint : transform;
 
        if (Physics.Raycast(origin.position, origin.forward, out RaycastHit hit, range))
        {
            Health targetHealth = hit.collider.GetComponentInParent<Health>();
            if (targetHealth != null && targetHealth.gameObject != gameObject)
            {
                targetHealth.TakeDamage(damage);
            }
        }
 
        // Visible in Scene view during Play mode so you can confirm aim/range
        Debug.DrawRay(origin.position, origin.forward * range, Color.red, 0.5f);
    }
}