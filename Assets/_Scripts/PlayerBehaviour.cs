using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBehaviour : MonoBehaviour
{
    public Joystick joystick;
    public float joystickHorizontalSensitivity;
    public float joystickVerticalSensitivity;
    public float horizontalForce;
    public float verticalForce;
    public bool isGrounded;
    public bool isJumping;
    public bool isCrouching;
    public Transform spawnPoint;

    public int health;
    public int lives;
    public HealthBarController healthBar;
    public Animator HealthBarAnimator;

    private Rigidbody2D m_rigidBody2D;
    private SpriteRenderer m_spriteRenderer;
    private Animator m_animator;

    // Start is called before the first frame update
    void Start()
    {
        health = 100;
        lives = 3;

        m_rigidBody2D = GetComponent<Rigidbody2D>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        _Move();
    }

    void _Move()
    {
        if (isGrounded)
        {
            if (!isJumping && !isCrouching)
            {
                if (joystick.Horizontal > joystickHorizontalSensitivity)
                {
                    // move right
                    m_rigidBody2D.AddForce(Vector2.right * horizontalForce * Time.deltaTime);
                    m_spriteRenderer.flipX = false;
                    m_animator.SetInteger("AnimState", (int)PlayerMovementType.RUN);
                }
                else if (joystick.Horizontal < -joystickHorizontalSensitivity)
                {
                    // move left
                    m_rigidBody2D.AddForce(Vector2.left * horizontalForce * Time.deltaTime);
                    m_spriteRenderer.flipX = true;
                    m_animator.SetInteger("AnimState", (int)PlayerMovementType.RUN);
                }
                else if (!isJumping)
                {
                    m_animator.SetInteger("AnimState", (int)PlayerMovementType.IDLE);
                }
            }

            //Debug.Log(isCrouching);
            if ((joystick.Vertical < -joystickVerticalSensitivity) && (!isCrouching))
            {
                m_animator.SetInteger("AnimState", (int)PlayerMovementType.CROUCH);
                isCrouching = true;
            }
            else if(joystick.Vertical > -joystickVerticalSensitivity)
            {
                isCrouching = false;
            }
            
            if ((joystick.Vertical > joystickVerticalSensitivity) && (!isJumping))
            {
                // jump
                Jump();
            }
            else
            {
                isJumping = false;
            }
        }

    }

    public void Jump()
    {
        if (!isJumping)
        {
            m_rigidBody2D.AddForce(Vector2.up * verticalForce);
            m_animator.SetInteger("AnimState", (int)PlayerMovementType.JUMP);
            isJumping = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        isGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        isGrounded = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // respawn
        if (other.gameObject.CompareTag("DeathPlane"))
        {
            LoseLife();
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(15);
        }
    }

    public void LoseLife()
    {
        lives -= 1;
        HealthBarAnimator.SetInteger("LivesAnimState", lives);

        if (lives > 0)
        {
            health = 100;
            healthBar.SetValue(health);
            transform.position = spawnPoint.position;
        }
        else
        {
            SceneManager.LoadScene("End");
        }
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        healthBar.SetValue(health);

        if(health <= 0)
        {
            LoseLife();
        }
    }
}
