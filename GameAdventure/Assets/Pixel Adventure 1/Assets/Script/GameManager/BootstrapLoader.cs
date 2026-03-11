using UnityEngine;
using UnityEngine.SceneManagement;

public class BootstrapLoader : MonoBehaviour
{
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "BootstrapScene")
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}