using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private float baseMouseSensitivity = 1f;
    [SerializeField] private ScriptableObject_OptionSettings soOptionSettings;
    private float calculatedMouseSensetivity;

    private float xRotation;
    [SerializeField] private Transform cameraTransform;
    private void Start()
    {
        UpdateSensetivity();
        soOptionSettings.PlayerCameraScript = this;
    }
    void FixedUpdate()
    {
        MoveCamera();
    }
    private void MoveCamera()
    {
        Vector2 playerMouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        xRotation -= playerMouseInput.y * calculatedMouseSensetivity; // Bei Oben/Unten Bewegungen wird die Kamera rotiert
        transform.Rotate(0, playerMouseInput.x * calculatedMouseSensetivity, 0); // Bei Links-/Rechtsbewegungen wird der Spieler Rotiert(nur an der y-Achse)
        xRotation = Mathf.Clamp(xRotation, -85, 85);
        cameraTransform.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
    }
    public void UpdateSensetivity()
    {
        // In den Einstellungen wird der Multiplikator für die Empfindlichkeit eingestellt
        calculatedMouseSensetivity = baseMouseSensitivity * ((soOptionSettings.MouseSensitivity * 3) + 1); // Komische Umrechnung, weil ich einen Wert zwischen 0 und 1 im Slider einstellen lassen möchte
        //Debug.Log("Mouse Sensetivity Updated!");
    }
}
