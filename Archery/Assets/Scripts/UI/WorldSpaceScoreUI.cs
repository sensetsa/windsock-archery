using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class WorldSpaceScoreUI : MonoBehaviour {
    [SerializeField] private float secondsAlive = 2f;
    [SerializeField] private float floatUpDistance = 1f;
    [SerializeField] private TextMeshProUGUI scoreText;
    public string ScoreText { set { scoreText.text = value; } }
	// Use this for initialization
	void Start () {
        StartCoroutine(DestroyAfterSeconds());
	}
	
	// Update is called once per frame
	void Update () {
        transform.rotation = GameObject.FindGameObjectWithTag("MainCamera").transform.rotation;
        transform.position += new Vector3(0, floatUpDistance, 0) * Time.deltaTime;
        scoreText.CrossFadeAlpha(0, 1, false);
	}
    IEnumerator DestroyAfterSeconds()
    {
        yield return new WaitForSeconds(secondsAlive);
        Destroy(this.gameObject);
    }
}