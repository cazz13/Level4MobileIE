using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
public class menu : MonoBehaviour
{
    [SerializeField] public GameObject pausePanel; // El panel del menu pausa
    
    
   
    private bool isPauseOpen = false; 
    private bool isPause = false;

     

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Menu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Reiniciar()
    {
        // Cargar la escena actual para reiniciarla
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        Time.timeScale = 1;
    }

    // metodo para activar o desactivar el panel del movil
    public void OpenPause()
    {
        
        if (isPauseOpen == false && isPause == false)
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0;
            isPause = true;
            isPauseOpen = true;
        }


    }

    //metodo para cerrar el menu de pausa
    public void ClosePause()
    {
        if (isPauseOpen == true && isPause == true)
        {
            pausePanel.SetActive(false);
            isPause = false;
            isPauseOpen= false;
            Time.timeScale = 1;
        }
    }

}
