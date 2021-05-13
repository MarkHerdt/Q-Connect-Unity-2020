using UnityEngine;
using System;

// Gives the robot the selected part
public class SelectedRobotPart : MonoBehaviour
{
    Collider2D collision; // Slot, the selector collides with
    GameObject robotPart; // RobotPart inside the slot UI

    public RobotRepairCollider trigger; // Robot inside the "RobotRepairCollider"
    public RobotParts part; // Reference to Scriptable object

    // Particle
    public GameObject lightning;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Saves the tag of the object the selector collides with in "this.collision"
        if (collision.gameObject.tag == "1")
        {
            this.collision = collision;
        }
        else if (collision.gameObject.tag == "2")
        {
            this.collision = collision;
        }
        else if (collision.gameObject.tag == "3")
        {
            this.collision = collision;
        }
        else if (collision.gameObject.tag == "4")
        {
            this.collision = collision;
        }
        else if (collision.gameObject.tag == "5")
        {
            this.collision = collision;
        }
        else if (collision.gameObject.tag == "6")
        {
            this.collision = collision;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // Displays the position, the selector currently is on
        if (collision.gameObject.tag == "1" || collision.gameObject.tag == "2" || collision.gameObject.tag == "3" || collision.gameObject.tag == "4" || collision.gameObject.tag == "5" || collision.gameObject.tag == "6")
        {
            collision.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 4;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Sets the "sortingOrder" to it's defaul value, when the selector leaves the slot
        if (collision.gameObject.tag == "1" || collision.gameObject.tag == "2" || collision.gameObject.tag == "3" || collision.gameObject.tag == "4" || collision.gameObject.tag == "5" || collision.gameObject.tag == "6")
        {
            collision.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            robotPart = collision.gameObject.transform.GetChild(0).gameObject; // Robotpart the selector collides with

            // Player can only make one mistake repairing a robot, after the second mistake the player can't repair that robot anymore
            if (trigger.Robot != null && trigger.Robot.GetComponent<RobotBehaviour>().FailedRepairAttempts < trigger.Robot.GetComponent<RobotBehaviour>().MaxFailedRepairAttempts)
            {
                // If the robot doesn't have the selected part
                if (trigger.Robot.transform.GetChild(Convert.ToInt32(robotPart.tag) - 1).GetComponent<SpriteRenderer>().sprite == null)
                {
                    // Gives the robot the selected part
                    trigger.Robot.transform.GetChild(Convert.ToInt32(robotPart.tag) - 1).GetComponent<SpriteRenderer>().sprite = part.partsArray[Convert.ToInt32(robotPart.tag) - 1];
                    Instantiate(lightning, new Vector3(trigger.Robot.transform.GetChild(Convert.ToInt32(robotPart.tag) - 1).transform.position.x, trigger.Robot.transform.GetChild(Convert.ToInt32(robotPart.tag) - 1).transform.position.y, trigger.Robot.transform.GetChild(Convert.ToInt32(robotPart.tag) - 1).transform.position.z - 1f), Quaternion.identity);
                    
                    SoundController.PlaySound(SoundController.Sound.Drill);

                    trigger.Robot.GetComponent<RobotBehaviour>().MissingParts--; // Decrements "MissingParts" for each successful repair
                    // When the robot has been fully repaired
                    if (trigger.Robot.GetComponent<RobotBehaviour>().MissingParts == 0)
                    {
                        trigger.SetIndicatorGreen();
                    }
                }
                // If the robot does have the selected part
                else if (trigger.Robot.transform.GetChild(Convert.ToInt32(robotPart.tag) - 1).GetComponent<SpriteRenderer>().sprite != null)
                {
                    trigger.Robot.GetComponent<RobotBehaviour>().FailedRepairAttempts++; // Increments "FailedRepairAttempts" for each failed attempt

                    // If the player has made the maximum number of failed attempts
                    if (trigger.Robot.GetComponent<RobotBehaviour>().FailedRepairAttempts >= trigger.Robot.GetComponent<RobotBehaviour>().MaxFailedRepairAttempts && trigger.Robot.GetComponent<RobotBehaviour>().MissingParts != 0)
                    {
                        trigger.SetIndicatorRed();
                    }
                }
            }
        }
    }
}
