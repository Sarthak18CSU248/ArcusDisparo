using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjects : MonoBehaviour
{
    [SerializeField]
    private float minXSpeed, maxXSpeed, minYSpeed, maxYSpeed,minZSpeed, maxZSpeed;

    [SerializeField]
    private float destroyTime;
    [SerializeField]
    private AudioSource audio_Play;
   

    // Use this for initialization
    void Start()
    {
        this.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(minXSpeed, maxXSpeed), Random.Range(minYSpeed, maxYSpeed), Random.Range(minZSpeed, maxZSpeed));
        Destroy(this.gameObject, this.destroyTime);
        audio_Play = gameObject.GetComponent<AudioSource>();

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Arrow")
        {
            Debug.Log("TRiggered");
            audio_Play.Play();
            UiManager.instance.timer += 10f;
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Arrow")
        {
            if (gameObject.tag == "Fruit")
            {
                audio_Play.Play();
                UiManager.instance.timer += 10;
                Destroy(this.gameObject);
            }
            else if (gameObject.tag == "Bomb")
            {
                audio_Play.Play();
                Destroy(this.gameObject);
            }
        }
    }
}
