using UnityEngine;

// Behaviour for each robot
public class RobotBehaviour : MonoBehaviour
{
    public static float initialSpeed = 50f;
    public static float speed = 50f; // Speed the roboter moves with

    int missingParts = 0;
    /// <summary>
    /// Count of how many parts are missing on a robot
    /// </summary>
    public int MissingParts
    {
        get
        {
            return missingParts;
        }
        set
        {
            missingParts = value;
        }
    }

    int failedRepairAttempts = 0;
    /// <summary>
    /// Attempts the player has, selecting a part thats already on the robot
    /// </summary>
    public int FailedRepairAttempts
    {
        get
        {
            return failedRepairAttempts;
        }
        set
        {
            failedRepairAttempts = value;
        }
    }
    int maxFailedRepairAttempts = 2;
    /// <summary>
    /// Maximum number of failed attempts, before the player can't repair this robot anymore
    /// </summary>
    public int MaxFailedRepairAttempts { get => maxFailedRepairAttempts; }

    public bool failedToRepair = false; // Is being set in the "RepairRobot" script, when a robot has been failed to be repaired
    public RobotRepairCollider repairTrigger; // Reference to the "RepairRobot" script

    private void FixedUpdate()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Destroys the roboter when it's out of screen
        if (collision.tag == "Border")
        {
            // Substracts one life point when a robot has been failed to be repaired and leaves the screen
            if (repairTrigger != null)
            {
                repairTrigger.life.SubstractLife();
            }
            Destroy(gameObject);
        }
    }
}
