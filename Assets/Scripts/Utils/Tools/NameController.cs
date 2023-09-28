using System.Collections.Generic;
using UnityEngine;

public static class NameController
{
    public static string getRandomName()
    {
        TextAsset textColor = Resources.Load<TextAsset>("Text/Names/NameColors");
        TextAsset textName = Resources.Load<TextAsset>("Text/Names/NameObject");

        List<string> strColor = TextReader.getTextParagraph(textColor);
        List<string> strName = TextReader.getTextParagraph(textName);

        string playerName = "#";

        //SELECTION DE LA COULEUR:
        int randomApparition = (int)Random.Range(0, 10);
        int randomIndexColor = (int)Random.Range(0, strColor.Count);

        if (randomApparition <= 6)
            playerName += strColor[randomIndexColor];

        //SELECTION DU NOM:
        int randomIndexName = (int)Random.Range(0, strName.Count);
        playerName += strName[randomIndexName];

        return playerName;
    }
}
