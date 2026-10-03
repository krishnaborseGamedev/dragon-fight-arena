using System.Collections;
using UnityEngine;
 

public class AbilityLock : MonoBehaviour
{
    public bool IsBusy { get; private set; }
 
    public void Engage(float duration)
    {
        StopAllCoroutines();
        StartCoroutine(BusyTimer(duration));
    }
 
    private IEnumerator BusyTimer(float duration)
    {
        IsBusy = true;
        yield return new WaitForSeconds(duration);
        IsBusy = false;
    }
}