using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BombExplosion : MonoBehaviour
{
    public GameObject Explosion;
    public GameObject Bomb;
    private UiManager ui;
    private Animator Explosion_anim;
    public AudioSource explosion;
    // Start is called before the first frame update
    void Start()
    {
        Explosion_anim = Explosion.GetComponent<Animator>();
        ui = GameObject.FindGameObjectWithTag("UI").GetComponent<UiManager>();

    }

    // Update is called once per frame
    void Update()
    {
        Explosion.transform.position = Bomb.transform.position;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Arrow")
        {
            ui.health -= 25f;
            Bomb.SetActive(false);
            Explosion_anim.enabled = false;
            Explosion.SetActive(true);
            explosion.Play();
            Handheld.Vibrate();
            Destroy(this.Explosion,1f);
        }
    }

}