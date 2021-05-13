using UnityEngine;

// Controller for the life of the player
public class LifeController : MonoBehaviour
{
    // Life
    int maxLife = 3; // Maximum life points the player has
    int currentLife;
    public int CurrentLife { get => currentLife; }

    // Sprites
    SpriteRenderer[] lifeSprites;
    public Sprite green;
    public Sprite red;

    // References
    public GameController gameController;

    private void Awake()
    {
        currentLife = maxLife;

        lifeSprites = new SpriteRenderer[maxLife];
        for (int i = 0; i < lifeSprites.Length; i++)
        {
            lifeSprites[i] = transform.GetChild(i).GetComponent<SpriteRenderer>(); 
        }
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            AddLife();
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            SubstractLife();
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            EventController.self.Test();
        }
    }
#endif

    /// <summary>
    /// Gives the player one life point
    /// </summary>
    public void AddLife()
    {
        if (currentLife < maxLife) 
        {            
            SoundController.PlaySound(SoundController.Sound.AddLife);

            // Changes the sprite
            lifeSprites[currentLife].transform.gameObject.GetComponent<SpriteRenderer>().sprite = green;

            currentLife++;
            EventController.self.AddLIfe();
        }
    }

    /// <summary>
    /// Substracts one life point if a robot is not fully repaired
    /// </summary>
    public void SubstractLife()
    {
        if (gameController.GameActive)
        {
            --currentLife;
            if (currentLife >= 0)
            {
                // Changes the sprite
                for (int i = currentLife; i < lifeSprites.Length; i++)
                {
                    lifeSprites[i].transform.gameObject.GetComponent<SpriteRenderer>().sprite = red;
                }
                SoundController.PlaySound(SoundController.Sound.SubstractLife, .25f);
            }

            // When the player reaches 0 life points
            if (currentLife == 0)
            {
                SoundController.PlaySound(SoundController.Sound.GameOver);

                // Event
                EventController.self.GameOver();
            }
        }
    }

    /// <summary>
    /// Resets life when a new game is started
    /// </summary>
    public void ResetLife()
    {
        for (int i = 0; i < maxLife; i++)
        {
            lifeSprites[i].transform.gameObject.GetComponent<SpriteRenderer>().sprite = green;
        }

        currentLife = maxLife;
    }
}
