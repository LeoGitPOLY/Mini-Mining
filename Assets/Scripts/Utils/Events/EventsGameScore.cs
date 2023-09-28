using UnityEngine;
using System;

public class EventsGameScore : MonoBehaviour
{
    public static EventsGameScore instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
    }

    public event Action<int> onEssence;
    public void noEssence(int pourcentage)
    {
        onEssence?.Invoke(pourcentage);
    }


    public event Action OnNewBonus;
    public void newBonus()
    {
        OnNewBonus?.Invoke();
    }

    public event Action<EnumCell> onFindNewBlock;
    public void findNewBlock(EnumCell type)
    {
        onFindNewBlock?.Invoke(type);
    }

    /**
     * ON CHANGE:
     */
    //Gold:
    public event Action onChangeGold;
    public void changeGold()
    {
        onChangeGold?.Invoke();
    }

    //Treuil:
    public event Action onChangeTreuil;
    public void changeTreuil()
    {
        onChangeTreuil?.Invoke();
    }

    //Cargo:
    public event Action onChangeCargo;
    public void changeCargo()
    {
        onChangeCargo?.Invoke();
    }

    //Dig:
    public event Action onChangeDig;
    public void changeDig()
    {
        onChangeDig?.Invoke();
    }

    //Fuel:
    public event Action onChangeFuel;
    public void changeFuel()
    {
        onChangeFuel?.Invoke();
    }

    //Light
    public event Action onChangeLight;
    public void changeLight()
    {
        onChangeLight?.Invoke();
    }
}
