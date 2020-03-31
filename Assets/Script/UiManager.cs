using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UiManager : MonoBehaviour
{
    public Text timer_txt;
    public Text target_txt;
    private float timer = 60f;
    public AudioSource whistle;
    public Image healthbar;
    public float health = 50f;
    private float TargetScore = 980f;
    public static UiManager instance;

    // Start is called before the first frame update
    void Start()
    {
        whistle = gameObject.GetComponent<AudioSource>();
        target_txt.text = "*" + Convert.ToString(TargetScore);
    }

    // Update is called once per frame
    void Update()
    {
        Mathf.Clamp(health, 0, 100);
        healthbar.fillAmount = (health / 100);
        timer -= Time.deltaTime;
        timer_txt.text = Convert.ToString(Mathf.Round(timer));
        if (ScoreManager.instance.score >= TargetScore)
        {
            TargetScore += 250;
        }
        if (ScoreManager.instance.score >= TargetScore && Mathf.Round(timer) < 0)
        {
            SceneManager.LoadScene("CongratulationScreen");
        }
        if (Mathf.Round(timer) == 0)
        {
            SceneManager.LoadScene("ScoreScreen");
        }
        if(Mathf.Round(timer) ==10)
        {
            whistle.Play();
        }
        if(Mathf.Round(health) == 0f)
        {
            Invoke("FailedScreen_Load", 1f);  
        }  
    }
    public void FailedScreen_Load()
    {
        SceneManager.LoadScene("FailedScreen");
    }

}
