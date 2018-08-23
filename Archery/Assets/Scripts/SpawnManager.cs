using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {
    const float RAY_LENGTH = 1000f;
    [SerializeField] Vector3 spawnAreaSize;
    [SerializeField] GameObject spawnObject;
    GameObject currentObjectInstance;
	void Start () {
        LevelEvents.ContinueToNextLevel += SpawnObject;
        SpawnObject();
	}
    public void SpawnObject()
    {
        if (currentObjectInstance != null)
            Destroy(currentObjectInstance);
        RaycastHit hitInfo = GetRandomSpawnHitRayPosition();
        GameObject spawnObjectInstance = Instantiate(spawnObject, hitInfo.point, Quaternion.identity);
        spawnObjectInstance.transform.LookAt(GameObject.FindGameObjectWithTag("Player").transform);
        currentObjectInstance = spawnObjectInstance;
    }
    RaycastHit GetRandomSpawnHitRayPosition()
    {
        Vector3 generatedRayPosition = transform.position + new Vector3(Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2), Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2), Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2));
        RaycastHit hit;
        Physics.Raycast(generatedRayPosition, -Vector3.up, out hit, Mathf.Infinity);
        return hit;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position, spawnAreaSize);
    }
    private void OnDestroy()
    {
        LevelEvents.ContinueToNextLevel -= SpawnObject;
    }
}
