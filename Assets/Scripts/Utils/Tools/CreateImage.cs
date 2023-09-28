using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class CreateImage
{
    public static Texture2D createdMapPixel(CellTheme cells)
    {
        Texture2D brush = new Texture2D(Utils.gridMonde.GetLength(1), Utils.gridMonde.GetLength(0), TextureFormat.RGB565, false);
        Color color1 = Color.black;
        Color color2 = new Color(0.1f, 0.1f, 0.1f);

        brush.filterMode = FilterMode.Point;

        for (int i = 0; i < brush.width; i++)
        {
            for (int j = 0; j < brush.height; j++)
            {
                Color color;
                if (Utils.getExplorationGrid(j, i) == 1)
                {
                    int numberBloc = Utils.getTypeGrid(j, i);
                    color = cells.allCell[numberBloc].color;
                }
                else
                {
                    if (j % 2 == 0)
                        color = color1;
                    else
                        color = color2;
                }

                brush.SetPixel(i, j, color);
            }
        }
        brush.Apply(true);

        return brush;
    }

    public static Texture2D createdMapPixelLowRez(CellTheme cells, Dictionary<string, Sprite> notExplore)
    {
        Dictionary<string, Texture2D> notExploreTextures = new Dictionary<string, Texture2D>();
        foreach (var item in notExplore)
        {
            notExploreTextures.Add(item.Key, textureFromSprite(item.Value));
        }
        Texture2D[] allTexture2D = new Texture2D[cells.allCell.Length];

        Texture2D brush = new Texture2D(Utils.gridMonde.GetLength(1) * notExploreTextures["Center"].width, Utils.gridMonde.GetLength(0) * notExploreTextures["Center"].height, TextureFormat.RGB565, false);
        Color colorOfset;

        brush.filterMode = FilterMode.Point;

        //Transformer all Sprite to Texture2D:
        foreach (Cell cell in cells.allCell)
        {
            Sprite spr = cell.lowRezTexture;
            allTexture2D[(int)cell.typeCell] = textureFromSprite(spr);

        }

        //BOUCLE POUR LES BLOCS:
        for (int i = 0; i < brush.width; i = i + notExploreTextures["Center"].width)
        {
            for (int j = 0; j < brush.height; j = j + notExploreTextures["Center"].height)
            {
                int numberBloc = Utils.getTypeGrid(j / notExploreTextures["Center"].width, i / notExploreTextures["Center"].height);
                Texture2D localText;
                Color color;         

                //UP, right, down, left:
                List<char> cote = new List<char> {'n','n', 'n', 'n'};

                int nbPixel = notExploreTextures["Center"].width;
                if (Utils.getExplorationGrid(j / nbPixel, i / nbPixel) != 0)
                {
                    localText = allTexture2D[numberBloc];
                    colorOfset = new Color(0, 0, 0);
                }
                else
                {
                    if ((j/nbPixel) % 10 == 0)
                        colorOfset = new Color(0.01f, 0.01f, 0.01f);
                    else
                        colorOfset = new Color(0, 0, 0);

                    if (Utils.getExplorationGrid(j / nbPixel - 1, i / nbPixel) != 0)
                        cote[0] = 'y';
                    if (Utils.getExplorationGrid(j / nbPixel, i / nbPixel -1) != 0)
                        cote[1] = 'y';
                    if (Utils.getExplorationGrid(j / nbPixel + 1, i / nbPixel) != 0)
                        cote[2] = 'y';
                    if (Utils.getExplorationGrid(j / nbPixel, i / nbPixel+1) != 0)
                        cote[3] = 'y';

                    string result = new string(cote.ToArray());

                    switch (result)
                    {
                        //One line:
                        case "nnnn":
                            localText = notExploreTextures["Center"];
                            break;
                        case "ynnn":
                            localText = notExploreTextures["Up"];
                            break;
                        case "nynn":
                            localText = notExploreTextures["Right"];
                            break;
                        case "nnyn":
                            localText = notExploreTextures["Down"];
                            break;
                        case "nnny":
                            localText = notExploreTextures["Left"];
                            break;

                        //Coins:
                        case "yynn":
                            localText = notExploreTextures["CoinUpRight"];
                            break;
                        case "nyyn":
                            localText = notExploreTextures["CoinRightDown"];
                            break;
                        case "nnyy":
                            localText = notExploreTextures["CoinDownLeft"];
                            break;
                        case "ynny":
                            localText = notExploreTextures["CoinLeftUp"];
                            break;

                        //U avec espace:
                        case "nyyy":
                            localText = notExploreTextures["USpaceUp"];
                            break;
                        case "ynyy":
                            localText = notExploreTextures["USpaceRight"];
                            break;
                        case "yyny":
                            localText = notExploreTextures["USpaceDown"];
                            break;
                        case "yyyn":
                            localText = notExploreTextures["USpaceLeft"];
                            break;

                        //Deux lignes:
                        case "ynyn":
                            localText = notExploreTextures["UpDown"];
                            break;
                        case "nyny":
                            localText = notExploreTextures["LeftRight"];
                            break;

                        //All around:
                        case "yyyy":
                            localText = notExploreTextures["AllAround"];
                            break;

                        default:
                            localText = notExploreTextures["Center"];
                            break;
                    }
                }

                //BOUCLE POUR LES PIXELS DANS L'ICON:
                for (int i_inter = 0; i_inter < notExploreTextures["Center"].width; i_inter++)
                {
                    for (int j_inter = 0; j_inter < notExploreTextures["Center"].height; j_inter++)
                    {
                        color = localText.GetPixel(i_inter, j_inter);
                        color += colorOfset;

                        brush.SetPixel(i + (notExploreTextures["Center"].width - (i_inter + 1)), j + (notExploreTextures["Center"].height - (j_inter + 1)), color);
                    }
                }

            }
        }
        brush.Apply(true);

        return brush;
    }

    public static Texture2D textureFromSprite(Sprite sprite)
    {
        if (sprite.rect.width != sprite.texture.width)
        {
            Texture2D newText = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
            Color[] newColors = sprite.texture.GetPixels((int)sprite.textureRect.x,
                                                         (int)sprite.textureRect.y,
                                                         (int)sprite.textureRect.width,
                                                         (int)sprite.textureRect.height);
            newText.SetPixels(newColors);
            newText.Apply();
            return newText;
        }
        else
            return sprite.texture;
    }
}
