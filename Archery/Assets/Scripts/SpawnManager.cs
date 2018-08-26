using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {
    [SerializeField] private Vector3 spawnAreaSize;
    [SerializeField] private GameObject spawnObject;
    [HideInInspector] public GameObject currentObjectInstance;

    [SerializeField] private GameObject targetDistanceOverlayUIPrefab;
    private GameObject currentTargetDistanceOverlayUI;

    
	private void Start () {
        LevelEvents.ContinueToNextLevel += SpawnObject;
        BowEvents.ShootArrow += DestroySpawnTargetDistanceOverlayUI;
        SpawnObject();
	}
    private float GetTargetDistanceToPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        return Vector3.Distance(currentObjectInstance.transform.position, player.transform.position);
    }
    private void SpawnTargetDistanceOverlayUI()
    {
        Camera camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        GameObject targetDistanceOverlayUIInstance = Instantiate(targetDistanceOverlayUIPrefab, currentObjectInstance.transform);
        TargetDistanceOverlayUI targetDistanceOverlayUI = targetDistanceOverlayUIInstance.GetComponent<TargetDistanceOverlayUI>();
        targetDistanceOverlayUI.targetDistanceFromPlayer = GetTargetDistanceToPlayer();
        targetDistanceOverlayUI.targetDistanceTextObject.transform.position = camera.WorldToScreenPoint(currentObjectInstance.transform.position);
        currentTargetDistanceOverlayUI = targetDistanceOverlayUIInstance;
    }
    private void DestroySpawnTargetDistanceOverlayUI()
    {
        Destroy(currentTargetDistanceOverlayUI);
        currentTargetDistanceOverlayUI = null;
    }
    private void SpawnObject()
    {
        if (currentObjectInstance != null)
            Destroy(currentObjectInstance);
        RaycastHit hitInfo = GetRandomSpawnHitRayPosition();
        currentObjectInstance = Instantiate(spawnObject, hitInfo.point, Quaternion.identity);
        currentObjectInstance.transform.LookAt(GameObject.FindGameObjectWithTag("Player").transform);
        SpawnTargetDistanceOverlayUI();
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
        BowEvents.ShootArrow -= DestroySpawnTargetDistanceOverlayUI;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position, spawnAreaSize);
    }
}