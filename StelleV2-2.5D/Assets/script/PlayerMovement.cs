using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;
    private Animator animator;
    private float inputX, inputY;
    private float stopX, stopY;
    private bool isPickingUp; // Whether currently picking up
    private bool isAttacking; // Whether currently attacking
    public bool canPickUp = false;

    public int A, B, C,D;

    // private int books; // Counter variable
    // public Text BooksText; // UI component for scroll counting

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Get input from WASD keys
        inputX = Input.GetKey(KeyCode.D) ? 1 : (Input.GetKey(KeyCode.A) ? -1 : 0);
        inputY = Input.GetKey(KeyCode.W) ? 1 : (Input.GetKey(KeyCode.S) ? -1 : 0);
        
        // Try to get input from ChannelScript if available
        try {
            ChannelScript channelScript = GameObject.Find("Player")?.GetComponent<ChannelScript>();
            if (channelScript != null) {
                A = channelScript.A;
                B = channelScript.B;
                C = channelScript.C;
                D = channelScript.D;
                
                // If there's input from the channel, override keyboard input
                if (A != 0 || B != 0) {
                    inputX = A - 1;
                    inputY = B - 1;
                }
            }
        } catch (System.Exception) {
            // If there's an error with ChannelScript, just use keyboard input
        }
        
        Attack();
        
        // Move the character based on input
        Vector2 input = new Vector2(inputX, inputY).normalized;
        rb.velocity = input * speed;
        
        // Rotate to face mouse position if mouse is being used
        if (Input.GetMouseButton(0)) {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = transform.position.z;
            Vector3 direction = (mousePosition - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        if (input != Vector2.zero)
        {
            animator.SetBool("isMoving", true);
            stopX = inputX;
            stopY = inputY;

        }
        else
        {
            animator.SetBool("isMoving", false);
        }

        // Handle picking up action - Use E key, Space, or channel input
        if (canPickUp && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space) || C == 1))
        {
            isPickingUp = true;
            animator.SetTrigger("isPickingUp");
            Debug.Log("Picking up item");
            
            // Calculate facing direction and set PickDirection
            Vector2 foodPosition = GetClosestFoodPosition();
            float direction = transform.position.x > foodPosition.x ? -1 : 1;

            animator.SetFloat("PickDirection", direction);
            SoundManager.PlayHealSpellClip(); // Play health recovery audio
            
            // Reset C value if using ChannelScript
            try {
                ChannelScript channelScript = GameObject.Find("Player")?.GetComponent<ChannelScript>();
                if (channelScript != null && C == 1) {
                    channelScript.C = 0;
                }
            } catch (System.Exception) {
                // If there's an error with ChannelScript, just continue
            }
        }

        animator.SetFloat("InputX", stopX);
        animator.SetFloat("InputY", stopY);

    }

    Vector2 GetClosestFoodPosition()
    {
        GameObject[] foods = GameObject.FindGameObjectsWithTag("food");
        float closestDistance = float.MaxValue;
        Vector2 closestPosition = Vector2.zero;
        foreach (var food in foods)
        {
            float distance = Vector2.Distance(transform.position, food.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPosition = food.transform.position;
            }
        }
        return closestPosition;
    }

    public void EndPickAni() // Call this function at the end of the pick animation
    {
        isPickingUp = false;
    }

    void Attack()
    {
        // Attack with right mouse button or from channel input
        if (Input.GetMouseButtonDown(1) || Input.GetButtonDown("Attack") || D == 1)
        {
            animator.SetTrigger("Attack");
            isAttacking = true;
            SoundManager.PlayPlayerAttackClip(); // Play attack audio
            
            // Reset D value if using ChannelScript
            try {
                ChannelScript channelScript = GameObject.Find("Player")?.GetComponent<ChannelScript>();
                if (channelScript != null && D == 1) {
                    channelScript.D = 0;
                }
            } catch (System.Exception) {
                // If there's an error with ChannelScript, just continue
            }
        }
    }

    public void AttackFinished()  // Call this function at the end of the attack animation
    {
        isAttacking = false;
    }

    public void EnablePickUp()
    {
        canPickUp = true;
    }

    public void DisablePickUp()
    {
        canPickUp = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (isAttacking)  // Only damage the enemy when the player is attacking
            {
                other.GetComponent<EnemyMovement>().TakeDamage(30);
            }
        }
    }
}
