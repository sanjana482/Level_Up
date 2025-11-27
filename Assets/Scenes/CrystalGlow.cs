using System.Collections; // For Coroutines
using UnityEngine;
using UnityEngine.UI;

public class CrystalGlow : MonoBehaviour
{
    // New: LoadingManager को असाइन करें
    public LoadingManager loadingManager;
    public Image crystalImage;         // Crystal that will glow (Ensure its color is set to white initially)

    private bool glowActive = false;

    // LoadingBar.cs से कॉल होगा
    public void StartGlow()
    {
        if (loadingManager == null)
        {
            Debug.LogError("LoadingManager not assigned to CrystalGlow script!");
            return;
        }

        glowActive = true;
        // सीधे अगले सीन को लोड करने के बजाय ट्रांज़िशन शुरू करें
        StartCoroutine(GlowDurationAndTransition());
    }

    void Update()
    {
        if (!glowActive) return;

        // Glow pulse effect (using your existing logic)
        // Note: Color.cyan is a placeholder; use your #33FFFF color if possible
        float pulse = Mathf.PingPong(Time.time * 2f, 1f);
        crystalImage.color = Color.Lerp(Color.white, Color.cyan, pulse);
    }

    IEnumerator GlowDurationAndTransition()
    {
        // 1. क्रिस्टल को चमकने दें (जैसे 1.2 सेकंड तक)
        yield return new WaitForSeconds(1.2f);

        // 2. चमकना बंद करें (ताकि वह ट्रांज़िशन के दौरान सिर्फ़ ऊपर उठे)
        glowActive = false;

        // 3. LoadingManager को एनिमेशन शुरू करने के लिए कहें
        loadingManager.StartCrystalTransition();

        // Note: SceneManager.LoadScene() अब LoadingManager में होगा
    }
}