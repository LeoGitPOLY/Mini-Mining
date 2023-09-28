using UnityEngine;

public class TutorielParser : MonoBehaviour
{
    private bool isLeftHanded;
    public bool IsLeftHanded { get => isLeftHanded; set => isLeftHanded = value; }

    public bool loadInfoParser()
    {
        print("load: " + isLeftHanded);
        return isLeftHanded;
    }

    public void deleteParser()
    {
        Destroy(gameObject);
    }
}
