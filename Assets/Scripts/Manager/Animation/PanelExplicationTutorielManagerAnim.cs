using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelExplicationTutorielManagerAnim : MonoBehaviour
{
    [SerializeField] private TextAsset ExplicationTutoriel;
    
    [SerializeField] private GameObject flipableSection;
    [SerializeField] private GameObject[] notFlipebale;
    [SerializeField] private List<int> sceneToOnFlip;

    private Animator animator;
    private EasyComponentsGetter getter;
    private List<string> stringsTutoriel;

    private const string NAME_ANIM_CYCLE = "ExplicationTutorielCycle";
    private const string NAME_ANIM_ONE_TIME = "ExplicationTutorielOneTime";

    private int layerIndexCycle;
    private int layerIndexOneTime;

    private bool firstStart = true;

    private void firstStartMethode()
    {
        animator = GetComponent<Animator>();
        getter = GetComponent<EasyComponentsGetter>();

        layerIndexCycle = animator.GetLayerIndex("CycleLayer");
        layerIndexOneTime = animator.GetLayerIndex("OneTimeLayer");

        stringsTutoriel = TextReader.getTextParagraph(ExplicationTutoriel);

        if (SettingManager.instance.LeftHandedControle)
        {
            flipableSection.transform.localScale = new Vector3(-1, 1, 1);

            foreach (var item in notFlipebale)
            {
                item.transform.localScale = new Vector3(-1, 1, 1);
            }
        }

      firstStart = false;
    }

    public void setExplication(int index)
    {
        if (firstStart)
            firstStartMethode();

        gameObject.SetActive(true);

        if(sceneToOnFlip.Contains(index))
            flipableSection.transform.localScale = new Vector3(1, 1, 1);
       
        string nameAnimCycle = NAME_ANIM_CYCLE + index.ToString();
        string nameAnimOneTime = NAME_ANIM_ONE_TIME + index.ToString();

        getter.getTxt(0).SetText(stringsTutoriel[index - 1]);

        animator.Play(nameAnimCycle, layerIndexCycle);
        animator.Play(nameAnimOneTime, layerIndexOneTime);

    }
    public void setInfo(int index)
    {
        if (firstStart)
            firstStartMethode();

        gameObject.SetActive(true);

        if (sceneToOnFlip.Contains(index))
            flipableSection.transform.localScale = new Vector3(1, 1, 1);

        string nameAnimCycle = NAME_ANIM_CYCLE + index.ToString();
        string nameAnimOneTime = "ExplicationInstant";

        getter.getTxt(0).SetText(stringsTutoriel[index - 1]);

        animator.Play(nameAnimCycle, layerIndexCycle);
        animator.Play(nameAnimOneTime, layerIndexOneTime);

    }

    public void closeAll()
    {
        animator.StopPlayback();
        gameObject.SetActive(false);
    }
    
}
