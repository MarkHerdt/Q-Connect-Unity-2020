using UnityEngine;

// Collider in which the robots can be reapired
public class RobotRepairCollider : MonoBehaviour
{
    // Robot
    GameObject robot;
    /// <summary>
    /// Robot inside repair trigger
    /// </summary>
    public GameObject Robot { get => robot; }

    // References
    public GameController gameController;
    public LifeController life;
    public GameObject lightBeam;

    // Sprites
    SpriteRenderer[] lights;
    public SpriteRenderer greenLight;
    public SpriteRenderer redLight;
    public Sprite glowingHead;

    // For random life add
    int randomInt;
    int offset = 0;
    public int Offset { get => offset; }

    private void Awake()
    {
        lights = lightBeam.GetComponentsInChildren<SpriteRenderer>();

        randomInt = getRandomInt();
    }

    private void Start()
    {
        foreach (var item in lights)
        {
            item.gameObject.SetActive(false);
        }
        redLight.gameObject.SetActive(false);
        greenLight.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Robot")
        {
            greenLight.gameObject.SetActive(false);
            redLight.gameObject.SetActive(false);

            SoundController.PlaySound(SoundController.Sound.LightSwitch, .25f);
            
            // Activates the lightbeam, when the robot enters the "RobotRepairCollider"
            foreach (var item in lights)
            {
                item.gameObject.SetActive(true);
            }

            robot = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Deactivates the lightbeam, when the robot leaves the "RobotRepairCollider"
        foreach (var item in lights)
        {
            item.gameObject.SetActive(false);
        }

        if (collision.tag == "Robot" && gameController.GameActive)
        {
            bool substract = false;
            bool failedToRepair = false;
            
            for (int i = 0; i < robot.transform.childCount; i++)
            {
                if (robot.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite == null) // When the robot isn't fully repaired
                {
                    redLight.gameObject.SetActive(true);
                    greenLight.gameObject.SetActive(false);
                    if (substract == false && life.CurrentLife > 0) // Only substracts life once, even if multiple robot parts are missing
                    {
                        collision.GetComponent<RobotBehaviour>().failedToRepair = true;
                        collision.GetComponent<RobotBehaviour>().repairTrigger = this; // Passes a reference of this script to the "Robot" script
                        substract = true;
                    }
                    failedToRepair = true;
                }
                else if (i == robot.transform.childCount -1 && failedToRepair == false) // When the robot is fully repaired
                {
                    greenLight.gameObject.SetActive(true);
                    redLight.gameObject.SetActive(false);

                    // Adjust score
                    gameController.SetScore();

                    SoundController.PlaySound(SoundController.Sound.RobotRepaired, .25f, collision.transform.position);

                    robot.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = glowingHead;

                    // Gives the player one life point, if the score reaches "randomInt + offset"
                    if(gameController.Score == randomInt + offset)
                    {
                        offset += randomInt;
                        life.AddLife();
                        randomInt = getRandomInt();
                    }

                }
            }
            robot = null;
        }
    }

    /// <summary>
    /// Gives the player one life point, if the score reaches this value
    /// </summary>
    /// <returns></returns>
    int getRandomInt()
    {
        return Random.Range(10, 25);
    }

    /// <summary>
    /// Sets the repair indicator to red
    /// </summary>
    public void SetIndicatorRed()
    {
        redLight.gameObject.SetActive(true);
        greenLight.gameObject.SetActive(false);
    }

    /// <summary>
    /// Sets the repair indicator to green
    /// </summary>
    public void SetIndicatorGreen()
    {
        greenLight.gameObject.SetActive(true);
        redLight.gameObject.SetActive(false);
    }

    /// <summary>
    /// Resets the offset for the random life gain on game restart back to zero
    /// </summary>
    public void ResetOffset()
    {
        offset = 0;
    }
}
