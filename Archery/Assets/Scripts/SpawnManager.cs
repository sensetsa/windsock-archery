using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {
    [SerializeField] private Vector3 spawnAreaSize;
    [SerializeField] private GameObject spawnObject;
    public GameObject currentObjectInstance;
	private void Start () {
        LevelEvents.ContinueToNextLevel += SpawnObject;
        SpawnObject();
	}
    private void SpawnObject()
    {
        if (currentObjectInstance != null)
            Destroy(currentObjectInstance);
        RaycastHit hitInfo = GetRandomSpawnHitRayPosition();
        currentObjectInstance = Instantiate(spawnObject, hitInfo.point, Quaternion.identity);
        currentObjectInstance.transform.LookAt(GameObject.FindGameObjectWithTag("Player").transform);
        LevelEvents.RaiseLevelEvent(LevelEvents.LevelEventType.SpawnTarget);
    }
    private RaycastHit GetRandomSpawnHitRayPosition()
    {
        Vector3 generatedRayPosition = transform.position + new Vector3(Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2), Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2), Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2));
        RaycastHit hit;
        Physics.Raycast(generatedRayPosition, -Vector3.up, out hit, Mathf.Infinity);
        return hit;
    }
    private void OnDestroy()
    {
        LevelEvents.ContinueToNextLevel -= SpawnObject;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position, spawnAreaSize);
    }
}