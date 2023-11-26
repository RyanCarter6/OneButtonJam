using System.Collections;
using UnityEngine;

// Used on the player to control movement/interactions with environment
public class PlayerController : MonoBehaviour
{
    // Ref to components on the player gameobject
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Animator animator;

    // Vars that affect the player's jump
    private bool isGrounded;
    private int gravityChangeCount;             // Counts the amount of times the player has changed gravity before touching the ground
    [SerializeField] private float jumpForce;     

    // Checks if the player should be able to jump, turned off during final cutscene
    bool canJump = true;

    // Vars used when player interacts with an item/projectile
    bool isPoisoned;
        //
    int pagesObtained;
        //
    [SerializeField] private float dashForce;
    bool isDashing;

    // Update is called once per frame to check for user input
    void Update()
    {
        // Checks for button press
        if(Input.GetButtonDown("Input"))
        {
            InputManager();
        }
    }

    // Handles if the player should jump or change gravity on button press
    void InputManager()
    {
        // Stops method from continuing if the player shouldn't jump
        if(!canJump)
        {
            return;
        }

        // If player is on the ground, jump
        if(isGrounded)
        {
            rb.AddForce(new Vector2(0, jumpForce * rb.gravityScale));
            isGrounded = false;
        }
        // if player is in the air, switch gravity
        else
        {
            // Checks if player has already changed gravity before landing
            if(gravityChangeCount < 2)
            {
                // Change player's gravity and flip player
                rb.gravityScale *= -1;
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * -1, transform.localScale.z);
                gravityChangeCount += 1;
            }
        }
    }

    // Called when the game should end, win or lose
    void GameEnd(bool didWin)
    {
        // Updates audio to stop song & play win/lose sound
        InterfaceHandler.i.OpenEndUI(didWin);

        // Updates audio to stop song & play win/lose sound
        InterfaceHandler.i.PlayEndSound(didWin);
    }

    // Transforms the player back to their original form, called when game is won
    void StartTransformAnimation()
    {
        // Stop time
        Time.timeScale = 0f;

        // Stops player from jumping/changing gravity during final cutscene
        canJump = false;

        // Sets player upright
        transform.localScale = new Vector3(1f, 1f, 1f);

        // Trigger animator to start animation/final cutscene
        animator.SetTrigger("Transform");
    }

    // Called at the end of the transform animation
    void EndOfAnimation()
    {
        // End the game with the player winning
        GameEnd(true);
    }

    // Handles interaction when the player gets hit by a boss projectile
    IEnumerator PoisonPlayer()
    {
        // Make jump/gravity have shorter/slower, visualize player being poisoned
        jumpForce /= 2f;
        rb.gravityScale /= 2f;
        sr.color = Color.green;

        // Delay until player returns to normal
        yield return new WaitForSeconds(1f);

        // Visualize player not posioned and return to normal movement
        sr.color = Color.white;
        jumpForce *= 2f;
        rb.gravityScale *= 2f;
        isPoisoned = false;
    }

    // Handles interaction when the player picks up a page
    void AddPage()
    {
        // Adds a page
        pagesObtained += 1;
        InterfaceHandler.i.UpdatePageText(pagesObtained);

        // If all the pages needed are obtained
        if(pagesObtained >= 5)
        {
            // Win game
            StartTransformAnimation();
        }
    }

    // Gives the player a speed boost, called on cheese collection
    IEnumerator SpeedBoost()
    {
        // Move player forward
        rb.velocity = new Vector2(dashForce, rb.velocity.y);

        // Delay until another speed boost can be active
        yield return new WaitForSeconds(1f);
        isDashing = false;
    }

    // Checks if the player has hit the boss projectile, page, cheese, or boss
    void OnTriggerEnter2D(Collider2D collider)
    {
        // Case for boss proj (impairs player)
        if(collider.gameObject.layer == 11)
        {
            // If player isn't already poisoned
            if(!isPoisoned)
            {
                // Start poison and destroy proj
                StartCoroutine(PoisonPlayer());
                isPoisoned = true;
                Destroy(collider.gameObject);
            }
        }
        // Case for page (possibly wins game)
        else if(collider.gameObject.layer == 10)
        {
            // Add a page and destroy the page gameobject
            AddPage();
            Destroy(collider.transform.parent.gameObject);     // page collider for the player is on the child, destroy parent
        }
        // Case for cheese (speed boost to player)
        else if (collider.gameObject.layer == 7 && !isDashing)
        {
            StartCoroutine(SpeedBoost());
            Destroy(collider.gameObject);
            isDashing = true;
        }
        // Case for boss (loses game)
        else if(collider.gameObject.layer == 9)
        {
            // Stop time
            Time.timeScale = 0f;

            // Stops player from jumping/changing gravity when game over
            canJump = false;

            // Lose game
            GameEnd(false);
        }
    }

    // Checks if the player is grounded
    void OnCollisionEnter2D(Collision2D collider)
    {
        // Checks if gameobject player collided with is the ground
        if(collider.gameObject.layer == 6)
        {
            isGrounded = true;
            gravityChangeCount = 0;
        }
    }

    // Checks if the player isn't grounded
    void OnCollisionExit2D(Collision2D collider)
    {
        // Checks if gameobject player leaves collision with is the ground
        if(collider.gameObject.layer == 6)
        {
            isGrounded = false;
        }
    }
}
