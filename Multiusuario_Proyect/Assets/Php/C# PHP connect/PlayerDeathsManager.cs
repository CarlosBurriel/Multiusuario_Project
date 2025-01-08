using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class PlayerDeathsManager : MonoBehaviour
{
    //On Player Death Send Coroutine

    public IEnumerator PlayerDeath()
    {
        WWWForm form = new WWWForm();
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/unity_api/login.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                //resultText.text = "Error: " + www.error;
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
                    //resultText.text = "Login failed!";
                }
            }
        }
    }
}
