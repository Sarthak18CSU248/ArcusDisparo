using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShootBow : MonoBehaviour {

    [SerializeField] GameObject arrowPrefab = null; 
    [SerializeField] GameObject bow = null; 
    [SerializeField] int arrowsRemaining = 10;
    [SerializeField] int pullSpeed = 10;

    public GameObject arrow;
    private TrailRenderer trail; //trail for the arrow
    private bool stopDraw = false;
    public AudioManager Audio_Manager;
    //private bool zoom = false;

    private bool reset = false;

    private bool knockedArrow = false;
    private float drawDistance = 0;

    private Quaternion originalRot;
    private Vector3 originalPos;

    private float changeRot = 45f / 46f;
    private float totalHipRotChange = -85f;

    private Vector3 changeBowPos = new Vector3(0, 0.005f * 2f, -0.0025f * 2f);
    private Vector3 changeArrowPos = new Vector3(-0.01651724137f*2f, -0.00655172413f*2f, 0.00125862068f*2f);

    private Vector3 totalBowPosChanges = new Vector3(0, 0, 0);
    private Vector3 totalArrowPosChanges = new Vector3(0, 0, 0);

    private void Start() {
        originalRot = bow.transform.localRotation; //store the bow's original rotatation
        originalPos = bow.transform.localPosition; //store the bow's original pos
        SpawnArrow();
        Audio_Manager = FindObjectOfType<AudioManager>();
    }

    //Once per frame, check to see if the player attempted to shoot an arrow 
    private void Update() {
        ShootArrow();

        if (reset == true) {
            if (bow.transform.localRotation.eulerAngles.x <= originalRot.eulerAngles.x) reset = false;
            else StartCoroutine(ResetRotation());
        }
    }

    //Spawn an arrow at the transform's position
    private void SpawnArrow() {
        if(arrowsRemaining > 0) { //if the player still has arrows

            if (reset) { //If the bow is still resetting, then we'll just jump back to the starting animation
                reset = false;
                bow.transform.localRotation = originalRot;
                bow.transform.localPosition = originalPos;
            }

            knockedArrow = true;
            arrow = Instantiate(arrowPrefab, transform.position, transform.rotation) as GameObject;
            arrow.transform.SetParent(transform, true);
            arrow.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
            trail = arrow.GetComponent<TrailRenderer>(); //get the new trail component
        }
    }
    
    //reset the rotation (and height) of the bow to rest position. 
    private IEnumerator ResetRotation() {
        while (bow.transform.localRotation.eulerAngles.x > originalRot.eulerAngles.x) {
            bow.transform.Rotate(Time.deltaTime * -5, 0, 0, Space.Self);

            if (bow.transform.localPosition.y >= -0.4) bow.transform.localPosition = new Vector3(0, bow.transform.localPosition.y - 0.002f, bow.transform.localPosition.z);

            yield return new WaitForSeconds(0.001f);
        }
    }

    //Reset the bow and arrow locations after an arrow is fired or canceled.
    private void ResetBow() {
        totalBowPosChanges.y = 0;
        bow.transform.localPosition -= totalBowPosChanges; //reset bow's pos
        reset = true;

        transform.localPosition -= totalArrowPosChanges; //reset ArrowSpawnPoint's pos

        totalBowPosChanges = new Vector3(0, 0, 0); //clear the total changes
        totalArrowPosChanges = new Vector3(0, 0, 0); //clear the total changes
        totalHipRotChange = -85f;
    }

    //Check to see if the player wants to shoot an arrow
    private void ShootArrow() {
        if (arrowsRemaining > 0)
        {           

            
            SkinnedMeshRenderer bowSkin = bow.transform.GetComponent<SkinnedMeshRenderer>();
            SkinnedMeshRenderer arrowSkin = arrow.transform.GetComponent<SkinnedMeshRenderer>();
            Rigidbody arrowRB = arrow.transform.GetComponent<Rigidbody>();
            ArrowForce af = arrow.transform.GetComponent<ArrowForce>(); 
            if (Input.GetMouseButtonDown(0))
                Audio_Manager.Play("BowString");
            
            if (Input.GetMouseButton(0) && !stopDraw)
            {
               
                
                if (Input.GetKeyDown(KeyCode.R))
                {
                    stopDraw = true;
                    drawDistance = 0;
                    ResetBow();
                }

                if (!stopDraw)
                { 
                    drawDistance += Time.deltaTime * pullSpeed; //set the draw distance

                    
                    if (drawDistance < 100)
                    {


                        //Move the bow and arrow to be closer as the player draws the string back
                        bow.transform.localPosition += changeBowPos; transform.localPosition += changeArrowPos;
                        bow.transform.localRotation = Quaternion.Euler(totalHipRotChange += changeRot, -90, 0);

                        totalBowPosChanges += changeBowPos;
                        totalArrowPosChanges += changeArrowPos;

                    }
                    else
                    {
                        drawDistance = 100;
                        Audio_Manager.Stop("BowString");
                    }
                }
            }

            if (Input.GetMouseButtonUp(0)) {
                if (stopDraw) stopDraw = false;
                else if (drawDistance >= 10) { //if the draw distance is enough to actually launch the arrow
                    knockedArrow = false; //set it so the player no longer has an arrow drawn
                    arrowRB.isKinematic = false;
                    
                    Audio_Manager.Play("ArrowShoot");
                    Handheld.Vibrate();
                    arrow.transform.SetParent(null);
                    arrow.transform.position = transform.position;

                    arrowsRemaining -= 1;

                    af.shootForce = af.shootForce * ((drawDistance / 100) + 0.05f); //calculate the force of the arrow based on the draw distance

                    drawDistance = 0; 
                    af.enabled = true; 
                    trail.enabled = true; //enable the trail effect

                    ResetBow(); //reset the bow's rotation and position
                }
                else {
                    drawDistance = 0; 
                    ResetBow();
                }
            }
            
            bowSkin.SetBlendShapeWeight(0, drawDistance); 
            arrowSkin.SetBlendShapeWeight(0, drawDistance);
        }

        //If the player hit the left mouse and doesn't have an arrow knocked, spawn another arrow
        if (Input.GetMouseButtonDown(0) && !knockedArrow) SpawnArrow();        
    }
}
