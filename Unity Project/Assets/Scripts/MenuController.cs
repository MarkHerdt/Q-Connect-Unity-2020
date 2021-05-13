using UnityEngine;
using UnityEngine.UI;

// Controller for the different menus
public class MenuController : MonoBehaviour
{
    // Controls to move through the menus with the keyboard
    Button[] buttons; // List of menu bottons that are currently active
    ColorBlock colorBlock;
    Color normal;
    Color highlighted;
    int index = 0; // Index of button that's currently selected

    // References
    public GameController gameController;
    public LifeController life;
    public GameObject mainMenuScreen;
    Transform[] mainMenuScreenArray;
    public GameObject mainMenu;
    public GameObject options;
    public GameObject pauseMenu;
    public GameObject gameOverMenu;
    GameObject[] robot;
    public RobotRepairCollider offset;

    bool gameIsPaused = false;
    public bool GameIsPaused { get => gameIsPaused; }

    float buttonVolume = .5f;

    void Awake()
    {
        mainMenuScreenArray = mainMenuScreen.GetComponentsInChildren<Transform>();
    }

    private void Start()
    {
        Cursor.visible = false;

        colorBlock.colorMultiplier = 1; // "colorMultiplier" of "colorBlock" needs to be not zero, in order to see the color changes
    }

    private void OnEnable()
    {
        // Subscriptions
        EventController.self.OnGameOver += GameOver;
    }

    private void OnDestroy()
    {
        // Subscriptions
        EventController.self.OnGameOver -= GameOver;
    }

    void Update()
    {
        // Starts/Restarts the game
        if (!gameController.GameActive && !gameIsPaused && !gameController.GameOver && Input.GetKeyDown(KeyCode.Return))
        {
            StartGame();
        }

        // Changing the gamestate 
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SoundController.PlaySound(SoundController.Sound.Button, buttonVolume);

            if (gameIsPaused)
            {
                Resume();
            }
            else if (gameController.GameActive && !gameIsPaused)
            {
                PauseMenu();
            }
            else if (!gameController.GameActive && !gameIsPaused)
            {
                MainMenu();
            }
        }

        // Keyboard movement inside the menus
        // Only accessible while the Pause/GameOver Menus are active
        if (gameIsPaused || gameController.GameOver)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                SoundController.PlaySound(SoundController.Sound.Button, buttonVolume);

                SetInitialButtonColor();

                // Moves the selector up
                if (index == 0)
                {
                    index = buttons.Length - 1;
                }
                else
                    index--;

                SetNewButtonColor();
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                SoundController.PlaySound(SoundController.Sound.Button, buttonVolume);

                SetInitialButtonColor();

                // Moves the selector down
                if (index == buttons.Length - 1)
                {
                    index = 0;
                }
                else
                    index++;

                SetNewButtonColor();
            }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SoundController.PlaySound(SoundController.Sound.Button, buttonVolume);

                // Calls the script that's on the button
                buttons[index].onClick.Invoke();
            }
        }
    }

    /// <summary>
    /// Starts/Restarts the game
    /// </summary>
    public void StartGame()
    {
        if (buttons != null)
        {
            SetInitialButtonColor();
        }

        Cursor.visible = false;

        // Destroyes all active robots
        robot = GameObject.FindGameObjectsWithTag("Robot");
        foreach (GameObject item in robot)
        {
            Destroy(item);
        }
        RobotBehaviour.speed = RobotBehaviour.initialSpeed;

        // GameState
        MainMenuActive_Inactive(false);
        gameOverMenu.SetActive(false);
        gameController.GameActiveTrue();

        // LifeController
        life.ResetLife();
        offset.ResetOffset();
        // Score
        gameController.ResetScore();

        SoundController.PlaySound(SoundController.Sound.Button, buttonVolume);
    }

    /// <summary>
    /// Menu when in title screen
    /// </summary>
    void MainMenu()
    {
        gameIsPaused = true;
        Time.timeScale = 0;
        mainMenu.SetActive(true);
        GetButtons(mainMenu.transform);
    }

    /// <summary>
    ///  Menu during active game
    /// </summary>
    void PauseMenu()
    {
        gameIsPaused = true;        
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        GetButtons(pauseMenu.transform);
    }

    /// <summary>
    /// Resumes the game if it was paused
    /// </summary>
    public void Resume()
    {
        SetInitialButtonColor();

        gameIsPaused = false;
        Time.timeScale = 1;
        mainMenu.SetActive(false);
        pauseMenu.SetActive(false);
        
        Cursor.visible = false;
    }

    /// <summary>
    /// Back to TitleScreen
    /// </summary>
    public void TitleScreen()
    {
        SetInitialButtonColor();

        gameIsPaused = false;
        Time.timeScale = 1;

        Cursor.visible = false;

        // GameObjects
        mainMenu.SetActive(false);
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        MainMenuActive_Inactive(true);

        // GameState
        gameController.GameOverFalse();
        gameController.GameActiveFalse();
        gameController.ResetScore();
    }

    public void OptionsMenu()
    {
        options.SetActive(true);
    }

    /// <summary>
    /// Activates the game over screen on GameOver
    /// </summary>
    void GameOver()
    {
        Cursor.visible = true;
        gameOverMenu.SetActive(true);
        GetButtons(gameOverMenu.transform);
    }

    /// <summary>
    /// Quit the game
    /// </summary>
    public void QuitGame()
    {
        SoundController.PlaySound(SoundController.Sound.Button, buttonVolume);
        Application.Quit();
    }

    /// <summary>
    /// Gets the buttons inside the respective menu thats being activated
    /// </summary>
    /// <param name="obj"></param>
    void GetButtons(Transform obj)
    {
        buttons = obj.GetComponentsInChildren<Button>();

        // Saves initial color settings of the button
        normal = buttons[index].colors.normalColor;
        highlighted = buttons[index].colors.highlightedColor;

        // Applies highlighted settings to the button while it's selected
        colorBlock.normalColor = highlighted;
        colorBlock.highlightedColor = highlighted;
        buttons[index].colors = colorBlock;

    }

    /// <summary>
    /// Sets the button colors to their initial values
    /// </summary>
    public void SetInitialButtonColor()
    {
        colorBlock.normalColor = normal;
        colorBlock.highlightedColor = highlighted;
        buttons[index].colors = colorBlock;
    }

    /// <summary>
    /// Highlights the button while it's selected
    /// </summary>
    void SetNewButtonColor()
    {
        // Saves the color settings of the new button
        normal = buttons[index].colors.normalColor;
        highlighted = buttons[index].colors.highlightedColor;

        // Applies highlighted settings to the button while it's selected
        colorBlock.normalColor = highlighted;
        colorBlock.highlightedColor = highlighted;
        buttons[index].colors = colorBlock;
    }

    /// <summary>
    /// Sets the main menu active/inactive
    /// </summary>
    /// <param name="activeSelf"></param>
    void MainMenuActive_Inactive(bool activeSelf)
    {
        for (int i = 1; i < mainMenuScreenArray.Length; i++)
        {
            mainMenuScreenArray[i].gameObject.SetActive(activeSelf);
        }
    }
}
