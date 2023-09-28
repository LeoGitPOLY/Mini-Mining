using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum typeMort
{
    Cursed,
    Chains,
    Fuel,
    Quit,
}
public class EventsGameState : MonoBehaviour
{
    public static EventsGameState instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
    }

    private bool isDead = false;

    public event Action<typeMort> onDeadType;
    public event Action onDead;

    public void dead(typeMort type)
    {
        if (isDead)
            return;
        /*Callback for:
        * UI Manager
        * Save Manager
        */
        isDead = true;

        onDeadType?.Invoke(type);
        onDead?.Invoke();

        if (type == typeMort.Cursed)
            ScoreManager.instance.isDeadNotSaveble = true;
    }

    public event Action onRevive;
    public void revive()
    {
        /*Callback for:
         * Save Manager(Take care of all Managers)
         */
        isDead = false;

        ScoreManager.instance.isDeadNotSaveble = false;
        onRevive?.Invoke();
    }

    public event Action onRestart;
    public void restart()
    {
        /*Callback for:
         * UI Manager
         * Save (Take care of all Managers)
         */
        ScoreManager.instance.isDeadNotSaveble = false;
        onRestart?.Invoke();
        SceneManager.LoadScene(SceneName.SCENE_LOADING);
    }

    public event Action onFinish;
    public void finish()
    {
        /*Callback for:
         * Save (Take care of all Managers)
         */
        onFinish?.Invoke();
    }

    public event Action<int> onTeleport;
    public void teleport(int indexTeleport)
    {
        /*Callback for:
         * Save (Take care of all Managers)
         */
        onTeleport?.Invoke(indexTeleport);
    }
}
