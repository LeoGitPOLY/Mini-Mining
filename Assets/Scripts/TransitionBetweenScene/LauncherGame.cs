using UnityEngine;
using UnityEngine.SceneManagement;

public class LauncherGame : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        loadSceneToStart();
    }

    private void loadSceneToStart()
    {
        PlayerData data = SaveSystem.loadPlayer();

        if (data == null)
            SceneManager.LoadScene(SceneName.SCENE_BD_SCINEMATIC);
        else
            SceneManager.LoadScene(SceneName.SCENE_LOADING);
    }
}
