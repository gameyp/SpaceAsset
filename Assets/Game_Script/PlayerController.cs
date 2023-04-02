using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 0.6f;
    public float turnSpeed = 180.0f;
    public float runMultiplier = 1.3f;
    public float gravity = 9.81f;
    public float jumpHeight = 1.5f;
    public int maxHealth = 100;
    public int currentHealth;

    public HealthBar healthBar;

    private PlayerInput playerInput;
    private CharacterController characterController;
    private Animator animator;

    private Vector2 currentMovementInput;
    private Vector3 currentMovement;
    private bool isMovementPressed;
    private bool isRunPressed;
    private float currentRun = 1.0f;
    private Vector2 mouseDelta;
    private bool groundedPlayer;

    private int jumpCount = 0; // Add this variable for double jump
    public int maxJumpCount = 2; // Add this variable to set the maximum number of jumps
    private float groundedTolerance = 0.05f; // Add a new variable for grounded tolerance

    private bool isAiming; // Add this variable for aiming
    private bool isSwordAttacking; // Add this variable for sword attacking
    private bool isDoubleJumping;
    private float timeSinceLastJump = 0.0f; // Add this variable for delay between jumps
    private float timeSinceLastGrounded = 0.0f;
    private float jumpCooldown = 0.2f; // Add this variable for jump cooldown

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void Awake()
    {
        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        playerInput.CharacterControls.Move.started += onMovementInput;
        playerInput.CharacterControls.Move.canceled += onMovementInput;
        playerInput.CharacterControls.Move.performed += onMovementInput;
        playerInput.CharacterControls.Run.started += onRun;
        playerInput.CharacterControls.Run.canceled += onRun;
        playerInput.CharacterControls.Look.performed += onLook;
        playerInput.CharacterControls.Jump.started += OnJump; // Add this line to handle jump input
        playerInput.CharacterControls.Aim.started += OnAim; // Add this line to handle aim input
        playerInput.CharacterControls.Aim.canceled += OnAim; // Add this line to handle aim input
        playerInput.CharacterControls.Shoot.started += OnShoot; // Add this line to handle shoot input
        playerInput.CharacterControls.SwordAttack.started += OnSwordAttack; // Add this line to handle sword attack input
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            // Load the main menu scene
            SceneManager.LoadScene("MenuScene");
        }
    }


    void OnAim(InputAction.CallbackContext context)
    {
        isAiming = context.ReadValueAsButton();
        animator.SetBool("isAiming", isAiming);
    }

    void OnJump(InputAction.CallbackContext context)
    {
        if (groundedPlayer && Time.time - timeSinceLastJump > jumpCooldown)
        {
            jumpCount = 1;
            currentMovement.y = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);
            animator.SetTrigger("isJumping");
            timeSinceLastJump = Time.time;
        }
        else if (jumpCount == 1 && Time.time - timeSinceLastJump > jumpCooldown)
        {
            jumpCount = 2;
            currentMovement.y = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);
            animator.SetBool("isDoubleJumping", true);
            timeSinceLastJump = Time.time;
        }
    }

    void OnShoot(InputAction.CallbackContext context)
    {
        animator.SetTrigger("isShooting");
    }

    void OnSwordAttack(InputAction.CallbackContext context)
    {
        if (isAiming)
        {
            isSwordAttacking = true;
            animator.SetTrigger("isSwordAttacking");
        }
        else
        {
            isSwordAttacking = false;
        }
    }

    void onRun(InputAction.CallbackContext context)
    {
        isRunPressed = context.ReadValueAsButton();
        currentRun = isRunPressed ? runMultiplier : 1.0f;
    }

    void onMovementInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
    }

    void onLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    void handleRotation()
    {
        if (mouseDelta != Vector2.zero)
        {
            float turnAngle = mouseDelta.x * turnSpeed * Time.deltaTime * 3;
            transform.Rotate(0, turnAngle, 0);
        }
    }

    void handleAnimation()
    {
        animator.SetBool("isWalking", isMovementPressed);
        animator.SetBool("isRunning", isMovementPressed && isRunPressed);
    }

    void Update()
    {
        handleRotation();
        handleAnimation();
        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(10);
        }

    }

    void FixedUpdate()
    {
        float speed = moveSpeed * currentRun;
        Vector2 currentMovementInput = this.currentMovementInput;

        Camera mainCamera = Camera.main;
        Vector3 cameraForward = Vector3.Scale(mainCamera.transform.forward, new Vector3(1, 0, 1)).normalized;
        currentMovement = transform.TransformDirection(new Vector3(currentMovementInput.x, 0, currentMovementInput.y)) * speed;

        groundedPlayer = characterController.isGrounded;

        if (groundedPlayer)
        {
            timeSinceLastGrounded = 0.0f;
        }
        else
        {
            timeSinceLastGrounded += Time.fixedDeltaTime;
        }

        characterController.Move(currentMovement * Time.fixedDeltaTime);

        // Reset mouseDelta
        mouseDelta = Vector2.zero;
    }

    void OnEnable()
    {
        playerInput.CharacterControls.Enable();
    }

    void OnDisable()
    {
        playerInput.CharacterControls.Disable();
    }
}
