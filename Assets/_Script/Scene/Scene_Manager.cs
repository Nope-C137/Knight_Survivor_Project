using UnityEngine;
using UnityEngine.SceneManagement;

namespace TD
{
    public class Scene_Manager : MonoBehaviour
    {
        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }

}