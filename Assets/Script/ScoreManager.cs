using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ScoreManager : MonoBehaviour
{
    public int score = 0;
    public static ScoreManager instance;
    [SerializeField]
    Text scoreText;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = ":" + score.ToString();
        if(UiManager.instance.TargetScore <= score)
        {
            SceneManager.LoadScene("CongratulationScreen");
        }
    }
}
