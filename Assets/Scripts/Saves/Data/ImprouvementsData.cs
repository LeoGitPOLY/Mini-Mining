
[System.Serializable]
public class ImprouvementsData
{
    public int DigLevel;
    public int LightLevel;
    public int CargoLevel;
    public int ChainsLevel;
    public int FuelLevel;


    public ImprouvementsData(ImprouvementManager improuv)
    {
        DigLevel = improuv.DigLevel;
        LightLevel = improuv.LightLevel;
        CargoLevel = improuv.CargoLevel;
        ChainsLevel = improuv.ChainsLevel;
        FuelLevel = improuv.FuelLevel;
    }
}
