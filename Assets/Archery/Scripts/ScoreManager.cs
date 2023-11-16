using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScoreManager : MonoBehaviour {
    public int score = 0;
    public static ScoreManager instance;
    public Animator anim;
    [SerializeField]
    public Text scoreText;
    [SerializeField]
    public Text scoreText1;
    // Use this for initialization
    void Start () {
        instance = this;
        anim = scoreText.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        scoreText.text = score.ToString();
        scoreText1.text = score.ToString();
    }
}
