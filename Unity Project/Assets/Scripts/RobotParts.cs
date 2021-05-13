using UnityEngine;

// Parts, the robots are composed of
[CreateAssetMenu(fileName = "RobotParts", menuName = "RobotParts")]
public class RobotParts : ScriptableObject
{
    // Parts are saved in array, so they can be adressed through the index
    public Sprite[] partsArray = new Sprite[6];

    Sprite head; 
    Sprite torso;
    Sprite armLeft;
    Sprite armRight;
    Sprite legLeft;
    Sprite legRight;

    private void Awake()
    {
        partsArray[0] = head;     
        partsArray[1] = torso;    
        partsArray[2] = armLeft;  
        partsArray[3] = armRight; 
        partsArray[4] = legLeft;  
        partsArray[5] = legRight;
    }
}
