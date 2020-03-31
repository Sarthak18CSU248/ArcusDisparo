using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scenes : MonoBehaviour
{
    public AudioSource button;
    public Text score;
    public static Scenes instance;
    private void Start()
    {
        button = gameObject.GetComponent<AudioSource>();
        instance = this;
    }
    private void Update()
    {
        score.text = ScoreManager.instance.score.ToString();
    }
    public void Play()
    {
        SceneManager.LoadScene("Instruction");
        button.Play();
    }
    public void Quit()
    {
        button.Play();
        Application.Quit();
    }
    public void Retry()
    {
        SceneManager.LoadScene("Main");
        button.Play();
    }
}
