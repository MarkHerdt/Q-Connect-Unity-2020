using UnityEngine;

public class GameAssets : MonoBehaviour
{
    private static GameAssets instance;

    public static GameAssets Instance
    {
        get
        {
            // If there's no gameobject with this script on the scene
            if (instance == null)
            {
                // Go into the "Resource" folder and instantiate the prefab "GameAssets"
                instance = Instantiate(Resources.Load<GameAssets>("GameAssets"));
            }
            return instance;
        }
    }

    // Array of AudioClips
    public SoundAudioClip[] audioArray;

    [System.Serializable]
    public class SoundAudioClip
    {
        public SoundController.Sound sound; // Name of AudioClip
        public AudioClip audioClip;         // AudioFile
    }
}
