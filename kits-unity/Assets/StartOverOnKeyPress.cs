using UnityEngine;
using UnityEngine.SceneManagement;

public class StartOverOnKeyPress : MonoBehaviour
{
    void Update()
    {
        if (Input.anyKey)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
