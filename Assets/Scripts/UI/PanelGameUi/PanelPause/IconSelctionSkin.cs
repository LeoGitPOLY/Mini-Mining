using System.Collections.Generic;
using UnityEngine;

public class IconSelctionSkin : MonoBehaviour
{
    [SerializeField] private SkinTheme skins;
    [SerializeField] private GameObject prefabSkin;
    [SerializeField] private Transform parent;

    [Header("Color:")]
    [SerializeField] private Color colorBuy;
    [SerializeField] private Color colornotBuy;

    [Header("Getter:")]
    [SerializeField] private EasyComponentsGetter getter;
    [SerializeField] private PanelLeaderBoardManager panelLeader;

    private List<GameObject> allPrefabSkin;


    private void OnEnable()
    {
        redrawPanel();
    }

    private void redrawPanel()
    {
        if (allPrefabSkin == null)
        {
            allPrefabSkin = new List<GameObject>();

            foreach (var item in skins.allSkin)
            {
                GameObject prefab = Instantiate(prefabSkin, parent);

                //Add tabs general:
                GeneralTabs tabs = prefab.GetComponent<GeneralTabs>();
                tabs.IndexTab = (int)item.SkinType;
                tabs.MethodeToRun = selectPlayerIcon;


                prefab.name = item.NameSkin;
                allPrefabSkin.Add(prefab);
            }
        }

        for (int i = 0; i < allPrefabSkin.Count; i++)
        {
            EasyComponentsGetter getter = allPrefabSkin[i].GetComponent<EasyComponentsGetter>();

            getter.getImage(1).sprite = skins.allSkin[i].SpriteToShow;

            if (SettingManager.instance.skinsUnlock[(int)skins.allSkin[i].SkinType] != 1)
                getter.getImage(1).color = colornotBuy;
            else
                getter.getImage(1).color = colorBuy;
        }

        GameObject inter = allPrefabSkin[SettingManager.instance.indexIconSelected];
        EasyComponentsGetter getterInter = inter.GetComponent<EasyComponentsGetter>();

        getterInter.getImage(0).color = new Color(255, 255, 255, 1);
        getter.getImage(0).sprite = skins.allSkin[SettingManager.instance.indexIconSelected].SpriteToShow;
    }

    private void selectPlayerIcon(int index)
    {
        if (SettingManager.instance.skinsUnlock[index] == 1)
        {
            EasyComponentsGetter getterOld = allPrefabSkin[SettingManager.instance.indexIconSelected].GetComponent<EasyComponentsGetter>();
            EasyComponentsGetter getterNew = allPrefabSkin[index].GetComponent<EasyComponentsGetter>();

            getterOld.getImage(0).color = new Color(255, 255, 255, 0);
            getterNew.getImage(0).color = new Color(255, 255, 255, 1);
            getter.getImage(0).sprite = skins.allSkin[index].SpriteToShow;


            SettingManager.instance.indexIconSelected = index;
            panelLeader.ChangeIconLeaderBoard(index);
            panelLeader.restartLeaderBoard();
            getter.getGameObject(0).SetActive(false);
        }
    }
}
