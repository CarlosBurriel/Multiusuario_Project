using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RegisterManager : MonoBehaviour
{
    public InputField usernameField;
    public InputField passwordField;
    public Text resultText;

    public void StartRegister()
    {
        print("hola");
        StartCoroutine(Register());
    }

    IEnumerator Register()
    {
        
        WWWForm form = new WWWForm();
        form.AddField("username", usernameField.text);
        form.AddField("password", passwordField.text);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/unity_api/register_duplicate.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                resultText.text = "Error: " + www.error;
            }
            else
            {
                string responseText = www.downloadHandler.text;
                Debug.Log(responseText);
                if (responseText.Contains("success"))
                {
                    SceneManager.LoadScene("StartMenu");
                    //resultText.text = "Register successful!";
                }
                //==Este else if es añadido para ver si esta duplicado===
                else if(responseText.Contains("duplicate"))
                {
                    resultText.text = "Register failed! Duplicated value";
                }
                //===================================================
                else
                {
                    resultText.text = "Register failed!";
                }
            }
        }
    }
}
