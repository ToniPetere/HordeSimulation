using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] ScriptableObject_Bool soIsPauseMenuOpen;

    [Header("Move Values")]
    [SerializeField] private Rigidbody playerRigidbody;

    [SerializeField] float moveSpeed = 5f;


    [Header("Jump Values")]
    [SerializeField] float JumpForce = 50f;

    [SerializeField] GroundCheck groundCheck;
    [SerializeField] float friction = 0.9f;

    void FixedUpdate()
    {
        if (soIsPauseMenuOpen.Value == true) return;

        // schwerkraft deaktivieren wenn man auf dem Boden ist, damit man bei schrägen nicht runter rutscht?
        //if (groundCheck.IsOnGround == true) DisableGravity();
        //else EnableGravity();

        MovePlayer();

        if (Input.GetKey(KeyCode.Space) && groundCheck.IsOnGround)
        {
            Jump();
        }
    }


    private void MovePlayer()
    {
        //soll nur ausgeführt werden, wenn man W, A, S oder D drückt, damit der Spieler andernfalls wieder gebremst wird
        // -> friction in Unity ausgestellt, damit man nicht an Wänden hängen bleibt, wenn man in der Luft gegen sie läuft
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) 
        {
            Vector3 playerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        
            Vector3 lookDirection = transform.forward;
            Vector3 NextToTheCharacter = transform.right;
        
            //lookDirection und playerMovementInput.z sind nur für Vorne und Hinten zuständig(also W und S), NextToTheCharacter und playerMovementInput.x für Rechts und Links(A und D)
            Vector3 movement = (lookDirection * playerMovementInput.z + NextToTheCharacter * playerMovementInput.x).normalized;
            movement *= moveSpeed;
            playerRigidbody.velocity = new Vector3(movement.x, playerRigidbody.velocity.y, movement.z);
        }
        else
        {
            //wird jetzt auch in der Luft gebremst, sollte ggf geändert werden, falls nötig
            playerRigidbody.velocity = new Vector3 (playerRigidbody.velocity.x * friction, playerRigidbody.velocity.y, playerRigidbody.velocity.z * friction);
        }



        //Ist ein anderes Controller System aus dem Internet:

        //Vector3 PlayerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")); //Erhält den Input W,A,S,D mit der jeweiligen Richtung
        //Vector3 Movement = transform.TransformDirection(PlayerMovementInput * moveSpeed); //passt die Richtung an der Ausrichtung des Players an(W = vorne wo wir hingucken und nicht "Norden")
        //playerRigidbody.velocity = new Vector3(Movement.x, playerRigidbody.velocity.y, Movement.z);
    }

    private void Jump()
    {
        // Bei jedem Sprung die y geschwindigkeit wieder auf Null setzen und dann die AddForce Methode anwenden -> Dadurch wird ein Exponentielles beschleunigen an z.B. Kanten verhindert
        playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, 0, playerRigidbody.velocity.z);
        playerRigidbody.AddForce(new Vector3(0,JumpForce,0));
    }


    private void DisableGravity()
    {
        playerRigidbody.useGravity = false;
    }
    private void EnableGravity()
    {
        playerRigidbody.useGravity = true;
    }
}
