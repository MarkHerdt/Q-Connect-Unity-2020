using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Plays the different sounds
/// </summary>
public static class SoundController
{
    /// <summary>
    /// Available sounds
    /// </summary>
    public enum Sound
    {
        BackgroundMusic,
        ConveyorBelt,
        Button,
        Selector,
        LightSwitch,
        Drill,
        AddLife,
        SubstractLife,
        RobotRepaired,
        GameOver,
        SoundName // For testing purposes
    }

    private static float audioTravelDistance = 50; // Distance the AudioClip will be heard at

    private static Dictionary<Sound, float> soundTimerDictionary; // Stores the time, a sound has last been played
    private static GameObject oneShotGameObject;
    private static AudioSource oneShotAudioSource;

    /// <summary>
    /// Initial values for all sounds that have an exception in the "CanPlaySound(Sound sound)" method
    /// </summary>
    public static void InitializeSounds()
    {
        soundTimerDictionary = new Dictionary<Sound, float>(); // Creates a new object

        // All sounds with an exception set in the "CanPlaySound(Sound sound)" method need an initial key
        soundTimerDictionary[Sound.SoundName] = 0f; // Initial key for the sound
    }

    /// <summary>
    /// Plays the parsed sound
    /// </summary>
    /// <param name="sound"></param>
    public static void PlaySound(Sound sound)
    {
        // Only plays the sound if the "CanPlaySound(Sound sound)" method return "true"
        if (CanPlaySound(sound))
        {
            // Only creates the gameobject once, then reuses it for every other sound
            if (oneShotGameObject == null)
            {
                oneShotGameObject = new GameObject("Sound"); // Creates an object to play the sound from
                oneShotAudioSource = oneShotGameObject.AddComponent<AudioSource>(); // Attaches an audiosource to the created gameobject
            }
            oneShotAudioSource.PlayOneShot(GetAudioClip(sound)); // Plays the parsed sound
        }
    }

    /// <summary>
    /// Plays the parsed sound with the set volume
    /// </summary>
    /// <param name="sound"></param>
    public static void PlaySound(Sound sound, float volume)
    {
        // Only plays the sound if the "CanPlaySound(Sound sound)" method return "true"
        if (CanPlaySound(sound))
        {
            GameObject soundGameObject = new GameObject("Sound"); // Creates an object to play the sound from
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>(); // Attaches an audiosource to the created gameobject
            audioSource.clip = GetAudioClip(sound);
            audioSource.volume = volume; // Sets the volume
            audioSource.Play(); // Plays the parsed sound

            Object.Destroy(soundGameObject, audioSource.clip.length); // Destroys the gameobject after the clip has finished playing
        }
    }

    /// <summary>
    /// Plays the parsed sound at the specified position
    /// </summary>
    /// <param name="sound"></param>
    public static void PlaySound(Sound sound, Vector3 position)
    {
        // Only plays the sound if the "CanPlaySound(Sound sound)" method return "true"
        if (CanPlaySound(sound))
        {
            GameObject soundGameObject = new GameObject("Sound"); // Creates an object to play the sound from
            soundGameObject.transform.position = position; // Sets the position of the "soundGameObject"
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>(); // Attaches an audiosource to the created gameobject
            audioSource.clip = GetAudioClip(sound);
            audioSource.spatialBlend = 1f;
            audioSource.minDistance = audioTravelDistance; // Min distance the AudioClip will be heard at
            audioSource.maxDistance = audioTravelDistance; // Max distance the AudioClip will be heard at
            audioSource.Play(); // Plays the parsed sound

            Object.Destroy(soundGameObject, audioSource.clip.length); // Destroys the gameobject after the clip has finished playing
        }
    }

    /// <summary>
    /// Plays the parsed sound at the specified position, with the specified AudioTravelDistance
    /// </summary>
    /// <param name="sound"></param>
    public static void PlaySound(Sound sound, Vector3 position, float audioTravelDistance)
    {
        // Only plays the sound if the "CanPlaySound(Sound sound)" method return "true"
        if (CanPlaySound(sound))
        {
            GameObject soundGameObject = new GameObject("Sound"); // Creates an object to play the sound from
            soundGameObject.transform.position = position; // Sets the position of the "soundGameObject"
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>(); // Attaches an audiosource to the created gameobject
            audioSource.clip = GetAudioClip(sound);
            audioSource.spatialBlend = 1f;
            audioSource.minDistance = audioTravelDistance; // Min distance the AudioClip will be heard at
            audioSource.maxDistance = audioTravelDistance; // Max distance the AudioClip will be heard at
            audioSource.Play(); // Plays the parsed sound

            Object.Destroy(soundGameObject, audioSource.clip.length); // Destroys the gameobject after the clip has finished playing
        }
    }

    /// <summary>
    /// Plays the parsed sound with the set volume, at the specified position
    /// </summary>
    /// <param name="sound"></param>
    public static void PlaySound(Sound sound, float volume, Vector3 position)
    {
        // Only plays the sound if the "CanPlaySound(Sound sound)" method return "true"
        if (CanPlaySound(sound))
        {
            GameObject soundGameObject = new GameObject("Sound"); // Creates an object to play the sound from
            soundGameObject.transform.position = position; // Sets the position of the "soundGameObject"
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>(); // Attaches an audiosource to the created gameobject
            audioSource.clip = GetAudioClip(sound);
            audioSource.spatialBlend = 1f;
            audioSource.volume = volume; // Sets the volume
            audioSource.minDistance = audioTravelDistance; // Min distance the AudioClip will be heard at
            audioSource.maxDistance = audioTravelDistance; // Max distance the AudioClip will be heard at
            audioSource.Play(); // Plays the parsed sound

            Object.Destroy(soundGameObject, audioSource.clip.length); // Destroys the gameobject after the clip has finished playing
        }
    }

    /// <summary>
    /// Plays the parsed sound with the set volume, at the specified position and the specified AudioTravelDistance
    /// </summary>
    /// <param name="sound"></param>
    public static void PlaySound(Sound sound, float volume, Vector3 position, float audioTravelDistance)
    {
        // Only plays the sound if the "CanPlaySound(Sound sound)" method return "true"
        if (CanPlaySound(sound))
        {
            GameObject soundGameObject = new GameObject("Sound"); // Creates an object to play the sound from
            soundGameObject.transform.position = position; // Sets the position of the "soundGameObject"
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>(); // Attaches an audiosource to the created gameobject
            audioSource.clip = GetAudioClip(sound);
            audioSource.spatialBlend = 1f;
            audioSource.volume = volume; // Sets the volume
            audioSource.minDistance = audioTravelDistance; // Min distance the AudioClip will be heard at
            audioSource.maxDistance = audioTravelDistance; // Max distance the AudioClip will be heard at
            audioSource.Play(); // Plays the parsed sound

            Object.Destroy(soundGameObject, audioSource.clip.length); // Destroys the gameobject after the clip has finished playing
        }
    }

    /// <summary>
    /// Checks if enough time has passed to play a sound again
    /// </summary>
    /// <param name="sound"></param>
    /// <returns></returns>
    private static bool CanPlaySound(Sound sound)
    {
        switch (sound)
        {
            // Case for all sounds not specified below (playes all sound normaly)
            default:
                return true;
            // Example 
            case Sound.SoundName:
                // Checks if the dictionary contains the key(SoundName)
                if (soundTimerDictionary.ContainsKey(sound))
                {
                    float lastTimePlayed = soundTimerDictionary[sound];
                    float delayBetweenSounds = .5f;
                    // Only plays the sound if the the last time the sound was played + the delay specified for the sound is less than the current time
                    if (lastTimePlayed + delayBetweenSounds < Time.time)
                    {
                        soundTimerDictionary[sound] = Time.time; // Updates the time, the sound has last been played to the current time
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return true;
                }
                //break;
        }
    }

    /// <summary>
    /// Searches in "GameAssets.audioArray" for the parsed sound and returns it when found
    /// </summary>
    /// <param name="sound"></param>
    /// <returns></returns>
    private static AudioClip GetAudioClip(Sound sound)
    {
        foreach (GameAssets.SoundAudioClip soundAudioClip in GameAssets.Instance.audioArray)
        {
            // If the sound in the array matches the parsed sound
            if (soundAudioClip.sound == sound)
            {
                return soundAudioClip.audioClip;
            }
        }
        
        // If the sound is not found in the array
        Debug.LogError("AudioClip " + sound + " not found");
        return null;
    }
}