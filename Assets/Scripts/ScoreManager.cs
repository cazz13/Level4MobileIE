using UnityEngine;
using System.Collections;
using TMPro;

public class ScoreManager : MonoBehaviour
{

    public static ScoreManager instance;

    [SerializeField] private int puntajeActual;
    [SerializeField] public TMP_Text score;
    [SerializeField] public TMP_Text highScore;


    private void Awake()
    {
            
            instance = this;

           highScore.text = "High Score: " + PlayerPrefs.GetInt("HighScore").ToString("0000");
        
    }

    public void SumarPuntos(int puntos)
    {
        puntajeActual += puntos;


        score.text = "SCORE: " + puntajeActual;
        print(puntajeActual);
    }

    public int RecogerPuntuacion()
    {
        return puntajeActual;
    }

}
