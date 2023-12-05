using UnityEngine;


public class LightControl : MonoBehaviour
{
    public UnityEngine.Rendering.Universal.Light2D playerLight; // Assuming you are using Light2D instead of Unity's built-in Light

    public string impostorTag = "Impostor";

    void Update()
    {
        // Check if the player has the tag "Impostor"
        if (gameObject.CompareTag(impostorTag))
        {
            // Adjust inner and outer radius of the light
            if (playerLight != null)
            {
                playerLight.pointLightInnerRadius = 2f;
                playerLight.pointLightOuterRadius = 4f;
            }
        }
        // You can add an else statement if you want to handle a different behavior for non-Impostor players
    }
}
