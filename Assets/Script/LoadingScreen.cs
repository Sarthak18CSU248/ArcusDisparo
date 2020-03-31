using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    public GameObject instruction,Load_img;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Invoke("Loading_Screen", 5f);  
    }
    public void Loading_Screen()
    {
        instruction.SetActive(false);
        Load_img.SetActive(true);
        SceneManager.LoadScene("Main");
    }
}
