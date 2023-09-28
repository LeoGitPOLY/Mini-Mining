using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface panel
{
    void demarer();
}
public class PanelStarter : MonoBehaviour
{
    [SerializeField] private List<GameObject> panels;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject item in panels)
        {
            panel p = item.GetComponent<panel>();
            p.demarer();
        }
    }

}
