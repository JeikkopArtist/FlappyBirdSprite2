using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI puntos;

    private void Start()
    {
        TextPoints();
    }

    private void Update()
    {
        TextPoints();
    }
    
    public void TextPoints()
    {
        puntos.text = " ";
    }

    private void UpdateUITexts()
    {
        // Si el GameManager aún no ha arrancado, no hacemos nada para evitar errores
        if (GameManager.Instance == null) return;
        if (puntos != null)
        {
            if (GameManager.Instance.GameOver)
            {
                puntos.text = "";
            }
        }
    }

    public void Showpoints()
    {
        if  (GameManager.Instance == null) return;
        
        if (puntos != null)
        {
            if (puntos != null)
                puntos.text = "Puntos: " + GameManager.Instance.Puntos;
        }
       
    }
}
