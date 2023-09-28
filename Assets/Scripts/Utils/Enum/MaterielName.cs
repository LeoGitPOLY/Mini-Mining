using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MaterielName
{
    public static string getNameMat(EnumCell type)
    {
        string name = "";
        switch (type)
        {
            case EnumCell.Rien:
                name = "Empty Bloc";
                break;
            case EnumCell.Dirt:
                name = "Dirt";
                break;
            case EnumCell.Grass:
                name = "Grass";
                break;
            case EnumCell.Coal:
                name = "Coal";
                break;
            case EnumCell.Copper:
                name = "Copper";
                break;
            case EnumCell.Stone:
                name = "Stone";
                break;
            case EnumCell.Quartz:
                name = "Quartz";
                break;
            case EnumCell.Pink_Gold:
                name = "Pink gold";
                break;
            case EnumCell.Gold:
                name = "Gold";
                break;
            case EnumCell.Corundum:
                name = "Corundum";
                break;
            case EnumCell.Silver:
                name = "Silver";
                break;
            case EnumCell.Emerald:
                name = "Emerald";
                break;
            case EnumCell.Ruby:
                name = "Ruby";
                break;
            case EnumCell.Obsidian:
                name = "Obsidian";
                break;
            case EnumCell.Black_Onyx:
                name = "Black onyx";
                break;
            case EnumCell.Diamond:
                name = "Diamond";
                break;
            case EnumCell.Treasure:
                break;
            case EnumCell.Treuil:
                break;
            case EnumCell.Chain:
                break;
            case EnumCell.Platefrom:
                break;
            case EnumCell.Mystery:
                break;
            case EnumCell.Magma:
                break;
            case EnumCell.Cursed:
                break;
            case EnumCell.BedRock:
                break;
            default:
                break;
        }

        return name;
    }
    public static string getShortNameMat(EnumCell type)
    {
        string name = "";
        switch (type)
        {
            case EnumCell.Rien:
                name = "Empty Bloc";
                break;
            case EnumCell.Dirt:
                name = "Dirt";
                break;
            case EnumCell.Grass:
                name = "Grass";
                break;
            case EnumCell.Coal:
                name = "Coal";
                break;
            case EnumCell.Copper:
                name = "Copper";
                break;
            case EnumCell.Stone:
                name = "Stone";
                break;
            case EnumCell.Quartz:
                name = "Quartz";
                break;
            case EnumCell.Pink_Gold:
                name = "P.gold";
                break;
            case EnumCell.Gold:
                name = "Gold";
                break;
            case EnumCell.Corundum:
                name = "Cor.";
                break;
            case EnumCell.Silver:
                name = "Silver";
                break;
            case EnumCell.Emerald:
                name = "Emerald";
                break;
            case EnumCell.Ruby:
                name = "Ruby";
                break;
            case EnumCell.Obsidian:
                name = "Obsi.";
                break;
            case EnumCell.Black_Onyx:
                name = "B.onyx";
                break;
            case EnumCell.Diamond:
                name = "Diamond";
                break;
            case EnumCell.Treasure:
                break;
            case EnumCell.Treuil:
                break;
            case EnumCell.Chain:
                break;
            case EnumCell.Platefrom:
                break;
            case EnumCell.Mystery:
                break;
            case EnumCell.Magma:
                break;
            case EnumCell.Cursed:
                break;
            case EnumCell.BedRock:
                break;
            default:
                break;
        }

        return name;
    }
}
