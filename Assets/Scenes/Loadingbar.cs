using UnityEngine;
using UnityEngine.UI;
//using static System.Net.Mime.MediaTypeNames;

public class LoadingBar : MonoBehaviour
{
    public Image loadingFill;
    public CrystalGlow crystal;
    public float loadSpeed = 0.7f;

    private float progress = 0f;

    void Start()
    {
        loadingFill.fillAmount = 0f;
    }

    void Update()
    {
        if (progress < 1f)
        {
            progress += Time.deltaTime * loadSpeed;
            loadingFill.fillAmount = progress;
        }
        else
        {
            // Start glow only once
            if (!crystal.enabled) return;

            crystal.enabled = true;
            crystal.StartGlow();
        }
    }
}
