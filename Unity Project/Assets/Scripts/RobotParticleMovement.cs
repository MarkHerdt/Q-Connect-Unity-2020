using UnityEngine;

// Moves instantiated particle with robots
public class RobotParticleMovement : MonoBehaviour
{
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb.velocity = new Vector3(RobotBehaviour.speed, transform.position.y, transform.position.z);    
    }
}
