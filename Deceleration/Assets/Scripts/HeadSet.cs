using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadSet : MonoBehaviour
{   
    [HideInInspector] public List<float> HeadXPos = new List<float>();
    [HideInInspector] public List<float> HeadYPos = new List<float>();
    [HideInInspector] public List<float> HeadZPos = new List<float>();
    [HideInInspector] public List<float> HeadXRot = new List<float>();
    [HideInInspector] public List<float> HeadYRot = new List<float>();
    [HideInInspector] public List<float> HeadZRot = new List<float>();
    [HideInInspector] public List<float> TotalTime = new List<float>();

    // Update is called once per frame
    void Update()
    {
        HeadXPos.Add(Camera.main.transform.position.x);
        HeadYPos.Add(Camera.main.transform.position.y);
        HeadZPos.Add(Camera.main.transform.position.z);
        
        HeadXRot.Add(Camera.main.transform.eulerAngles.x);
        HeadYRot.Add(Camera.main.transform.eulerAngles.y);
        HeadZRot.Add(Camera.main.transform.eulerAngles.z);

        TotalTime.Add(Time.time);

    }

}
