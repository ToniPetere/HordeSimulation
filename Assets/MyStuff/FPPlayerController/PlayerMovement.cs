using UnityEngine;

[RequireComponent (typeof(CharacterController))]

public class PlayerMovement : MonoBehaviour
{
    [Header("Move Values")]
    [SerializeField] private float walkSpeed = 6f;
    [SerializeField] private float runSpeed = 12f;


    [Header("Jump Values")]
    [SerializeField] private float jumpPower = 7f;
    [SerializeField] private float gravity = 10f;
    [SerializeField] private GroundCheck groundCheck;

    [Header("Look Values")]
    [SerializeField] private float lookSpeed = 2f;
    [SerializeField] private float lookXLimit = 45f;

    [Header("Others")]
    [SerializeField] private Camera playerCamera;
    // [SerializeField] private float friction = 0.9f; Old Value

    private Vector3 moveDirection = Vector3.zero;
    private bool canMove = true;
    private float rotationX = 0f;

    private CharacterController characterController;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        //Movement
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0f;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0f;
        float movementDirextionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        //Jumping
        if(Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
        }
        else
        {
            moveDirection.y = movementDirextionY;
        }

        if(!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        //CameraRotation
        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove)
        {
            rotationX += Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(-rotationX, 0f, 0f);
            transform.rotation *= Quaternion.Euler(0f, Input.GetAxis("Mouse X") * lookSpeed, 0f);
        }

    }

    // My old logic -> Maybe is even better

    //void FixedUpdate()
    //{

    //    if (Input.GetKey(KeyCode.Space) && groundCheck.IsOnGround)
    //    {
    //        Jump();
    //    }
    //}

    //private void Jump()
    //{
    //    // Bei jedem Sprung die y geschwindigkeit wieder auf Null setzen und dann die AddForce Methode anwenden -> Dadurch wird ein Exponentielles beschleunigen an z.B. Kanten verhindert
    //    playerRigidbody.linearVelocity = new Vector3(playerRigidbody.linearVelocity.x, 0, playerRigidbody.linearVelocity.z);
    //    playerRigidbody.AddForce(new Vector3(0,JumpForce,0));
    //}

}
