using UnityEngine;
using UnityEngine.Events;
 
public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;
 
    [Header("Events")]
    public UnityEvent<float, float> OnHealthChanged; // (current, max) — health bar UI listens here
    public UnityEvent OnDeath;                        // Winner Screen listens here
 
    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
    public bool IsDead { get; private set; }
 
    private void Awake()
    {
        currentHealth = maxHealth;
    }
 
    public void TakeDamage(float amount)
    {
        if (IsDead) return;
 
        currentHealth = Mathf.Max(0f, currentHealth - amount);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
 
        if (currentHealth <= 0f)
        {
            IsDead = true;
            OnDeath?.Invoke();
        }
    }
 
    // Handy for the Restart button later
    public void ResetHealth()
    {
        currentHealth = maxHealth;
        IsDead = false;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }
}