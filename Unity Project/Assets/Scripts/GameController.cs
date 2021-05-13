using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Controller for the different game states
public class GameController : MonoBehaviour
{
    public GameObject backGround;
    Canvas backGroundCanvas;

    // Robot
    public RobotParts part;
    public GameObject robotPrefab;
    GameObject robot;

    int randomChance = 50; // Chance that a second part is missing (0 - 100)
    float chanceTimer;
    float spawnDelay = 2; // Time between spawns
    float spawnTimer;
    float speedDelay = 3f; // Time between speed increase
    float speedTimer;
    float maxSpeed = 10f; // Maximum speed the robots move with
    float minSpawnDelay = 1.5f; // Min delay between a robot spawn

    // Score
    public Text scoreText; // Current score
    public Text highscoreText; // Highest score
    int score = 0;
    public int Score { get => score; }
    int highScore;

    // GameState
    bool gameOver = false;
    public bool GameOver { get => gameOver; }
    bool gameActive = false;
    public bool GameActive { get => gameActive; }

    bool accessible = true; // Only allows robot parts to be set to "null" while "accessible" is "true"

    // Audio
    AudioSource source;
    public AudioClip conveyorBelt;

    private void Awake()
    {
        backGroundCanvas = backGround.GetComponentInChildren<Canvas>();

        highScore = PlayerPrefs.GetInt("SaveScore");

        SoundController.InitializeSounds();

        source = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        EventController.self.OnGameOver += GameOverTrue;
    }

    private void OnDestroy()
    {
        EventController.self.OnGameOver -= GameOverTrue;
    }

    private void Start()
    {
        highscoreText.text = "" + string.Format("{0:0000}", PlayerPrefs.GetInt("SaveScore"));
        scoreText.text = "" + string.Format("{0:0000}", score);

        Cursor.visible = false;
        // Spawn
        spawnTimer = spawnDelay;
        SpawnRobot();
        // Audio
        source.clip = conveyorBelt;
        source.loop = true;
        source.volume = .25f;
        source.Play();
    }

    private void Update()
    {
        // Spawns a roboter every set amount
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0) 
        {
            spawnTimer = spawnDelay;
            SpawnRobot();
        }

        // Increases speed every set amount
        speedTimer += Time.deltaTime;
        if (speedTimer >= speedDelay && RobotBehaviour.speed <= maxSpeed) 
        {
            RobotBehaviour.speed += .05f;
            if (spawnDelay >= minSpawnDelay) // Decreases spawn intervall
            {
                spawnDelay -= .025f;
            }
            speedTimer = 0;
        }

        // Increases the chance that muliple robot parts are mussing
        chanceTimer -= Time.deltaTime;
        if (RobotBehaviour.speed >= maxSpeed && spawnDelay >= minSpawnDelay) // When the robots speed and the "spawnDelay" have both reached their maximum values
        {
            randomChance += 1;
        }

        // Saves and sets the current score as the new highscore, if the current score is larger or equal to the current highscore
        if (highScore <= score)
        {
            highScore = score;
            PlayerPrefs.SetInt("SaveScore", highScore);
            highscoreText.text = "" + string.Format("{0:0000}", PlayerPrefs.GetInt("SaveScore"));
        }
    }

    /// <summary>
    /// Instantiates the robots
    /// </summary>
    void SpawnRobot()
    {
        List<int> missingParts = new List<int>(); // Saves how many different parts are missing on a robot

        if (!gameOver)
        {
            accessible = true; // Multiple robot parts can be missing as long as "accessible" is "true"
            int randomPart = Random.Range(0, part.partsArray.Length); // Robot part that will be missing
            int random = Random.Range(0, 100); // Chance to set another robot part to "null"

            robot = Instantiate(robotPrefab, transform.position, Quaternion.identity);
            robot.transform.SetParent(backGroundCanvas.transform.parent);
            // Sets scaling an position of robots depending on the screen size
            robot.transform.localScale = new Vector2(61.5f * 9.0f / 16.0f * Screen.width / Screen.height, 61.5f * 9.0f / 16.0f * Screen.width / Screen.height);
            robot.transform.localPosition = new Vector3(-120f * 9.0f / 16.0f * Screen.width / Screen.height, -25f * 9.0f / 16.0f * Screen.width / Screen.height, 0f);

            for (int i = 0; i <= part.partsArray.Length; i++)
            {
                if (i == randomPart) // Sets one of the sprites to "null"
                {
                    robot.transform.GetChild(randomPart).GetComponent<SpriteRenderer>().sprite = null;
                    missingParts.Add(randomPart); // Saves the selected part to the list
                    robot.GetComponent<RobotBehaviour>().MissingParts++;

                    if (random <= randomChance && accessible) // Has a set chance to set another sprite to "null"
                    {
                        randomPart = Random.Range(0, part.partsArray.Length);
                        robot.transform.GetChild(randomPart).GetComponent<SpriteRenderer>().sprite = null;

                        // Checks if the value of "randomPart" is already in the list
                        bool alreadyInList = false;
                        foreach (int item in missingParts)
                        {
                            if (randomPart == item)
                            {
                                alreadyInList = true;
                                break;
                            }
                        }
                        // Only increments "MissingParts" if the new part isn't already in the list
                        if (!alreadyInList)
                        {
                            robot.GetComponent<RobotBehaviour>().MissingParts++;
                            missingParts.Add(randomPart); // Saves the selected part to the list
                        }

                        accessible = false;
                    }
                    break;
                }
                //// Keine ahnung was das hier macht (kommentare wären schön)
                //else if (i <= 5)
                //{
                //    robot.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = part.partsArray[i];
                //}
            }
        }
    }

    /// <summary>
    /// Sets and displays the current score
    /// </summary>
    public void SetScore()
    {
        score++;
        scoreText.text = "" + string.Format("{0:0000}", score);
    }

    /// <summary>
    /// Sets the current GameState to "true"
    /// </summary>
    public void GameActiveTrue()
    {
        gameOver = false;
        gameActive = true;
    }

    /// <summary>
    /// Sets the current GameState to "false"
    /// </summary>
    public void GameActiveFalse()
    {
        gameActive = false;
    }

    /// <summary>
    /// Sets "gameOver" to "true" and stops new roboter from instantiating
    /// </summary>
    void GameOverTrue()
    {
        gameOver = true;
        GameActiveFalse();
    }

    /// <summary>
    /// Sets "gameOver" back to "false"
    /// </summary>
    public void GameOverFalse()
    {
        gameOver = false;
    }

    /// <summary>
    /// Resets the score
    /// </summary>
    public void ResetScore()
    {
        score = 0;
        scoreText.text = "" + string.Format("{0:0000}", score);
    }
}
