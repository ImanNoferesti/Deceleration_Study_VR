using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelRotation : MonoBehaviour
{
    
    Deceleration_V1 decel;
    float speed;

    private void Start() 
    {
        decel = GameObject.Find("DataManager").GetComponent<Deceleration_V1>();    

    }
    private void Update()
    {
        speed = decel.GetSpeed();
        transform.Rotate(Vector3.back * 30 * speed * Time.deltaTime);
    }
}
