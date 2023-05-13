using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadSceneFromString(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
