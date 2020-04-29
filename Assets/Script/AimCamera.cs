using System;
using UnityEngine;

public class AimCamera : MonoBehaviour {
    private Rigidbody rb;
    private bool inAir = false;
    private bool crouch = false;
    private Vector3 ogScale;    
    [SerializeField] float verSensitivity = 0.5f;
    [SerializeField] float horSensitivity = 0.5f;
    [SerializeField] float speed = 0.5f;
    [SerializeField] float jumpForce = 1f;
    [SerializeField] float crouchScale = 1.5f;
    [SerializeField] Camera cam = null;

    private Vector3 firstpoint;
    private Vector3 secondpoint;
    private float yAngle;
    private float xAngle;
    private float yAngleTemp;
    private float xAngleTemp;

    protected Joystick joystick;

  
    private void Start() {
        rb = GetComponent<Rigidbody>(); 
        Cursor.visible = false; //make the cursor isn't visible
        ogScale = transform.localScale;
        joystick = FindObjectOfType<Joystick>();
    }

    //called once per frame
    private void Update() {
      
        CheckForCameraMovement();
        if (Input.GetKeyDown(KeyCode.Space) && !inAir) {
            rb.AddForce(new Vector3(0, jumpForce, 0)); //jump
            inAir = true;
        }
       
        if (Input.GetKeyDown(KeyCode.LeftControl) && !inAir && !crouch){
            transform.localScale -= new Vector3(0, transform.localScale.y - crouchScale, 0);
            crouch = true;
        }
        else if(Input.GetKeyDown(KeyCode.LeftControl) && !inAir && crouch){
            transform.localScale = ogScale;
            crouch = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (Cursor.visible) Cursor.visible = false; 
            else Cursor.visible = true; 
        }

        //If not aiming, move at regular speed
        if(!Input.GetMouseButton(0) && !Input.GetKey(KeyCode.Space)) CheckForMovement(speed);
        //else you'll move at half speed
        else CheckForMovement(speed / 1.5f);
    }

    //Check for ground collision 
    private void OnCollisionEnter(Collision c) { if (c.gameObject.tag == "Terrain") inAir = false; }

    private void CheckForMovement(float moveSpeed) {        
        rb.MovePosition(transform.position + (transform.right * Input.GetAxis("Vertical") * moveSpeed) 
            + (transform.forward * -Input.GetAxis("Horizontal") * moveSpeed));        
    }

    
    private void CheckForCameraMovement()
    {
        Vector3 dir = Vector3.zero;
        float x = joystick.Horizontal;
        float z = joystick.Vertical;
        Vector3 rotateX = new Vector3(z * verSensitivity, x*horSensitivity, 0);
        transform.Rotate(-rotateX,Space.Self);
        var tempEulers = transform.rotation.eulerAngles;
        tempEulers.x = Mathf.Clamp(tempEulers.x, 1.2f, 8);
        tempEulers.y = Mathf.Clamp(tempEulers.y, 285, 305);
        tempEulers.z = Mathf.Clamp(tempEulers.z, 0, 5);
        transform.rotation = Quaternion.Euler(tempEulers);
    }
}