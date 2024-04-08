using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private int childCount;
    Rigidbody rb;
    private float weight, minLift, maxLift, AOAInput, horizontalMotionInput, yawInput, liftForce, timer, bladeRotationSpeed = 1f; //Helicopter variables
    private float scriptTimeInterval = 1, fixedScriptTime, scriptTime; //Script time variables
    private bool wKeyDown = false, sKeyDown = false, bladeHit = false;
    public float rotationAmount = 50f, yawLimit = 30f, minLiftMultiplier, maxLiftMultiplier, liftIncrement, timeInterval; //Variables that are to be changed from the editor
    public LiftBar liftBar; //Canvas object to represent the lift power to the user
    GAME gameState = GAME.INIT;
    public enum GAME //Game state enum to keep workflow accordingly
    {
        INIT, //initialization phase
        START, //start phase
        STOP, //stop - menu etc
        RESTART, //reset
        OVER //state between over and init
    }
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        AdjustScriptTime();
        if (bladeHit)
            bladeRotationSpeed = Mathf.Lerp(bladeRotationSpeed, 0, 0.001f);
        Animate();
        if (CheckGameState() == GAME.START)
        {
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
        SetGameState(GAME.START);
    }
    void Animate() //Just a little rotation animation as cosmetics.
    {
        transform.GetChild(0).Rotate(0, 0, 1000 * scriptTime * bladeRotationSpeed);
        transform.GetChild(1).Rotate(0, 0, 2000 * scriptTime * bladeRotationSpeed);
    }
    private void FixedUpdate() //To apply forces on the rigidbody in specific time period fixed update is used.
    {
        if (CheckGameState() == GAME.START)
        {
            CalculateForces();
        }
    }
    public GAME CheckGameState()
    {
        return gameState;
    }

    public void AdjustScriptTime() //to stop all the motion and physics calculation while not touching the Time.timeScale to display UI changes on frames such as brightness.
    {
        if (scriptTimeInterval == 0)
        {
            rb.isKinematic = true;
            ChangeBladeAttribute(true);
        }
        else
        {
            rb.isKinematic = false;
            ChangeBladeAttribute(false);            
        }
        scriptTime = Time.deltaTime * scriptTimeInterval;
        fixedScriptTime = Time.fixedTime * scriptTimeInterval;
    }

    public void ChangeBladeAttribute(bool flag) //alters the kinematic option depending on the frame(does user see game or UI)
    {
        Rigidbody childRb;
        if (transform.GetChild(1).name.ToLower() == "blades")
        {
            if (transform.GetChild(1).TryGetComponent<Rigidbody>(out childRb))
            {
                childRb.isKinematic = flag;
                CheckBladeVelocityForBladeRotation(childRb);
            }
        }
    }

    public void CheckBladeVelocityForBladeRotation(Rigidbody r) //After the blades touched any surface, it checks whether the blades are still airborne or not so that animation keeps going like it still has momentum
    {
        if (r.velocity.magnitude != 0 && r.velocity.magnitude < 0.8f)
            bladeRotationSpeed = Mathf.Lerp(bladeRotationSpeed, 0, 0.05f);
    }
    public void SetGameState(GAME g) //just a gamestate setter function to manipulate the game state in other scripts
    {
        gameState = g;
    }
    void ReadInput()
    {
        //INPUT Management
        AOAInput = Input.GetAxis("AOA"); //Angle of attack
        horizontalMotionInput = Input.GetAxis("Horizontal"); //Horizontal rotation
        yawInput = Input.GetAxis("Yaw");

        /*Debug.Log("AOT: " + AOAInput);
        Debug.Log("Horizontal: " + horizontalMotionInput);
        Debug.Log("Yaw: " + yawInput);
        Debug.Log("Lift Input: " + liftInput);*/
        InputTimer(timeInterval);
        if (Input.GetKeyDown(KeyCode.W))
        {
            wKeyDown = true;
            timer = fixedScriptTime;
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            wKeyDown = false;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            sKeyDown = true;
            timer = fixedScriptTime;
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            sKeyDown = false;
        }
        transform.Rotate(rotationAmount * scriptTime * horizontalMotionInput, 0, 0);
        transform.Rotate(0, 0, rotationAmount * scriptTime * AOAInput);
        transform.Rotate(Vector3.up * yawLimit * scriptTime * yawInput);
    }

    public void SetScriptTimeInterval(float x)
    {
        scriptTimeInterval = x;
    }
    void InputTimer(float delay) //This function limits the frames to apply force to the rigidbody in a given time period.
    {
        if (((sKeyDown || wKeyDown) && !(sKeyDown && wKeyDown)) && fixedScriptTime > timer + delay)
        {
            if (sKeyDown && liftForce > minLift)
            {
                liftForce -= liftIncrement;
            }
            else if (wKeyDown && liftForce < maxLift)
            {
                liftForce += liftIncrement;
            }
            //Debug.Log(sKeyDown ? "sKeyDown " + "Button was held for " + (Time.fixedTime - timer) + " seconds" : "wKeyDown " + "Button was held for " + (Time.fixedTime - timer) + " seconds");
            timer = fixedScriptTime;
            liftBar.SetValue(liftForce);
        }
        else if(sKeyDown && wKeyDown)
        {
            timer = fixedScriptTime;
        }
    }
    void CalculateForces()
    {
        rb.AddRelativeForce(Vector3.up * liftForce, ForceMode.Force);
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(rb.velocity.x);
        if ((rb.velocity.x > 3 || rb.velocity.x < -3) || (rb.velocity.y > 3 || rb.velocity.y < -3) || (rb.velocity.z > 3 || rb.velocity.z < -3))
        {
            Debug.Log("Crash");
        }
        rb.freezeRotation = false;
        
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
        bladeHit = true;
        gameState = GAME.OVER;
        if (childCount == transform.childCount)
        {
            GameObject a = transform.GetChild(1).gameObject;
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
