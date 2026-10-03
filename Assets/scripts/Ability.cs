using UnityEngine;
using UnityEngine.Events;
 

public abstract class Ability : MonoBehaviour
{
    [Header("Ability Settings")]
    [SerializeField] protected float damage = 15f;
    [SerializeField] protected float cooldown = 3f;
    [SerializeField] protected float actionDuration = 0.6f; // roughly the animation's length — blocks other abilities while it plays
 
    private float cooldownTimer = 0f;
    private AbilityLock abilityLock;
 
    [Header("Events (hook animation/VFX/SFX here)")]
    public UnityEvent OnActivated;
 
    public bool IsReady => cooldownTimer <= 0f && (abilityLock == null || !abilityLock.IsBusy);
    public float CooldownRemaining01 => cooldown <= 0f ? 0f : Mathf.Clamp01(cooldownTimer / cooldown);
 
    protected virtual void Awake()
    {
        abilityLock = GetComponent<AbilityLock>();
    }
 
    protected virtual void Update()
    {
        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;
    }
 
    public bool TryActivate()
    {
        if (!IsReady) return false;
 
        cooldownTimer = cooldown;
        if (abilityLock != null) abilityLock.Engage(actionDuration);
 
        OnActivated?.Invoke();
        Execute();
        return true;
    }
 
    // Each ability implements its own effect
    protected abstract void Execute();
}