using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    public InputField usernameField;
    public InputField passwordField;
    public Text resultText;

    public void StartLogin()
    {
        StartCoroutine(Login());
    }

    IEnumerator Login()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", usernameField.text);
        form.AddField("password", passwordField.text);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/unity_api/login.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                resultText.text = "Error: " + www.error;
            }
            else
            {
                string responseText = www.downloadHandler.text;
                if (responseText.Contains("success"))
                {
                    SceneManager.LoadScene("StartMenu");
                    //resultText.text = "Login successful!";
                }
                else
                {
                    resultText.text = "Login failed!";
                }
            }
        }
    }
}

