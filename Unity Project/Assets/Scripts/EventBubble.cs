using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Displays text inside the bubble for the different events
public class EventBubble : MonoBehaviour
{
    public GameObject bubble;
    Text bubbleText;
    string initialText;

    List<string> textQueue = new List<string>();

    IEnumerator displayText; // Coroutine
    IEnumerator waitForCoroutine; // Coroutine
    float displayTime = 2.5f; // Time in seconds, the message will e displayed
    float waitTime = .5f; // Time in seconds to wait, before the running coroutine gets interrupted

    private void Awake()
    {
        bubbleText = bubble.GetComponentInChildren<Text>();
        initialText = bubbleText.text;
    }

    private void Start()
    {
        // Subscriptions
        EventController.self.OnLifeAdd += AddLife;
        EventController.self.OnTest += Test;
    }

    private void OnDestroy()
    {
        // Subscriptions
        EventController.self.OnLifeAdd -= AddLife;
        EventController.self.OnTest -= Test;
    }

    /// <summary>
    /// Displays the different messages inside the bubble for the different events
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    IEnumerator DisplayText(string text)
    {
        // Saves the active coroutíne in "displayText"
        displayText = DisplayText(text);

        // Activates the objects
        bubble.gameObject.SetActive(true);
        bubbleText.gameObject.SetActive(true);
        // Sets the new text
        bubbleText.text = text;

        yield return new WaitForSeconds(displayTime);

        // Sets the initial text
        bubbleText.text = initialText;
        // Deactivates the objects
        bubble.gameObject.SetActive(false);
        bubbleText.gameObject.SetActive(false);

        if (textQueue.Count > 0)
        {
            textQueue.RemoveAt(0); // Removes the text from the queue list
            Debug.Log(textQueue.Count);
        }

        // If the coroutine finishes before the "waitTime" of "WaitForCoroutine"
        if (textQueue.Count > 0)
        {
            // Stop the "WaitForCoroutine" coroutine
            if (waitForCoroutine != null)
            {
                StopCoroutine(waitForCoroutine);
            }

            // Start the next coroutine with the next text in the queue list
            StartCoroutine(DisplayText(textQueue[0]));
        }
        displayText = null;
    }

    /// <summary>
    /// Checks if a coroutine is currently running, if yes, stop it after a set amount of time
    /// </summary>
    void CheckForCoroutine(string text)
    {
        // Adds text to display to the queue list
        textQueue.Add(text);
        Debug.Log(textQueue.Count);

        // If the "DisplayText" coroutine is currently running, stop it
        if (displayText != null)
        {
            // Waits a set amount of time before stopping the running coroutine
            IEnumerator WaitForCoroutine()
            {
                waitForCoroutine = WaitForCoroutine();

                yield return new WaitForSeconds(waitTime);

                // If the "DisplayText" coroutine is still running, stop it
                if (displayText != null)
                {
                    StopCoroutine(displayText);
                }

                waitForCoroutine = null;
                StartCoroutine(DisplayText(text));
            }

            StartCoroutine(WaitForCoroutine());
        }
        // If the "DisplayText" coroutine is not running
        else
            StartCoroutine(DisplayText(text));
    }

    /// <summary>
    /// Displays "+1 Life" inside the event bubble when the player receives a life point
    /// </summary>
    void AddLife()
    {
        CheckForCoroutine("+1 Life");
    }

    /// <summary>
    /// Displays "Test" inside the event bubble (for testing purposes)
    /// </summary>
    void Test()
    {
        CheckForCoroutine("Test");
    }
}
