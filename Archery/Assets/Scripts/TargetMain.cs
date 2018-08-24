﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMain : MonoBehaviour {
    [SerializeField] private Transform targetCenter;
    [SerializeField] private float scoreMultiplier = 10;
    [SerializeField] private float scoreRange = 1;
    private bool isScorable = true;
    private GameManager gameManager;
    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        LevelEvents.ContinueToNextLevel += ResetIsScorable; //reset scorable flag every time level resets so that collisionenter will not trigger twice
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!isScorable || gameManager == null)    //ensure addscore only happens once due to collision point bug
            return;
        float addScore = (scoreRange - Vector3.Distance(collision.contacts[0].point, targetCenter.transform.position)) * scoreMultiplier;
        if (Mathf.Round(addScore) > 0) // must add score first before raising event
        {
            gameManager.AddScore((int)Mathf.Round(addScore));
        }
        else
        {
            gameManager.AddScore(0);
        }
        isScorable = false;
    }
    private void ResetIsScorable()
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