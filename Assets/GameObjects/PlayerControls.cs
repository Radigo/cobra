//using System.Numerics;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    Rigidbody rb;
    // Inputs
    private bool isInputDown = false;
    private float inputInitX = 0.0f;
    private float inputDeltaX = 0.0f;

    // Transform
    private float steerInit = 0.0f;
    private float speed = 0.0f;

    // Gameplay
    public float steeringMultiplier = 0.6f;
    public float thrustMultiplier = 200.0f;
    public float speedMax = 1000.0f;
    public float brakeMultiplier = 0.5f;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() {
        // Handle mouse inputs
        if (!isInputDown) {
            if (Input.GetMouseButtonDown(0)) {
                isInputDown = true;
                touchBegan(Input.mousePosition.x);
            }
            touchUp();
        } else {
            if (Input.GetMouseButtonUp(0)) {
                isInputDown = false;
            } else {
                touchMoved(Input.mousePosition.x);
            }
            touchDown();
        }

        // Handle screen touches.
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began) {
                touchBegan(touch.position.x);
            }
            
            if (touch.phase == TouchPhase.Moved) {
                touchMoved(touch.position.x);
            }

            /*
            if (Input.touchCount == 2)
            {
                touch = Input.GetTouch(1);

                if (touch.phase == TouchPhase.Began)
                {
                    // Halve the size of the cube.
                    transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
                }

                if (touch.phase == TouchPhase.Ended)
                {
                    // Restore the regular size of the cube.
                    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                }
            }
            */
        }
    }

    void touchBegan(float p_position) {
        // Init steering
        inputInitX = p_position;
        steerInit = transform.eulerAngles.y;
        //Debug.Log("<touchBegan> @ " + p_position + ", steerInit: " + steerInit);
    }

    void touchDown() {
        // Thrust
        speed = Mathf.Min((speed + (thrustMultiplier * Time.deltaTime)), speedMax);
        //Debug.Log("<touchDown> Accel. to " + speed + ", direction: " + transform.rotation);
        rb.AddForce(transform.rotation * Vector3.forward * speed * Time.deltaTime, ForceMode.Acceleration);
    }

    void touchMoved(float p_position) {
        // Steer when moving the finger on the screen
        inputDeltaX = p_position - inputInitX;
        //Debug.Log("Touch moved to " + p_position + ", steerInit: " + steerInit + ", deltaX: " + inputDeltaX);
        
        // Rotate player
        transform.rotation = Quaternion.Euler(0.0f, steerInit + (inputDeltaX * steeringMultiplier), 0.0f);
    }

    void touchUp() {
        //Debug.Log("<touchUp>");
        // Slow down
        speed = Mathf.Max(((speed - brakeMultiplier) * Time.deltaTime), 0.0f);
    }

    public float getThrustRatio() {
        //Debug.Log("<getThrustRatio> speedCurrent: " + speedCurrent + ", maxSpeed: " + maxSpeed);
        return speed / speedMax;
    }

    public Vector3 getVelocityNormalized() {
        // Returns the actual velocity vector of the boat, regardless where it's facing (ignore forward vector)
        //Debug.Log("<getVelocityNormalized> " + rb.velocity);
        if (rb.velocity.magnitude < 0.02f) {
            return Vector3.zero;
        }

        return rb.velocity.normalized;
    }
}
