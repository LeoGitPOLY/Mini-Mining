using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class NotExploreTiles
{
    public string name;
    public Sprite sprite;
}
public class MuseumMap : MonoBehaviour
{
    [SerializeField] CellTheme theme;
    [SerializeField] Image mapImage;

    [SerializeField] NotExploreTiles[] notExploreTiles;
    private Dictionary<string, Sprite> notExploreDic;
    private bool hasStart = false;

    private void OnEnable()
    {
        if (!hasStart)
        {
            notExploreDic = new Dictionary<string, Sprite>();
            foreach (NotExploreTiles item in notExploreTiles)
            {
                notExploreDic.Add(item.name, item.sprite);
            }
            hasStart = true;
        }
        redrawMap();
    }

    public void redrawMap()
    {
        Texture2D text2D = CreateImage.createdMapPixelLowRez(theme, notExploreDic);
        Sprite sprite = Sprite.Create(text2D, new Rect(0, 0, text2D.width, text2D.height), new Vector2(0.5f, 0.5f));

        mapImage.sprite = sprite;
    }


}
