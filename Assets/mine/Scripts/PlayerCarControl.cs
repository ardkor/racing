using UnityEngine;

public class PlayerCarControl : MonoBehaviour
{
    private float motorTorque = 4000;
    private float brakeTorque = 6000;
    private float maxSpeed = 30;
    private float steeringRange = 25;
    private float steeringRangeAtMaxSpeed = 15;
    private float centreOfGravityOffset = -0.8f;
    private float defaultBrakeFactor = 0.6f;

    private float vInput;
    private float hInput;

    private bool enableInput = false;

    private FinishZone _finish;

    [SerializeField] private LevelManager _levelManager;


    WheelControl[] wheels;
    Rigidbody rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        DisableGravity();
        // Adjust center of mass vertically, to help prevent the car from rolling
        rigidBody.centerOfMass += Vector3.up * centreOfGravityOffset;

        // Find all child GameObjects that have the WheelControl script attached
        wheels = GetComponentsInChildren<WheelControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enableInput)
        {
            vInput = Input.GetAxis("Vertical");
            hInput = Input.GetAxis("Horizontal");
        }
        else
        {
            vInput = 0;
            hInput = 0;
        }

        // Calculate current speed in relation to the forward direction of the car
        // (this returns a negative number when traveling backwards)
        float forwardSpeed = Vector3.Dot(transform.forward, rigidBody.velocity);


        // Calculate how close the car is to top speed
        // as a number from zero to one
        float speedFactor = Mathf.InverseLerp(0, maxSpeed, forwardSpeed);

        // Use that to calculate how much torque is available 
        // (zero torque at top speed)
        float currentMotorTorque = Mathf.Lerp(motorTorque, 0, speedFactor);

        // …and to calculate how much to steer 
        // (the car steers more gently at top speed)
        float currentSteerRange = Mathf.Lerp(steeringRange, steeringRangeAtMaxSpeed, speedFactor);

        // Check whether the user input is in the same direction 
        // as the car's velocity
        bool isAccelerating = Mathf.Sign(vInput) == Mathf.Sign(forwardSpeed);

        foreach (var wheel in wheels)
        {
            // Apply steering to Wheel colliders that have "Steerable" enabled
            if (wheel.steerable)
            {
                wheel.WheelCollider.steerAngle = hInput * currentSteerRange;
            }

            if (isAccelerating)
            {
                // Apply torque to Wheel colliders that have "Motorized" enabled
                if (wheel.motorized)
                {
                    wheel.WheelCollider.motorTorque = vInput * currentMotorTorque;
                }
                wheel.WheelCollider.brakeTorque = 0;
            }
            else
            {
                // If the user is trying to go in the opposite direction
                // apply brakes to all wheels
                wheel.WheelCollider.brakeTorque = Mathf.Abs(vInput) * brakeTorque;
                wheel.WheelCollider.motorTorque = 0;
            }
        }
    }
    void FixedUpdate()
    {
        if (rigidBody.velocity.magnitude > 0.1f)
        {  // Если машина движется
            rigidBody.AddForce(-rigidBody.velocity * defaultBrakeFactor, ForceMode.Acceleration);
        }
    }
    private void OnEnable()
    {
        _levelManager.levelStarted += EnableInput;
        _levelManager.levelStarted += InstallFinish;
        _levelManager.levelStarted += EnableGravity;
    }
    private void OnDisable()
    {
        _levelManager.levelStarted -= EnableInput;
        _levelManager.levelStarted -= InstallFinish;
        _levelManager.levelStarted -= EnableGravity;
        _finish.levelFinnished -= DisableInput;
    }
    private void DisableInput()
    {
        enableInput = false;
    }
    private void EnableInput()
    {
        enableInput = true;
    }
    private void DisableGravity()
    {
       // rigidBody.useGravity = false;
        rigidBody.isKinematic = true;
    }
    private void EnableGravity()
    {
       // rigidBody.useGravity = true;
        rigidBody.isKinematic = false;
    }
    private void InstallFinish() 
    {
        _finish = _levelManager.finishZone;
        _finish.levelFinnished += DisableInput;

    }
}