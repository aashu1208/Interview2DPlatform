using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class PlayerController : MonoBehaviour
    {
        public float movingSpeed;
        public float jumpForce;
        public int maxJumps = 2;  // Maximum number of jumps allowed
        private int jumpsRemaining; // Number of jumps remaining

        private float moveInput;

        private bool facingRight = false;
        [HideInInspector]
        public bool deathState = false;

        private bool isGrounded;
        public Transform groundCheck;

        private Rigidbody2D rb;
        private Animator animator;
        private GameManager gameManager;

        private AudioSource audioSource;
        public AudioClip coinCollectSound;
        public AudioClip jumpSound;
        public AudioClip doubleJumpSound;
        public AudioClip deathSound;
        

        public GameObject deathPlayerPrefab;
        public GameObject playerGameObject;

        private bool isLevelCompleted = false;  // New variable to track level completion

        public GameObject levelCompletePopup;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();

            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

            // Initialize jumpsRemaining to maxJumps
            jumpsRemaining = maxJumps;
        }

        private void FixedUpdate()
        {
            CheckGround();
        }

        void Update()
        {
            if (Input.GetButton("Horizontal"))
            {
                moveInput = Input.GetAxis("Horizontal");
                Vector3 direction = transform.right * moveInput;
                transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, movingSpeed * Time.deltaTime);
                animator.SetInteger("playerState", 1); // Turn on run animation
                
            }
            else
            {
                if (isGrounded) animator.SetInteger("playerState", 0); // Turn on idle animation
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isGrounded || jumpsRemaining > 0)
                {
                    rb.velocity = new Vector2(rb.velocity.x, 0f); // Reset vertical velocity before jump
                    rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);

                    if (!isGrounded)
                    {
                        PlaySound(doubleJumpSound);
                        jumpsRemaining--; // Decrease jumpsRemaining on double jump
                    }
                    else
                    {
                        PlaySound(jumpSound);
                        jumpsRemaining = maxJumps - 1; // Reset jumpsRemaining on initial jump
                    }
                }
            }

            if (!isGrounded) animator.SetInteger("playerState", 2); // Turn on jump animation

            if (facingRight == false && moveInput > 0)
            {
                Flip();
            }
            else if (facingRight == true && moveInput < 0)
            {
                Flip();
            }

            // Reset jumpsRemaining when grounded
            if (isGrounded)
            {
                jumpsRemaining = maxJumps;
            }

            if (gameManager.coinsCounter == gameManager.totalCoins)
            {
                if (!isLevelCompleted)
                {
                    // Execute level completion logic (e.g., show popup, activate completion screen)
                    LevelCompleted();
                }
            }
        }

        private void Flip()
        {
            facingRight = !facingRight;
            Vector3 Scaler = transform.localScale;
            Scaler.x *= -1;
            transform.localScale = Scaler;
        }

        private void CheckGround()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.transform.position, 0.2f);
            isGrounded = colliders.Length > 1;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.tag == "Enemy")
            {
                PlaySound(deathSound);
                Debug.Log("Hitting enemy");
                deathState = true; // Say to GameManager that player is dead
                Debug.Log("Playing death Sound");
            }
            else
            {
                deathState = false;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "Coin")
            {
                gameManager.coinsCounter += 1;
                Destroy(other.gameObject);
                PlaySound(coinCollectSound);
            }
        }

        public void PlaySound(AudioClip clip)
        {
            if (clip != null)
            {
                audioSource.clip = clip;
                audioSource.Play();
                Debug.Log("Playing sound: " + clip.name);
            }
            else
            {
                Debug.LogWarning("Audio clip is null. Make sure it's assigned in the Unity Editor.");
            }
        }


        private void LevelCompleted()
        {
            Time.timeScale = 0;

            isLevelCompleted = true;  // Set the flag to prevent repeated execution

            // Add your code here to show a popup, activate a completion screen, or any other logic.
            // Example: Activate a GameObject with LevelCompletePopup component
            levelCompletePopup.SetActive(true);

        }
    }
}
