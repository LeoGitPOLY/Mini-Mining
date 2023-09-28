using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class torcheManager : MonoBehaviour
{
    [SerializeField] GameObject player;

    [Header("Variable modification:")]
    [SerializeField] private float maxRadius;
    [SerializeField] private float minRadius;
    [SerializeField] private int DimmerRadius;
    [SerializeField] private float DifferenceOuterInner;

    [Space()]
    [SerializeField] private float maxIntensity;
    [SerializeField] private float minIntensity;
    [SerializeField] private int DimmerIntensity;

    private UnityEngine.Rendering.Universal.Light2D Light;
    private float positionY;
    private bool localBoolLight;
    private bool isScenePrincipal;



    // Start is called before the first frame update
    void Start()
    {
        positionY = (int)player.transform.position.y;
        Light = GetComponent<UnityEngine.Rendering.Universal.Light2D>();

        isScenePrincipal = SceneName.SCENE_PRINCIPALE == SceneManager.GetActiveScene().name;
    }

    // Update is called once per frame
    void Update()
    {
        if (localBoolLight)
            Light.enabled = true;
        else
            Light.enabled = false;

        lightController();

        if (isScenePrincipal)
            decouverteController();
    }

    private void lightController()
    {
        float pos = Mathf.Abs(player.transform.position.y);

        int niv = ImprouvementManager.instance.getStats(EnumLevelName.Light);

        float outerRadius = ((pos - niv) * (minRadius - maxRadius)) / DimmerRadius + minRadius;
        outerRadius = Mathf.Clamp(outerRadius, minRadius, maxRadius);

        Light.pointLightInnerRadius = outerRadius - DifferenceOuterInner;
        Light.pointLightOuterRadius = outerRadius;

        niv = niv + DimmerIntensity;

        float intensity = ((pos - niv) * (minIntensity - maxIntensity)) / DimmerRadius + minIntensity;
        intensity = Mathf.Clamp(intensity, minIntensity, maxIntensity);

        Light.intensity = intensity;
    }

    private void decouverteController()
    {
        Vector2Int pos = Utils.PositionInt(player.transform);
        int innerRange = (int)Mathf.Round(Light.pointLightInnerRadius);

        int rangePos = Utils.getRangeFromXY(pos.x, pos.y);
        if (rangePos < innerRange && rangePos != -2)
        {
            Utils.setRangeGridFromXY(pos.x, pos.y, innerRange);
            Utils.setExplorationFromXY(pos.x, pos.y, 1);

            for (int i = -innerRange; i <= innerRange; i++)
            {
                for (int j = -innerRange; j <= innerRange; j++)
                {
                    if (!(innerRange >= 3 && (i == innerRange || i == -innerRange || j == innerRange || j == -innerRange)))
                    {
                        if (Utils.getExplorationFromXY(pos.x + i, pos.y + j) == 0)
                        {
                            Utils.setExplorationFromXY(pos.x + i, pos.y + j, 1);
                        }

                    }
                }
            }
        }
    }

    public void setBoolLight(bool isActive)
    {
        localBoolLight = isActive;
    }
}
