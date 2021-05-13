using UnityEngine;

// Moves the selector through the individual robot parts
public class SelectorController: MonoBehaviour
{
    Light uiLight; // Reference to move the selector (doesn't necessary need to be a light)
    public GameObject[] slot; // Position of robot parts

    int maxHorizontal = 3; // Max columns in a row
    int horizontal = 1; // Current horizontal position
    int maxVertical = 2; // Max rows
    int vertical = 1; // Current vertical position

    // References
    public GameController gameController;
    public MenuController menuController;

    private void Awake()
    {
        uiLight = GetComponentInChildren<Light>();
    }

    private void Start()
    {

        // Starting position of selector
        horizontal = 2;
        uiLight.transform.parent.position = new Vector2(slot[horizontal - 1].transform.position.x, slot[vertical + 1].transform.position.y);
    }

    private void Update()
    {
        // Only lets the player move the selector if the game is active and not paused
        if (gameController.GameActive && !menuController.GameIsPaused)
        {
            // Move the selector with "W" "A" "S" "D" or the arrow keys
            // Moves the selector to the left
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                // If the selector is on the first position of the row, move it to the rows last position
                // Else, move the selector one position to the left
                if (horizontal <= 1)
                {
                    horizontal = maxHorizontal;
                }
                else
                    horizontal--;
            }
            // Moves the selector to the right
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                // If the selector is on the last position of the row, move it to the rows first position
                // Else, move the selector one position to the right
                if (horizontal >= maxHorizontal)
                {
                    horizontal = 1;
                }
                else
                    horizontal += 1;
            }
            // Moves the selector up
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                // If the selector is on the first row, move it to the last row
                // Else, move the selector one row up
                if (vertical <= 1)
                {
                    vertical = maxVertical;
                }
                else
                    vertical--;
            }
            // Moves the selector to the down
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                // If the selector is on the last row, move it to the first row
                // Else, move the selector one row down
                if (vertical >= maxVertical)
                {
                    vertical = 1;
                }
                else
                    vertical++;
            }

            // Sets the selectors new position
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                uiLight.transform.parent.position = new Vector2(slot[horizontal - 1].transform.position.x, slot[vertical + 1].transform.position.y);

                SoundController.PlaySound(SoundController.Sound.Selector);
            }
        }
    }
}
