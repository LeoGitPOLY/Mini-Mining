using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EasyComponentsGetter : MonoBehaviour
{
    [Header("Components:")]
    [SerializeField] private TextMeshProUGUI[] txts;
    [SerializeField] private Image[] images;
    [SerializeField] private GameObject[] gameObjects;

    [Header("Asset:")]
    [SerializeField] private Sprite[] sprites;

    public TextMeshProUGUI getTxt(int index)
    {
        return txts[index];
    }

    public Image getImage(int index)
    {
        return images[index];
    }
    public GameObject getGameObject(int index)
    {
        return gameObjects[index];
    }

    public void setActiveGameObject(int index, bool isActive)
    {
        gameObjects[index].SetActive(isActive);
    }

    public Sprite getSprite(int index)
    {
        return sprites[index];
    }

}
