using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.Rendering;
using System.Collections;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class BallHandler : MonoBehaviour
{


    //Reference to the pivot rigidbody component
    [SerializeField] private Rigidbody2D pivot; 
    //Reference to the ball prefab
    [SerializeField] GameObject ballPrefab;

    [SerializeField] private float detachDelay;
    [SerializeField] private float respawnDelay;

    //The rigidbody of the current spawned ball
    private Rigidbody2D currentBallRigidbody;

    private SpringJoint2D currentBallSpringJoint;


    private bool isDragging;

    public float force;

    public float limitBall;

    public float actualBall;

    public bool quedanballs;


    [SerializeField] public GameObject winPanel;

    [SerializeField] private Image[] canvasSprites; // Lista de sprites en el Canvas

    [SerializeField] private float destroyDelay = 2f;

    private static int currentSpriteIndex = 0;


    void Start()
    {
        winPanel.SetActive(false);
        quedanballs = true;
        
        currentSpriteIndex = 0;
        //spawn the first ball when the game starts
        SpawnNewBall();

        Time.timeScale = 1;
        

    }

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }


    // Update is called once per frame
    void Update()
    {

        
        //if there is no current ball , exit the method
        if (currentBallRigidbody == null) { return; }


        //If I am not touching the screen
        //if (!Touchscreen.current.primaryTouch.press.isPressed)
        //check if there are no active touches
        if (Touch.activeTouches.Count == 0)
        {
            //if the ball was being dragged , launch it
            if (isDragging)
            {
                LaunchBall();
            }

            //reset the dragging state
            isDragging = false;
                        
            //dont do anything
            return;
        }
        
        //first time the player touch the screen
        //mark the ball kinematic to allow manual movement
        currentBallRigidbody.bodyType = RigidbodyType2D.Kinematic;

        //mark the ball as being dragged
        isDragging = true;

        //Get the first active touch
        //Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
        Vector2 touchPosition = new Vector2();

        //iterate through all active touches
        foreach (Touch touch in Touch.activeTouches)
        {
            touchPosition += touch.screenPosition; //Sum up th positions of all touches
        }

        //Get the average position of all touches 
        touchPosition /= Touch.activeTouches.Count; 

        //convert the touch position to the unity world space
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(touchPosition);

        //Update the ball's position to follow the touch
        currentBallRigidbody.position = worldPosition;


        
    }

    /// <summary>
    /// Spawn a new ball and attaches it to the pivot using SpringJoint2D
    /// </summary>
    private void SpawnNewBall()
    {
        if (limitBall > actualBall)
        {
            //Instantiate a new ball prefab at the pivot's position 
        GameObject ballInstance = Instantiate(ballPrefab, pivot.position, Quaternion.identity);

        //set the current rigidbody to the newly created instance
        currentBallRigidbody = ballInstance.GetComponent<Rigidbody2D>();

        //set the springJoint from the newly created instance
        currentBallSpringJoint = ballInstance.GetComponent<SpringJoint2D>();

        //attach the spring joint of the ball to the pivot
        currentBallSpringJoint.connectedBody = pivot;
            actualBall++;
            
        }
        else if (limitBall <= actualBall) 
        {
        
            quedanballs = false;
        }

        
    }

    private void LaunchBall()
    {
        //make the ball dynamic so it reacts to physics
        currentBallRigidbody.bodyType = RigidbodyType2D.Dynamic;

        //Schedule the ball's detachment after a delay
        Invoke(nameof(DetachBall), detachDelay);
        
    }

    private void DetachBall()
    {
        Vector2 dir = (new Vector2(pivot.position.x, pivot.position.y) - new Vector2(currentBallRigidbody.transform.position.x, currentBallRigidbody.transform.position.y)).normalized;

        currentBallRigidbody.AddForce(dir*force);

        //clear the reference to the current's ball rigidbody
        currentBallRigidbody = null;

        //Disable the spring joint to disconnect the ball from the pivot
        currentBallSpringJoint.enabled = false;

        //Clear the reference to the current ball's springjoint2D
        currentBallSpringJoint = null;

        //Respawn a new ball after a delay
        Invoke(nameof(SpawnNewBall), respawnDelay);

        StartCoroutine(DestroyAfterDelay());

    }

    public void Win()
    {
        int oldHighScore = PlayerPrefs.GetInt("HighScore");
        if (oldHighScore < ScoreManager.instance.RecogerPuntuacion()) 
        {
            PlayerPrefs.SetInt("HighScore", ScoreManager.instance.RecogerPuntuacion());
            ScoreManager.instance.highScore.text = "High Score: " + ScoreManager.instance.RecogerPuntuacion().ToString("0000");
        }
        winPanel.SetActive(true);
        Time.timeScale = 0;
            
        
    }

   


    private IEnumerator DestroyAfterDelay()
{
    // Espera el tiempo definido en destroyDelay
    yield return new WaitForSeconds(destroyDelay);

    // Oculta el sprite correspondiente
    if (currentSpriteIndex < canvasSprites.Length)
    {
        canvasSprites[currentSpriteIndex].gameObject.SetActive(false); // Desactiva el sprite
        currentSpriteIndex++; // Avanza al siguiente sprite
    }
    else
    {
        Debug.LogWarning("No hay más sprites para desactivar.");
    }

   
    if (quedanballs == false)
    {
        Win();
    }
}



}
