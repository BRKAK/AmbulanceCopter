using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private int childCount;
    Rigidbody rb;
    private float weight, minLift, maxLift, AOTInput, horizontalMotionInput, yawInput, liftInput, liftForce, timer;
    private bool wKeyDown = false, sKeyDown = false;
    public float rotationAmount = 50f, yawLimit = 30f, minLiftMultiplier, maxLiftMultiplier, liftIncrement, timeInterval;
    public LiftBar liftBar;
    GAME gameState = GAME.STOP;
    enum GAME //Game state enum to keep workflow accordingly
    {
        START,
        STOP,
        RESTART,
        OVER
    }
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckGameState() == GAME.START)
        {
            Animate();
            ReadInput();
        }
    }
    void Initialize()   //Rigidbody, physics calculation and interface canvas(liftBar) initializitaion
    {
        rb = transform.GetComponent<Rigidbody>();
        weight = rb.mass * Physics.gravity.magnitude;
        print(weight);
        minLift = weight - minLiftMultiplier;
        maxLift = weight + maxLiftMultiplier;
        liftBar.SetValue(minLift);
        liftBar.setMaxValue(maxLift);
        liftBar.setMinValue(minLift);
        childCount = transform.childCount;
        liftForce = minLift;
    }
    void Animate() //Just a little rotation animation override as cosmetics.
    {
        Debug.Log(liftForce - minLift);
        transform.GetChild(0).Rotate(0, 0, 1000 * Time.deltaTime) ;
        transform.GetChild(1).Rotate(0, 0, 2000 * Time.deltaTime);
    }
    private void FixedUpdate() //To apply forces on the rigidbody in specific time preiod fixed update is used.
    {
        //print(rb.velocity.magnitude);
        if (CheckGameState() == GAME.START)
        {
            //rb.velocity.magnitude
            //rb.AddRelativeForce(Vector3.up * weight, ForceMode.Force);
            CalculateForces();
        }
    }
    private GAME CheckGameState()
    {
        return gameState;
    }
    void ReadInput()
    {
        //INPUT Management
        AOTInput = Input.GetAxis("AOT"); //Angle of attack
        horizontalMotionInput = Input.GetAxis("Horizontal"); //Horizontal rotation
        yawInput = Input.GetAxis("Yaw");
        //Debug.Log(Input.GetKey(KeyCode.W));
        
        liftInput = Input.GetAxis("Lift"); //Lift force input

        /*Debug.Log("AOT: " + AOTInput);
        Debug.Log("Horizontal: " + horizontalMotionInput);
        Debug.Log("Yaw: " + yawInput);*/
        //Debug.Log("Lift Input: " + liftInput);
        InputTimer(timeInterval);
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("Key Down");
            wKeyDown = true;
            timer = Time.fixedTime;
            Debug.Log("Timer: " + timer);
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            Debug.Log("Key Up");
            wKeyDown = false;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("Key Down");
            sKeyDown = true;
            timer = Time.fixedTime;
            Debug.Log("Timer: " + timer);
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            Debug.Log("Key Up");
            sKeyDown = false;
        }
        //Debug.Log("Lift Force: " + liftForce);
        
        transform.Rotate(rotationAmount * Time.deltaTime * horizontalMotionInput, 0, 0);

        transform.Rotate(0, 0, rotationAmount * Time.deltaTime * AOTInput);
        transform.Rotate(Vector3.up * yawLimit * Time.deltaTime * yawInput);
        

    }
    void InputTimer(float delay) //This function limits the frames to apply force to the rigidbody in a given time period.
    {
        if (((sKeyDown || wKeyDown) && !(sKeyDown && wKeyDown)) && Time.fixedTime > timer + delay)
        {
            if (sKeyDown && liftForce > minLift)
            {
                //Debug.Log("S");
                liftForce -= liftIncrement;
            }
            else if (wKeyDown && liftForce < maxLift)
            {
                liftForce += liftIncrement;
                //Debug.Log("W");
            }
            //Debug.Log(sKeyDown ? "sKeyDown " + "Button was held for " + (Time.fixedTime - timer) + " seconds" : "wKeyDown " + "Button was held for " + (Time.fixedTime - timer) + " seconds");
            timer = Time.fixedTime;
            liftBar.SetValue(liftForce);
        }
        else if(sKeyDown && wKeyDown)
        {
            timer = Time.fixedTime;
        }
    }
    void CalculateForces()
    {
        rb.AddRelativeForce(Vector3.up * liftForce, ForceMode.Force);
    }
    private void OnCollisionEnter(Collision collision)
    {
        rb.freezeRotation = false;
        if (CheckGameState() == GAME.STOP)
            gameState = GAME.START;
    }
    private void OnCollisionStay(Collision collision)
    {
        rb.freezeRotation = false;

    }
    private void OnCollisionExit(Collision collision) //To prevent unwanted bounces and forces agaist gravitational pull
    {
        rb.freezeRotation = true;
    }
    private void OnTriggerEnter(Collider other) //This function disassembles the blades from the helicopter on collision with something.
    {
        Debug.Log("Blade hit");
        gameState = GAME.OVER;
        if (childCount == transform.childCount)
        {
            for (int i = 0; i < childCount; i++)
            {
                GameObject a = transform.GetChild(i).gameObject;
                if (a.name.ToLower() == "blades"/* || a.name.ToLower() == "rotor"*/)
                {
                    Rigidbody crb = a.AddComponent<Rigidbody>();
                    crb.useGravity = true;
                    crb.AddForce(Random.Range(-5, 5), Random.Range(5, 10), Random.Range(-5, 5), ForceMode.Impulse);
                    if (a.GetComponent<MeshCollider>() == null)
                    {
                        MeshCollider m = a.AddComponent<MeshCollider>();
                        m.isTrigger = false;
                        m.convex = true;
                    }
                    else
                    {
                        a.GetComponent<MeshCollider>().isTrigger = false;
                    }
                    a.transform.SetParent(a.transform);
                }
                liftBar.SetValue(minLift);
            }
        }

    }
}
