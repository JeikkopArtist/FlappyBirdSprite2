using UnityEngine;

public class UIMenuManager : MonoBehaviour
{
    public void OnClickNewGame()
    {
        //carga escena siguiente, pajaro physics
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }
    
    public void OnClickNewGameSimulated()
    {
        //carga escena pajaro simulado
        UnityEngine.SceneManagement.SceneManager.LoadScene("antiguo");
    }


    public void OnClickMainMenu()
    {
        //vuelve al menu
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
