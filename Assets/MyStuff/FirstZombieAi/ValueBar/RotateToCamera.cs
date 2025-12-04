using UnityEngine;

public class RotateToCamera : MonoBehaviour
{
    private void Update()
    {
        this.gameObject.transform.rotation = Camera.main.transform.rotation;
    }
}
