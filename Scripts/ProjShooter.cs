using System.Collections;
using UnityEngine;

// Used to shoot out pipes and pages towards the player
// Also used on the boss to shoot out projectiles to stun the player (no jumping)
public class ProjShooter : MonoBehaviour
{
    // Ref to projectile that should be shot, could be multiple to choose from
    [SerializeField] private GameObject[] proj;

    // Used to dictate speed/direction proj gets shot at
    [SerializeField] private float projForce;
    
    // Fields used to shoot the proj in random value between min and max values
    [SerializeField] private float minTime, maxTime;

    // Destroys proj after a certain amount of time
    [SerializeField] private float destroyTime;

    // Used to indicate if gravity should be random (calculates value between curr gravity and curr gravity * -1)
    [SerializeField] private bool shouldRandomizeGravity;

    // Start is called at the beginning to start shooting projectiles
    void Start()
    {
        // Calls proj shoot method 
        StartCoroutine(ShootProj());
    }

    // Shoots a projectile after a delay
    IEnumerator ShootProj()
    {
        // Delay
        yield return new WaitForSeconds(Random.Range(minTime, maxTime));

        // Spawn random proj from array and add force to it
        GameObject projRef = Instantiate(proj[Random.Range(0, proj.Length)], transform.position, transform.rotation);
        projRef.GetComponent<Rigidbody2D>().AddForce(transform.right * projForce, ForceMode2D.Force);

        // Randomizes strength of proj gravity if allowed
        if(shouldRandomizeGravity)
        {
            projRef.GetComponent<Rigidbody2D>().gravityScale = Random.Range(projRef.GetComponent<Rigidbody2D>().gravityScale * -1, 
                                                                projRef.GetComponent<Rigidbody2D>().gravityScale);
        }

        // Destroys proj after a delay
        Destroy(projRef, destroyTime);

        // Recursive call 
        StartCoroutine(ShootProj());
    }
}
