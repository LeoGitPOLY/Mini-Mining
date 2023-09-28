using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImprouvementManager : MonoBehaviour
{
    public static ImprouvementManager instance = null;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
    }

    public int DigLevel;
    public int LightLevel;
    public int CargoLevel;
    public int ChainsLevel;
    public int FuelLevel;

    //Dig:
    [Header("DIG:")]
    public int[] DigPrix;
    public int[] DigStats;

    //Essence:
    [Header("FUEL:")]
    public int[] FuelPrix;
    public int[] FuelStats;

    //Light:
    [Header("LIGHT:")]
    public int[] LightPrix;
    public int[] LightStats;

    //Cargo:
    [Header("CARGO:")]
    public int[] CargoPrix;
    public int[] CargoStats;

    //Chains:
    [Header("CHAINS:")]
    public int[] ChainsPrix;
    public int[] ChainsStats;
    public int[] TreuilStats;


    private const int max_Gold = 7;
    private const int max = 6;

    public void addLevelByIndex(int index)
    {
        addLevels((EnumLevelName)index);
    }
    public void addLevels(EnumLevelName name)
    {
        switch (name)
        {
            case EnumLevelName.Dig:
                if (DigLevel < max_Gold)
                {
                    DigLevel++;
                    EventsGameScore.instance.changeDig();
                }
                break;
            case EnumLevelName.Light:
                if (LightLevel < max_Gold)
                {
                    LightLevel++;
                    EventsGameScore.instance.changeLight();
                }
                break;
            case EnumLevelName.Cargo:
                if (CargoLevel < max)
                {
                    CargoLevel++;
                    EventsGameScore.instance.changeCargo();
                }
                break;
            case EnumLevelName.Chains:
                if (ChainsLevel < max_Gold)
                {
                    ChainsLevel++;
                    EventsGameScore.instance.changeTreuil();
                }
                break;
            case EnumLevelName.Fuel:
                if (FuelLevel < max)
                {
                    FuelLevel++;
                    ScoreManager.instance.refillFuel();
                    EventsGameScore.instance.changeFuel();
                }
                break;
            default:
                break;
        }
    }
    public int getLevelByNumber(int number)
    {
        switch (number)
        {
            case 0:
                return DigLevel;
            case 1:
                return LightLevel;
            case 2:
                return CargoLevel;
            case 3:
                return ChainsLevel;
            case 4:
                return FuelLevel;
            default:
                return -1;
        }
    }
    public int getLevel(EnumLevelName name)
    {
        switch (name)
        {
            case EnumLevelName.Dig:
                return DigLevel;
            case EnumLevelName.Light:
                return LightLevel;
            case EnumLevelName.Cargo:
                return CargoLevel;
            case EnumLevelName.Chains:
                return ChainsLevel;
            case EnumLevelName.Fuel:
                return FuelLevel;
            default:
                return -1;
        }
    }

    public int getPrice(EnumLevelName name)
    {
        int prix = 0;
        switch (name)
        {
            case EnumLevelName.Dig:
                prix = DigPrix[DigLevel];
                break;
            case EnumLevelName.Light:
                prix = LightPrix[LightLevel];
                break;
            case EnumLevelName.Cargo:
                prix = CargoPrix[CargoLevel];
                break;
            case EnumLevelName.Chains:
                prix = ChainsPrix[ChainsLevel];
                break;
            case EnumLevelName.Fuel:
                prix = FuelPrix[FuelLevel];
                break;
            default:
                break;
        }
        return prix;
    }

    public int getPriceByLevel(EnumLevelName name, int index)
    {
        int prix = 0;

        switch (name)
        {
            case EnumLevelName.Dig:
                prix = DigPrix[index];
                break;
            case EnumLevelName.Light:
                prix = LightPrix[index];
                break;
            case EnumLevelName.Cargo:
                prix = CargoPrix[index];
                break;
            case EnumLevelName.Chains:
                prix = ChainsPrix[index];
                break;
            case EnumLevelName.Fuel:
                prix = FuelPrix[index];
                break;
            default:
                break;
        }
        return prix;
    }

    public int getStats(EnumLevelName name, bool treuil = false)
    {
        int stats = 0;
        switch (name)
        {
            case EnumLevelName.Dig:
                stats = DigStats[DigLevel];
                break;
            case EnumLevelName.Light:
                stats = LightStats[LightLevel];
                break;
            case EnumLevelName.Cargo:
                stats = CargoStats[CargoLevel];
                break;
            case EnumLevelName.Chains:
                if (treuil)
                    stats = TreuilStats[ChainsLevel];
                else
                    stats = ChainsStats[ChainsLevel];
                break;
            case EnumLevelName.Fuel:
                stats = FuelStats[FuelLevel];
                break;
            default:
                break;
        }
        return stats;
    }

    //SAVE METHODE:
    public void saveScore()
    {
        SaveSystem.saveImprouvements(instance);
    }
    public void LoadImprouvements()
    {
        ImprouvementsData data = SaveSystem.loadImprouvements();

        if (data != null)
        {
            DigLevel = data.DigLevel;
            LightLevel = data.LightLevel;
            CargoLevel = data.CargoLevel;
            ChainsLevel = data.ChainsLevel;
            FuelLevel = data.FuelLevel;
        }
        else
        {
            loadNew();
        }
    }
    public void loadNew()
    {
        DigLevel = 0;
        LightLevel = 0;
        CargoLevel = 0;
        ChainsLevel = 0;
        FuelLevel = 0;
    }

    public void loadDevelopperMode()
    {
        DigLevel = 7;
        CargoLevel = 6;
        LightLevel = 7;
        FuelLevel = 6;
        ChainsLevel = 7;

    }
}
