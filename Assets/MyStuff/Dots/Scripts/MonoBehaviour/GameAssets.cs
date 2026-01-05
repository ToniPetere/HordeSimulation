using UnityEngine;

public class GameAssets : MonoBehaviour
{

    public const int UNITS_LAYER = 6; //For Zombies rn

    public static GameAssets Instance
    {
        get { return instance; }
        set { instance = value; }
    }
    private static GameAssets instance;
    private void Awake()
    {
        instance = this;
    }

}
