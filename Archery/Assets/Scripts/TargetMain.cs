using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMain : MonoBehaviour {
    public Transform targetCenter;
    [SerializeField] float scoreMultiplier = 10;
    [SerializeField] float scoreRange = 1;
    bool isScorable = true;
    GameManager gameManager;
    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        LevelEvents.ContinueToNextLevel += ResetIsScorable; //reset scorable flag every time level resets so that collisionenter will not trigger twice
    }
    float CheckDistanceFromCenter(Vector3 arrowCollision)
    {
        return Vector3.Distance(targetCenter.position, arrowCollision);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!isScorable)
            return;
        float addScore = (scoreRange - Vector3.Distance(collision.contacts[0].point, targetCenter.transform.position)) * scoreMultiplier;
        if (Mathf.Round(addScore) >= 0)
            gameManager.GameScore += Mathf.Round(addScore);
        isScorable = false;
    }
    public void ResetIsScorable()
    {
        isScorable = true;
    }
    private void OnDestroy() // avoid memory leaks
    {
        LevelEvents.ContinueToNextLevel -= ResetIsScorable;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(targetCenter.transform.position, scoreRange);
    }
}
