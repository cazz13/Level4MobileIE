using UnityEngine;

public class GameManager : MonoBehaviour
{

    //Singleton: to use methods from this class I don't need to get a component
    public static GameManager Instance; //Capitalize cause this is a static reference to my object

    private int playerPoints = 0;
    public int PlayerPoints
    {
        get => playerPoints;
        set => playerPoints = value;
    }

    private void Awake()
    {
        //Singleton, reference Instance to this object instance
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            //assign the static instance to the object
            Instance = this;
            //maintain this instance to the next scene
            DontDestroyOnLoad(this.gameObject);
        }
    }





}
