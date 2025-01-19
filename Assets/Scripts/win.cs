using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class win : MonoBehaviour
{
    [SerializeField] private int cantidadPuntos;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
            if (other.CompareTag("Ball"))
            {
            ScoreManager.instance.SumarPuntos(cantidadPuntos);
            print(cantidadPuntos);
           
            }
    }

  
        
}
