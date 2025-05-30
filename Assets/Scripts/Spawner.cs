using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    GameObject ObjectToSpawn;
    [SerializeField]
    Vector3 Range;

    private void Start()
    {
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, Range);
    }

    public void Spawn()
    {
        float spawnX = Random.Range(transform.position.x - Range.x, transform.position.x + Range.x);
        float spawnY = Random.Range(transform.position.y - Range.y, transform.position.y + Range.y);
        float spawnZ = Random.Range(transform.position.z - Range.z, transform.position.z + Range.z);

        Vector3 spawnPos = new Vector3(spawnX, spawnY, spawnZ);

        Instantiate(ObjectToSpawn, spawnPos, gameObject.transform.rotation);
    }
}
