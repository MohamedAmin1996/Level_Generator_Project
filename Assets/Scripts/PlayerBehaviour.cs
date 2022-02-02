using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float jumpForce;

    private Rigidbody2D rb;
    private PlayerAudio playerAudio;
    private Vector2 input;
    [SerializeField] Animator anim;

    private RaycastHit2D isGrounded;
    private RaycastHit2D isGroundedLeft;
    private RaycastHit2D isGroundedRight;
    [SerializeField] LayerMask whatIsGround;
    [SerializeField] Transform feetPos;
    [SerializeField] float RaycastDistance;

    [SerializeField] float jumpTime;
    private float jumpTimeCounter;
    private bool isJumping;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Invoke("Gravity", 2.5f);
        playerAudio = GetComponent<PlayerAudio>();
    }

    private void FixedUpdate()
    {
        GetInput();
        Move();
    }

    private void Update()
    {
        Flip();
        AnimatorSettings();
        Jump();
    }

    void Gravity()
    {
        rb.simulated = true;
    }


    void GetInput()
    {
        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        input = Vector2.ClampMagnitude(input, 1);
    }

    void Flip()
    {
        if (input.x > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (input.x < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }

    private void AnimatorSettings()
    {
        if (input.x > 0.05f || input.x < -0.05f)
        {
            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }

        if (isGrounded.collider == null || isGroundedLeft.collider == null || isGroundedRight.collider == null)
        {
            anim.SetBool("isJumping", true);
        }

        if (isGrounded.collider != null || isGroundedLeft.collider != null || isGroundedRight.collider != null)
        {
            anim.SetBool("isJumping", false);
        }
    }
    void Move()
    {
        rb.velocity = new Vector2(input.x * speed, rb.velocity.y);
    }

    void Jump()
    {
        Vector2 offset = new Vector2(0.4f, 0f);

        isGrounded = Physics2D.Raycast(feetPos.position, Vector2.down, RaycastDistance, whatIsGround);

        isGroundedLeft = Physics2D.Raycast((Vector2)feetPos.position + (offset * -1), Vector2.down, RaycastDistance, whatIsGround);

        isGroundedRight = Physics2D.Raycast((Vector2)feetPos.position + (offset), Vector2.down, RaycastDistance, whatIsGround);

        if ((isGrounded.collider != null || isGroundedLeft.collider != null || isGroundedRight.collider != null)  && Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rb.velocity = Vector2.up * jumpForce;
            playerAudio.JumpSound();
        }

        if (Input.GetKey(KeyCode.Space) && isJumping == true)
        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity = Vector2.up * jumpForce;
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }
    }

    private void OnDrawGizmos()
    {
        Vector2 offset = new Vector2(0.2f, 0f);
        Debug.DrawRay(feetPos.position, Vector2.down * RaycastDistance, Color.red);
        Debug.DrawRay((Vector2)feetPos.position + (offset * -1), Vector2.down * RaycastDistance, Color.green);
        Debug.DrawRay((Vector2)feetPos.position + (offset), Vector2.down * RaycastDistance, Color.blue);
    }
}
