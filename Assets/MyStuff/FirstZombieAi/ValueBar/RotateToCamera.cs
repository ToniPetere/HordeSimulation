using UnityEngine;

public class RotateToCamera : MonoBehaviour
{
    private void FixedUpdate()
    {
        this.gameObject.transform.rotation = Camera.main.transform.rotation;
    }
}
