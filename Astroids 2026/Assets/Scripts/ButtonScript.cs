using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
     
    public void LoadLevel(string p = "")
    {
        SceneManager.LoadScene(p);
    }

    public void OnQuit()
    {
        Application.Quit();
    }
}
