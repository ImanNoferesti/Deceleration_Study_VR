using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelRotationRef : MonoBehaviour
{
    public Transform wheelReference;

    private void Update()
    {
        transform.SetParent(wheelReference);

    }
}
