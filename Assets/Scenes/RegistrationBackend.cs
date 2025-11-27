using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Registration : MonoBehaviour
{
    public TMP_InputField playerId;
    public TMP_InputField email;
    public TMP_InputField password;
    public TMP_InputField confirmPassword;

    public GameObject errorPopup;     // Popup panel
    public TMP_Text popupMessage;     // Popup message text

    public DatabaseManager db;

    public void RegisterNow()
    {
        // 1. Empty fields check
        if (string.IsNullOrEmpty(playerId.text) ||
            string.IsNullOrEmpty(email.text) ||
            string.IsNullOrEmpty(password.text) ||
            string.IsNullOrEmpty(confirmPassword.text))
        {
            ShowPopup("All fields are required!");
            return;
        }

        // 2. Player ID check
        if (playerId.text.Length < 3)
        {
            ShowPopup("Player ID must be at least 3 characters long!");
            return;
        }

        // 3. Email format check
        if (!IsValidEmail(email.text))
        {
            ShowPopup("Invalid email format!");
            return;
        }

        // 4. Password length
        if (password.text.Length < 6)
        {
            ShowPopup("Password must be at least 6 characters long!");
            return;
        }

        // 5. Password match check
        if (password.text != confirmPassword.text)
        {
            ShowPopup("Passwords do not match!");
            return;
        }

        // 6. Register user in database
        bool success = db.RegisterUser(playerId.text, email.text, password.text);

        if (!success)
        {
            ShowPopup("User already exists. Try a different Player ID.");
            return;
        }

        // 7. Successful registration → go to Login page
        SceneManager.LoadScene("LoginPage");
    }

    // ==== POPUP FUNCTION ====
    private void ShowPopup(string msg)
    {
        popupMessage.text = msg;
        errorPopup.SetActive(true);   // show the popup on screen
    }

    // ==== EMAIL CHECK ====
    private bool IsValidEmail(string email)
    {
        try
        {
            var mail = new System.Net.Mail.MailAddress(email);
            return mail.Address == email;
        }
        catch
        {
            return false;
        }
    }
    // ==== POPUP CLOSE FUNCTION ====
    public void ClosePopup()
    {
        errorPopup.SetActive(false);
    }

}
