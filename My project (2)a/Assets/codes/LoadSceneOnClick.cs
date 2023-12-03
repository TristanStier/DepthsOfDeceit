using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour
{
    // Specify the name of the scene to be loaded
    public string sceneName;

    // This method is called when the button is clicked
    public void LoadScene()
    {
        // Load the specified scene
        SceneManager.LoadScene(sceneName);
    }
}