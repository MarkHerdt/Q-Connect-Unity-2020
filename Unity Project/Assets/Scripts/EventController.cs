using System;
using UnityEngine;

public class EventController : MonoBehaviour
{
    public static EventController self;

    private void Awake()
    {
        self = this;
    }

    /// <summary>
    /// Is fired when the player loses the game
    /// </summary>
    public event Action OnGameOver;
    /// <summary>
    /// Is fired when the player loses the game
    /// </summary>
    public void GameOver()
    {
        if (OnGameOver != null)
        {
            OnGameOver();
        }
    }

    /// <summary>
    /// Is fired when the player receives a life point
    /// </summary>
    public event Action OnLifeAdd;
    /// <summary>
    /// Is fired when the player receives a life point
    /// </summary>
    public void AddLIfe()
    {
        if (OnLifeAdd != null)
        {
            OnLifeAdd();
        }
    }

    /// <summary>
    /// Event for testing purposes
    /// </summary>
    public event Action OnTest;
    /// <summary>
    /// Event for testing purposes
    /// </summary>
    public void Test()
    {
        if (OnTest != null)
        {
            OnTest();
        }
    }
}
