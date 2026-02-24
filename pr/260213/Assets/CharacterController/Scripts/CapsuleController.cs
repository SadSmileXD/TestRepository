using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.TextCore.Text;

public class CapsuleController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpHeight = 2f;
    [Range(0f, 1f)] public float stepOffset;

    private CharacterController controller;
    private Vector3 velocity;
    private Vector2 moveInput;
    private bool jumpInput;

    public float groundCheckDistance = 0.2f; // 레이를 쏠 거리
    public LayerMask groundLayer;           // 지면으로 인식할 레이어

    private InputAction moveAction;
    private InputAction jumpAction;

    private InputAction dashAction;
    void Awake()
    {
        controller = GetComponent<CharacterController>();
        moveAction = InputSystem.actions["Move"];
        jumpAction = InputSystem.actions["Jump"];
        dashAction= InputSystem.actions["Dash"];
    }

    void OnEnable()
    {
        moveAction.performed += OnMove;
        moveAction.canceled += MoveCancel;
        jumpAction.started += OnJump;
        jumpAction.canceled += OnJumpCanceled;
        dashAction.started += OnDash;
    }

    void OnDisable()
    {
        moveAction.performed -= OnMove;
        moveAction.canceled -= MoveCancel;
        jumpAction.started -= OnJump;
        jumpAction.canceled -= OnJumpCanceled;
        dashAction.started -= OnDash;
    }

    void Update()
    {
        controller.stepOffset = stepOffset;

        var grounded = IsGrounded();

        if (grounded)
        {
            velocity.y = -2f; // 땅에 붙어있도록 약간의 음수값을 준다
            if (jumpInput)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);
                jumpInput = false;
            }

        }
        else
        {
            velocity.y += Physics.gravity.y * Time.deltaTime; // 공중에서는 중력 가속도를 적용한다
        }
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * speed*Time.deltaTime);


        velocity.y+=Physics.gravity.y * Time.deltaTime; 
        controller.Move(velocity * Time.deltaTime);
    }
   
    private bool IsGrounded()
    {
        if (controller.isGrounded) return true;

        var ray=transform.position+Vector3.up * 0.1f; // 캐릭터의 발 위치에서 약간 위로 레이를 쏜다
        if (Physics.Raycast(ray, Vector3.down, out RaycastHit hit, groundCheckDistance, groundLayer)) return true;

        return false;
    }
    public void OnMove(InputAction.CallbackContext ctx)
    {
      
        moveInput = ctx.ReadValue<Vector2>();
    }

    void MoveCancel(InputAction.CallbackContext ctx)
    {
        moveInput = Vector2.zero;
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if(ctx.started)
        {
            jumpInput = true;
        }
    }
    private void OnJumpCanceled(InputAction.CallbackContext ctx)
    {
        if(velocity.y>0f)
                velocity *= .5f;
    }
    private void OnDash(InputAction.CallbackContext ctx)
    {
        if(!isDashing)
            StartCoroutine(CorDash());
    }

    [SerializeField]private bool isDashing=false;
    [SerializeField] private float dashSpeed = 20f;
    IEnumerator CorDash()
    {
       isDashing = true;
       float startTime= Time.time;
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        while (Time.time< startTime+0.2f)
        {
         
            controller.Move(move*dashSpeed*Time.deltaTime);
            yield return null;
        }
        isDashing=false;
    }
}
