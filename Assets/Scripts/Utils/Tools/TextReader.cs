using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextReader
{
    public static List<string> getTextParagraph(TextAsset text)
    {
        //Devide by enter, remove enter

        List<string> listText = new List<string>();
        string textString = text.ToString();

        listText.AddRange(textString.Split(char.Parse("\n")));

        for (int i = 0; i < listText.Count; i++)
        {
            String str = listText[i];

            if (str != null && str.Length > 0)
            {
                str = str.Substring(0, str.Length - 1);
            }

            listText[i] = str;
        }

        return listText;
    }
    public static List<string> getTextParagraphBYComma(TextAsset text)
    {
        //Devide by comma, remove comma and other last Char
        //Use specialy for removing the enter
        //NOT FOR THE FIRST ONE

        List<string> listText = new List<string>();
        string textString = text.ToString();

        listText.AddRange(textString.Split(char.Parse(",")));

        for (int i = 0; i < listText.Count; i++)
        {
            String str = listText[i];

            if (str != null && str.Length > 0 && i !=0)
            {
                str = str.Substring(2);
            }

            listText[i] = str;
        }

        return listText;
    }
    public static List<string> getTextParagraphBYSlash(TextAsset text)
    {
        //Devide by slash, remove slash and other last Char
        //Use specialy for removing the enter
        //NOT FOR THE FIRST ONE

        List<string> listText = new List<string>();
        string textString = text.ToString();

        listText.AddRange(textString.Split(char.Parse("/")));

        for (int i = 0; i < listText.Count; i++)
        {
            String str = listText[i];

            if (str != null && str.Length > 0 && i != 0)
            {
                str = str.Substring(2);
            }

            listText[i] = str;
        }

        return listText;
    }

    public static List<string> getTextSentenceBYSpace(string textString)
    {
        //Devide by space, remove space

        List<string> listText = new List<string>();

        listText.AddRange(textString.Split(char.Parse(" ")));

        return listText;
    }
}
