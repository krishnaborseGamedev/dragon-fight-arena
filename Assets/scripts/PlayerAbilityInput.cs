using UnityEngine;
 
/// <summary>
/// Maps number keys to the player's abilities: 1 = Fire, 2 = Tail, 3 = Fly.
/// </summary>
public class PlayerAbilityInput : MonoBehaviour
{
    [SerializeField] private FireAbility fireAbility;
    [SerializeField] private TailAbility tailAbility;
    [SerializeField] private FlyAbility flyAbility;
 
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            fireAbility.TryActivate();
        }
 
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            tailAbility.TryActivate();
        }
 
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            flyAbility.TryActivate();
        }
    }
}