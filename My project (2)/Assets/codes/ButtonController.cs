using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    // Specify the name of the scene you want to load
    public string sampleSceneName = "SampleScene";

    // Start is called before the first frame update
    void Start()
    {
        // Get the Button component attached to this GameObject
        Button button = GetComponent<Button>();

        // Add a listener to the button's click event
        button.onClick.AddListener(TaskOnClick);
    }

    // This method is called when the button is clicked
    void TaskOnClick()
    {
        // Load the specified scene
        SceneManager.LoadScene(sampleSceneName);
    }
}
