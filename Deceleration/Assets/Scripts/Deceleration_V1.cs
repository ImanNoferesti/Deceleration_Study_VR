
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Oculus.Haptics;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


public class Deceleration_V1 : MonoBehaviour
{
    public GameObject prefab;
    public float objectHeight = 1.83f; 
    GameObject instantiatedPrefab;
    float carStartingPosition = -120;
    int totalTrials = 112;
    Dictionary<int, HashSet<float>> accel_Vel_Dictionary;
    Dictionary<int, int> observationCount;
    public HapticClip clip;
    HapticClipPlayer playerRight;

    float velocity;
    float initVelSelected;
    
    float accelSelected;
    float speed;
    float lastResetTime = 0;
    float startTime;
    float currentPosition;
    float positionToDecel;
    bool isCarCreated = false;
    bool rightTriggerIsPressed = false;
    bool leftTriggerIsPressed = false;
    bool isConstantVelocity;
    bool isDecelerated = true;
    bool zeroAcceleration = false;
    bool willBeDestroyed;
    float delay = 3f;
    float customTime;
    float headXPos;
    int trialNum = -15;
    int randomTrialsBetween = 2;
    int padding;
    bool isInitialMovement = false;
    float time1;
    bool startActualTrials = false;

    [HideInInspector] public List<int> response = new List<int>();
    [HideInInspector] public List<float> carVelocity = new List<float>();
    [HideInInspector] public List<float> carAcceleration = new List<float>();
    [HideInInspector] public List<float> carPosition = new List<float>();
    [HideInInspector] public List<float> tau = new List<float>();
    [HideInInspector] public List<float> tau2 = new List<float>();
    [HideInInspector] public List<float> tauDot1 = new List<float>();
    [HideInInspector] public List<float> tauDot2 = new List<float>();
    [HideInInspector] public List<float> visualAngle = new List<float>();
    [HideInInspector] public List<float> visualAngle2 = new List<float>();
    [HideInInspector] public List<float> RCVA = new List<float>();
    [HideInInspector] public List<float> RCVA2 = new List<float>();
    [HideInInspector] public List<float> velocityVector = new List<float>();
    [HideInInspector] public List<float> velocityAngle = new List<float>();
    [HideInInspector] public List<float> distanceVector = new List<float>();
    [HideInInspector] public List<float> carDistance = new List<float>();
    [HideInInspector] public List<int> trialNumber = new List<int>();
    [HideInInspector] public List<float> carXPos = new List<float>();
    [HideInInspector] public List<float> carYPos = new List<float>();
    [HideInInspector] public List<float> carZPos = new List<float>();
    [HideInInspector] public List<int> carCreation =   new List<int>();
    [HideInInspector] public List<int> carDestruction =   new List<int>();
    [HideInInspector] public List<int> carDecelStart = new List<int>();
    [HideInInspector] public List<int> availableKeys = new List<int>();
    [HideInInspector] public List<int> availableKeysPractice = new List<int>();
    


    
    // Start is called before the first frame update
    void Start()
    {
        playerRight = new HapticClipPlayer(clip);
        padding = randomTrialsBetween;
        //Dictionary containing all possible combinations of accelerations and velocities
        accel_Vel_Dictionary = new Dictionary<int, HashSet<float>>
        {
            //Adding sets of acceleration and velocity values to the dictionary
            { 1, new HashSet<float>(new float[] { 0f, 8.37f }) },
            { 2, new HashSet<float>(new float[] { 0f, 10.58f }) },
            { 3, new HashSet<float>(new float[] { 0f, 13.04f }) },
            { 4, new HashSet<float>(new float[] { 0f, 15.17f }) },
            { 5, new HashSet<float>(new float[] { 0f, 16.49f }) },
            { 6, new HashSet<float>(new float[] { 0f, 17.46f }) },
            { 7, new HashSet<float>(new float[] { 0f, 19.18f }) },
            { 8, new HashSet<float>(new float[] { 0f, 22.09f }) },
            { 9, new HashSet<float>(new float[] { -1.4f, 8.37f }) },
            { 10, new HashSet<float>(new float[] { -1.4f, 10.58f }) },
            { 11, new HashSet<float>(new float[] { -3.4f, 13.04f }) },
            { 12, new HashSet<float>(new float[] { -3.4f, 16.49f }) },
            { 13, new HashSet<float>(new float[] { -4.6f, 15.17f }) },
            { 14, new HashSet<float>(new float[] { -4.6f, 19.18f }) },
            { 15, new HashSet<float>(new float[] { -6.1f, 17.46f }) },
            { 16, new HashSet<float>(new float[] { -6.1f, 22.09f }) }
        };

        // Dictionary for counting the number of times each combination has been selected
        observationCount = new Dictionary<int, int>
        {
            {1,0},
            {2,0},
            {3,0},
            {4,0},
            {5,0},
            {6,0},
            {7,0},
            {8,0},
            {9,0},
            {10,0},
            {11,0},
            {12,0},
            {13,0},
            {14,0},
            {15,0},
            {16,0}
        };        

        //Initialize the available keys list for actual trials
        availableKeysPractice.AddRange(accel_Vel_Dictionary.Keys);

        //Initialize the available keys list for actual trials
        availableKeys.AddRange(accel_Vel_Dictionary.Keys);
    }


    // Update is called once per frame
    void Update()
    {
        OVRInput.Update();
        trialNumber.Add(trialNum - 1);
        carCreation.Add(0);
        carDestruction.Add(0);
        carDecelStart.Add(0);
        
        //When subject presses the trigger button on right controller
        if(!rightTriggerIsPressed)
        {
            response.Add(0);
        }

        else if(rightTriggerIsPressed)
        {
            // if(OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) != 0 || Input.GetKeyDown(KeyCode.R))
            if(OVRInput.Get(OVRInput.RawButton.RIndexTrigger))
            {
                Debug.Log("Button is pressed");
                rightTriggerIsPressed = false;
                
                //haptic feedback
                playerRight.Play(Controller.Right);
                response.Add(1);
            }
            else
            {
                response.Add(0);
            }
            
        }

        // Pressing B on the keyboard will skip practice trials, and starts actual trials immediately.
        if(Input.GetKeyDown(KeyCode.B))
        {
            leftTriggerIsPressed = true;
            startActualTrials = true;
            trialNum = 1;

        }

        //Pressing S on the keyboard, starts trials and instantiates cars
        if(Input.GetKeyDown(KeyCode.S))
        {
            leftTriggerIsPressed = true;
        }
        
        if(leftTriggerIsPressed)
        {
            //Pressing A on the keyboard, starts actual trials
             if(Input.GetKeyDown(KeyCode.A))
            {
                startActualTrials = true;
            }                         
            if(instantiatedPrefab != null) // Before instantiating a new car, the previous one will be destroyed
            {
                //Debug.Log("Trial Time: " + (Time.time - time1));
                Destroy(instantiatedPrefab);
                carDestruction.RemoveAt(carDestruction.Count - 1);
                carDestruction.Add(1);
                isCarCreated = false;

            }
            
            delay = 0.5f;
            customTime += Time.deltaTime;
                                  
            if(customTime >= delay)
            {
                if(trialNum < 1)
                {
                    CarSpawner();
                    customTime = 0;
                }
                else if(startActualTrials)
                {
                    CarSpawner();
                    customTime = 0;
                }
                
            }
                        

        }

        // A custom time to be reset everytime a car is created
        startTime = Time.time - lastResetTime; 

        // Once a car is created, computes the speed and moves it.
        if(isCarCreated)
        {
                        
            positionToDecel = PositionToDecelerate();
            // Debug.Log("Position to Decel: " + positionToDecel);
            currentPosition = instantiatedPrefab.transform.position.x;
            

            if(willBeDestroyed)
            {
                float accelVal = -accelSelected;
                float initVelVal = 0;
                delay = 2f;
                customTime += Time.deltaTime; 
                 
                speed = CurrentVelocity(accelVal, initVelVal, customTime);
                // Debug.Log("Speed is: " + speed);
                // Debug.Log("Time is: " + startTime);

                                   
                if(customTime >= delay)
                {
                    leftTriggerIsPressed = true;
                    customTime = 0;
                }
            }

            else if(currentPosition < positionToDecel)
            {
                speed = initVelSelected;
                lastResetTime = Time.time;
                if(zeroAcceleration)
                {
                    //if car is moving with constant velocity, after 1.5 seconds from position 0, it will be destroyed
                    if(currentPosition > delay * speed)
                    {
                        leftTriggerIsPressed = true;
                    }
                }
                //Debug.Log("Cond 1: Speed: " + speed);
            }

            else
            {
                if(isDecelerated)
                {
                    carDecelStart.RemoveAt(carCreation.Count - 1);
                    carDecelStart.Add(1);
                    isDecelerated = false;
                    
                }
                speed = CurrentVelocity(accelSelected, initVelSelected, startTime);
                if(speed <= 0)
                {
                    speed = 0;
                    delay = 0.5f;
                    customTime += Time.deltaTime;
                    if(customTime > delay)
                    {
                        willBeDestroyed = true;
                        customTime = 0;
                    }
                    
                }
 
            }
            
            // Moving instantiated car based on various speed that is calculated above
            instantiatedPrefab.transform.Translate(Vector3.right * Time.deltaTime * speed);

        }

        if(instantiatedPrefab == null)
            {
                carAcceleration.Add(0);
                carVelocity.Add(0);
                carDistance.Add(0);
                distanceVector.Add(0);
                velocityAngle.Add(0);
                velocityVector.Add(0);
                visualAngle.Add(0);
                visualAngle2.Add(0);
                RCVA.Add(0);
                RCVA2.Add(0);
                tau.Add(0);
                tau2.Add(0);
                tauDot1.Add(0);
                tauDot2.Add(0);
                carXPos.Add(0);
                carYPos.Add(0);
                carZPos.Add(0);
            }
        else
            {
                currentPosition =instantiatedPrefab.transform.position.x;
                headXPos = Camera.main.transform.position.x;
                float headZPos = Camera.main.transform.position.z;

                carXPos.Add(instantiatedPrefab.transform.position.x);
                carYPos.Add(instantiatedPrefab.transform.position.y);
                carZPos.Add(instantiatedPrefab.transform.position.z);
                carAcceleration.Add(accelSelected);
                carVelocity.Add(speed);
                
                float carDist = headXPos - currentPosition;
                carDistance.Add(carDist);

                float distVectorVal = ComputeVisualDistance(headXPos, headZPos, currentPosition);
                distanceVector.Add(distVectorVal);

                float speed2 = ComputeVelocityVector(speed, carDist, distVectorVal);
                velocityVector.Add(speed2);
                
                float pheta = ComputeVisualAngle(objectHeight, carDist);
                float pheta2 = ComputeVisualAngle(objectHeight, distVectorVal);
                visualAngle.Add(pheta);
                visualAngle2.Add(pheta2);

                float RCVAValue = ComputeRCVA(speed, pheta);
                float RCVAValue2 = ComputeRCVA(speed2, pheta2);
                RCVA.Add(RCVAValue);
                RCVA2.Add(RCVAValue2);

                tau.Add(ComputeTau(pheta, RCVAValue));
                tau2.Add(ComputeTau(pheta2, RCVAValue2));


                float currentAcceleration;
                if(speed == initVelSelected)
                {
                    isConstantVelocity = true;
                    currentAcceleration = 0;

                }
                else
                {
                    isConstantVelocity = false;
                    currentAcceleration = accelSelected;
                }

                if(isConstantVelocity)
                {
                    float phetaDoubleDots = ComputeRCVAPrime(speed, RCVAValue, pheta, currentAcceleration);
                    tauDot1.Add(ComputeTauDot(pheta, RCVAValue, phetaDoubleDots));

                }

                //If the car is decelerating, use tau-dot function for deceleration
                else
                {
                    tauDot1.Add(ComputeDecelTauDot(carDist, accelSelected, speed));
                }
                

            }

  
    }


    // Kinematic formula for current velocity: v = at + v0
    float CurrentVelocity(float acceleration, float initialVelocity, float time) 
    {
        velocity = (acceleration * time) + initialVelocity;
        return velocity;
    }


    float PositionToDecelerate()
    {
        positionToDecel = initVelSelected * initVelSelected / (2 * accelSelected);
        return positionToDecel;
    }

    void CarSpawner()
    {
        lastResetTime = Time.time;
        Debug.Log("------------------Trial: " + trialNum + " --------------------");
        time1 = Time.time;

        // Practice trials
        if(trialNum < 1)
        {
            if(availableKeysPractice.Count > 0)
            {
                // Generate a random index
                int randomIndex = new System.Random().Next(0, availableKeysPractice.Count);
                // Get a key at the random index
                int randomKey = availableKeysPractice[randomIndex];
                // Retrieve the set from the dictionary and update initial acceleration and velocity values.
                List<float> floatList = accel_Vel_Dictionary[randomKey].ToList();
                accelSelected = floatList[0];
                initVelSelected = floatList[1];

                // Remove the key from the available keys list
                availableKeysPractice.RemoveAt(randomIndex);                                
                
            }
        }
        else
        {
            if(availableKeys.Count > 0)
            {
                int limits = totalTrials / accel_Vel_Dictionary.Count;
                // Generate a random index
                int randomIndex = new System.Random().Next(0, availableKeys.Count);
                // Get a key at the random index
                int randomKey = availableKeys[randomIndex];
                // Retrieve the set from the dictionary and update initial acceleration and velocity values.
                List<float> floatList = accel_Vel_Dictionary[randomKey].ToList();
                accelSelected = floatList[0];
                initVelSelected = floatList[1];
                // Update observation count for the selected key
                observationCount[randomKey]++;
                

                if(observationCount[randomKey] >= limits)
                {
                    // Remove the key from the available keys list
                    availableKeys.RemoveAt(randomIndex);
                }                                
                
            }
        }

        Debug.Log("Selected Deceleration: " + accelSelected);
        Debug.Log("Selected Velocity: " + initVelSelected);
        
        trialNum++;

        // A new car will be instantiated at selected initial position
        instantiatedPrefab = Instantiate(prefab, new Vector3(carStartingPosition, transform.position.y, 0), quaternion.identity);
        isCarCreated = true;

        leftTriggerIsPressed = false;
        rightTriggerIsPressed = true;

        carCreation.RemoveAt(carCreation.Count - 1);
        carCreation.Add(1);

        trialNumber.RemoveAt(trialNumber.Count - 1);
        trialNumber.Add(trialNum - 1);

        isDecelerated = true;
        willBeDestroyed = false;
        isInitialMovement = true;
        
        //Debug.Log("Car is instantiated");

        if(accelSelected == 0)
        {
            zeroAcceleration = true;
        }

    }

    float ComputeVisualAngle(float objectHeight, float distance)
    {
        return 2 * Mathf.Atan(objectHeight / (2 * distance));
    }


    float ComputeVisualDistance(float headX, float headZ, float carX)
    {
        float carZPos = instantiatedPrefab.transform.position.z;
        float xDistance = headX - carX;
        float zDistance = headZ - carZPos;
 
        
        return (float) Math.Sqrt((xDistance * xDistance) + (zDistance * zDistance));
    }


    float ComputeVelocityVector(float velVal, float dist1, float dist2)
    {
        return (velVal * dist2 / dist1);
    }

    float ComputeTau(float phetaVal, float RCVA)
    {
        return phetaVal / RCVA;
    }


    float ComputeRCVA(float velVal, float phetaVal)
    {
        return((-2 * velVal * Mathf.Pow(Mathf.Sin(phetaVal/2), 2)) / (objectHeight / 2));
    }


    float ComputeRCVAPrime(float velVal, float rcvaVal, float phetaVal, float accelVal)
    {
        return((-2 * (accelVal * Mathf.Pow(Mathf.Sin(phetaVal/2), 2) + velVal * rcvaVal * Mathf.Sin(phetaVal / 2) * Mathf.Cos(phetaVal / 2))) / (objectHeight / 2));
    }

    float ComputeTauDot(float phetaVal, float RCVAVal, float RCVAPrimeVal)
    {
        return ((Mathf.Pow(RCVAVal,2) - (phetaVal * RCVAPrimeVal)) / Mathf.Pow(RCVAVal, 2));
    }
    float ComputeDecelTauDot(float dist, float accel, float vel)
    {
        return(-(1 + (dist * accel / Mathf.Pow(vel, 2))));
    }


    public float GetSpeed()
    {
        return speed;
    }

}
