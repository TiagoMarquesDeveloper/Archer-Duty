using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PointTracker : MonoBehaviour {
    [SerializeField]
    GameObject impactPrefab;
    public int value = 50;
 
	// Use this for initialization
	void Start () {
  
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnTriggerEnter(Collider coll)
    {

        ScoreManager.instance.score += value;
        ScoreManager.instance.anim.SetTrigger("scored");
        GameObject _canvas = Instantiate(impactPrefab, coll.transform.position, coll.transform.rotation) as GameObject;
        _canvas.GetComponentInChildren<Text>().text = value.ToString();
        Destroy(_canvas, 5);
        transform.parent.parent.gameObject.SetActive(false);

        GameController.targetCount++;
    }
}
