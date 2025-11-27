using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Login : MonoBehaviour
{
    public TMP_InputField playerId;
    public TMP_InputField password;

    public GameObject errorPopup;
    public TMP_Text popupMessage;

    public DatabaseManager db;

    public void LoginNow()
    {
        // Empty field validation
        if (string.IsNullOrEmpty(playerId.text) || string.IsNullOrEmpty(password.text))
        {
            ShowPopup("Please fill all fields!");
            return;
        }

        // Check Login
        bool success = db.LoginUser(playerId.text, password.text);

        if (success)
        {
            SceneManager.LoadScene("homeSence");
        }
        else
        {
            ShowPopup("Invalid PlayerID or Password!");
        }
    }

    public void OpenRegisterPage()
    {
        SceneManager.LoadScene("registrationPage");
    }

    // Show popup
    private void ShowPopup(string msg)
    {
        popupMessage.text = msg;
        errorPopup.SetActive(true);
    }

    // Close popup
    public void ClosePopup()
    {
        errorPopup.SetActive(false);
    }
}
