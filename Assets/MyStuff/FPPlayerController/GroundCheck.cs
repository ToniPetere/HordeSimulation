using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private bool isOnGround = false;
    public bool IsOnGround { get => isOnGround; }

    private void OnTriggerStay(Collider other)
    {
        if(other.transform.tag == "Ground")
            isOnGround = true;
    }
    

    private void OnTriggerExit(Collider other)
    {
        isOnGround = false;
    }
}
