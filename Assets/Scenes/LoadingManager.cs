using System.Collections; // Required for Coroutines
//using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    // --- Public Fields to Assign in the Unity Inspector ---

    [Header("UI Elements")]
    // Assign the RectTransform of your Crystal Image here
    public RectTransform crystalRT;

    // Assign the CanvasGroup that holds your Logo, Loading Bar, and other UI (to fade them out)
    public CanvasGroup loadingCanvasGroup;

    [Header("Animation Settings")]
    public string nextSceneName = "HomePageScene"; // Change this to your actual home page scene name
    public float centerMoveDuration = 0.5f; // Time for the crystal to move to the center
    public float zoomScale = 1.5f; // How much the crystal zooms in (e.g., 1.5x)
    public float zoomDuration = 0.3f; // Time for the zoom effect
    public float ascendDuration = 1.0f; // Time for the crystal to move off-screen

    // --- Internal Variables ---
    private Vector2 originalCrystalPos;

    void Awake()
    {
        // Store the crystal's starting position when the scene loads
        if (crystalRT != null)
        {
            originalCrystalPos = crystalRT.anchoredPosition;
            Debug.Log("AWAKE: Crystal starting position: " + originalCrystalPos); // LOG 1: शुरुआती पोजीशन
        }
    }

    // --- This is the function called by your LoadingBar.cs when load is 100% ---
    public void StartCrystalTransition()
    {
        UnityEngine.Debug.Log("Loading Complete. Starting Crystal Transition...");
        // Ensure the loading bar content is hidden/faded out before starting the main animation
        // (Assuming LoadingBar.cs handles hiding the fill image itself)

        StartCoroutine(CrystalTransitionSequence());
    }

    // --- The Main Animation Logic ---
    IEnumerator CrystalTransitionSequence()
    {
        // 1. Move to Center 
        Vector2 centerTarget = Vector2.zero; // Center of screen for UI
        float startTime = Time.time;

        while (Time.time < startTime + centerMoveDuration)
        {
            float t = (Time.time - startTime) / centerMoveDuration;
            // Lerp (Linear Interpolation) moves the position smoothly
            crystalRT.anchoredPosition = Vector2.Lerp(originalCrystalPos, centerTarget, t);
            yield return null; // Wait until next frame
        }
        crystalRT.anchoredPosition = centerTarget; // Ensure precise positioning

        // Add code here to trigger the crystal's peak glow and particle burst!

        // 2. Zoom and Wait
        startTime = Time.time;
        Vector3 originalScale = crystalRT.localScale;
        Vector3 zoomedScale = Vector3.one * zoomScale;

        // Zoom In
        while (Time.time < startTime + zoomDuration)
        {
            float t = (Time.time - startTime) / zoomDuration;
            crystalRT.localScale = Vector3.Lerp(originalScale, zoomedScale, t);
            yield return null;
        }
        crystalRT.localScale = zoomedScale;

        // Pause for impact (0.5 seconds)
        yield return new WaitForSeconds(0.5f);

        // 3. Ascend and Load Scene

        // Calculate the position far above the screen
        // We add 200 units to ensure it goes completely off-screen
        Vector2 offScreenTarget = new Vector2(0, Screen.height / 2 + 200);

        startTime = Time.time;

        while (Time.time < startTime + ascendDuration)
        {
            float t = (Time.time - startTime) / ascendDuration;

            // Crystal ascends
            crystalRT.anchoredPosition = Vector2.Lerp(centerTarget, offScreenTarget, t);

            // Other UI elements fade out (for a smooth wipe-up/fade-out effect)
            if (loadingCanvasGroup != null)
            {
                // Fades faster than the movement for a clean look
                loadingCanvasGroup.alpha = Mathf.Lerp(1f, 0f, t * 1.5f);
            }

            yield return null;
        }

        // Final step: Load the next scene
        SceneManager.LoadScene("LoginPage");
    }
}