using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerWithAnimation : MonoBehaviour
{
    [SerializeField] float runSpeed = 2.0f;
    [SerializeField] float jumpSpeed = 4.0f;
    public float rotationSpeedMultiplier = 1.0f;

    float moveX;
    float moveY;
    Rigidbody rgbd;

    Animator myAnimator;
    CapsuleCollider myCapsuleCollider;
    float gravityScaleAtStart;

    void Start()
    {
        rgbd = GetComponent<Rigidbody>();
        myAnimator = GetComponent<Animator>();
        myCapsuleCollider = GetComponent<CapsuleCollider>();
        gravityScaleAtStart = rgbd.useGravity ? Physics.gravity.y : 0f;
    }

    void Update()
    {
        Run();
    }

    void OnMoveX(InputValue value)
    {
        moveX = value.Get<float>();
        Debug.Log(moveX);
    }

    void OnMoveY(InputValue value)
    {
        moveY = value.Get<float>();
        Debug.Log(moveY);
    }

    void Run()
    {
        Vector3 playerVelocity = new Vector3(moveX * runSpeed, 0f, moveY * runSpeed);
        rgbd.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(rgbd.velocity.x) > Mathf.Epsilon;
        bool playerHasVerticalSpeed = Mathf.Abs(rgbd.velocity.z) > Mathf.Epsilon;

        myAnimator.SetFloat("Speed", (playerHasHorizontalSpeed || playerHasVerticalSpeed) ? Mathf.Sqrt(moveX * moveX + moveY * moveY) : 0f);

        float rotationSpeed = moveX * rotationSpeedMultiplier; // Set a rotationSpeedMultiplier value as per your requirements
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }

    void OnJump(InputValue value)
    {
        if (!IsGrounded()) { return; }
        if (value.isPressed)
        {
            rgbd.velocity += new Vector3(0f, jumpSpeed, 0f);
        }
    }

    bool IsGrounded()
    {
        float extraHeightText = 0.1f;
        bool isGrounded = Physics.Raycast(myCapsuleCollider.bounds.center, Vector3.down, myCapsuleCollider.bounds.extents.y + extraHeightText, LayerMask.GetMask("Ground"));
        return isGrounded;
    }
}
