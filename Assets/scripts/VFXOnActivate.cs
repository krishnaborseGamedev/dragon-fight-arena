using System.Collections;
using UnityEngine;


public class VFXOnActivate : MonoBehaviour
{
    [SerializeField] private GameObject vfxPrefab;
    [SerializeField] private Transform spawnPoint; // leave empty to spawn at this object's own position
    [SerializeField] private float lifetime = 2f;
    [SerializeField] private float delay = 0f; // wait this many seconds before the effect appears
    [SerializeField] private bool attachToDragon = true; // if true, the effect moves along with the dragon instead of staying in one spot

    public void SpawnEffect()
    {
        if (vfxPrefab == null) return;
        StartCoroutine(SpawnAfterDelay());
    }

    private IEnumerator SpawnAfterDelay()
    {
        if (delay > 0f)
            yield return new WaitForSeconds(delay);

        Transform point = spawnPoint != null ? spawnPoint : transform;
        Transform parent = attachToDragon ? point : null;

        GameObject instance = Instantiate(vfxPrefab, point.position, point.rotation, parent);
        Destroy(instance, lifetime);
    }
}