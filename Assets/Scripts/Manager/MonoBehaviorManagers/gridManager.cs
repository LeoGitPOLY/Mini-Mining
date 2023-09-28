using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.SceneManagement;

public class gridManager : MonoBehaviour
{
    public static gridManager instance = null;

    [Header("GameObjectsPrefab")]
    //GameObject
    [SerializeField] private GameObject bonderies;
    [SerializeField] private CellTheme cellPrefab;
    [SerializeField] private GameObject papa;

    //Player:
    [Header("PlayerPosition")]
    [SerializeField] private Transform player;

    // Cellules:
    [Header("Valeur Modifiable")]
    [SerializeField] private Texture2D[] endMaps;
    [SerializeField] private int nbBlocs;
    [SerializeField] private int nbGrilles;

    //Valeurs modifiables:
    [SerializeField] private StageBlocList StageBloc;
    private List<int> listBlocByStage;

    private Cell[,] CellGrid;
    private Cell[,] CellGrid_Inter;

    private Vector2 coin;
    private Vector2 deplacement;

    public double argentTot = 0;
    public int nbBlocsTot = 0;

    // MonoBehaviour Methodes:
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0);

        Vector2 topCoin = new Vector2(coin.x - ((int)(nbGrilles / 2) * nbBlocs), coin.y + ((int)(nbGrilles / 2) * nbBlocs));

        for (int x = 0; x < nbGrilles; x++)
        {
            for (int y = 0; y < nbGrilles; y++)
            {
                Vector2 milieu = new Vector2(topCoin.x + nbBlocs / 2f + nbBlocs * x, topCoin.y - nbBlocs / 2f - nbBlocs * y);
                Gizmos.DrawWireCube(milieu, new Vector3(nbBlocs, nbBlocs, 1));
            }

        }
        Gizmos.color = new Color(1, 0, 0);
        Gizmos.DrawSphere(coin, 0.2f);
    }
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
    }

    void FixedUpdate()
    {
        if (isResize())
            resize();
    }

    //Private function:
    private void setUpGridMonde()
    {
        Utils.largeur = (int)bonderies.transform.localScale.x;
        Utils.hauteur = (int)bonderies.transform.localScale.y;
        Utils.position = bonderies.transform.position;

        //Creation de la grille
        Utils.gridMonde = new string[(int)(Utils.hauteur / 2 - Utils.position.y), Utils.largeur];
        Utils.gridExploration = new string[(int)(Utils.hauteur / 2 - Utils.position.y), Utils.largeur];

        listBlocByStage = StageBloc.getStageTot();

        for (int i = 0; i < Utils.gridMonde.GetLength(0); i++)
        {
            for (int j = 0; j < Utils.gridMonde.GetLength(1); j++)
            {
                Utils.gridMonde[i, j] = "0" + determineMateriel(i).ToString();
                Utils.gridExploration[i, j] = determinationDecouverte(i);
            }
        }

        if (SceneManager.GetActiveScene().name == SceneName.SCENE_PRINCIPALE)
            determinationSpecifique();
        else
            determinationSpecifiqueTutoriel();

    }
    private void setUpGridCell()
    {
        if (CellGrid != null)
        {
            for (int i = 0; i < CellGrid.GetLength(0); i++)
            {
                for (int j = 0; j < CellGrid.GetLength(1); j++)
                {
                    Destroy(CellGrid[i, j].gameObject);
                }
            }
        }

        CellGrid = new Cell[nbBlocs * nbGrilles, nbBlocs * nbGrilles];
        int x_Initial = (int)(Utils.PositionInt(player).x - (int)(nbBlocs * nbGrilles / 2));
        int y_Initial = (int)(Utils.PositionInt(player).y + (int)(nbBlocs * nbGrilles / 2));
        int x_Inter = x_Initial;
        int y_Inter = y_Initial;

        for (int i = 0; i < CellGrid.GetLength(0); i++)
        {
            for (int j = 0; j < CellGrid.GetLength(1); j++)
            {
                Cell cellPrefab_Inter;
                x_Inter = x_Initial + j;
                y_Inter = y_Initial - i;

                if (Utils.isInGridMonde(x_Inter, y_Inter))
                    cellPrefab_Inter = cellPrefab.allCell[Utils.getTypeFromXY(x_Inter, y_Inter)];
                else
                    cellPrefab_Inter = cellPrefab.allCell[0];

                CellGrid[i, j] = GameObject.Instantiate(cellPrefab_Inter);
                CellGrid[i, j].SetNouvInfo(x_Inter, y_Inter);
                CellGrid[i, j].gameObject.transform.parent = papa.transform;
            }
        }
        coin = new Vector2(x_Initial, y_Initial);
        coin.x = coin.x + ((int)(nbGrilles / 2)) * nbBlocs;
        coin.y = coin.y - ((int)(nbGrilles / 2)) * nbBlocs;
    }
    private void resize()
    {
        int nouv_I = 0;
        int nouv_J = 0;
        CellGrid_Inter = new Cell[nbBlocs * nbGrilles, nbBlocs * nbGrilles];

        for (int i = 0; i < CellGrid.GetLength(0); i++)
        {
            for (int j = 0; j < CellGrid.GetLength(1); j++)
            {
                nouv_I = i + (int)deplacement.y;
                nouv_J = j + (int)deplacement.x;

                if (nouv_I >= CellGrid.GetLength(0) || nouv_J >= CellGrid.GetLength(1) || nouv_I < 0 || nouv_J < 0)
                {
                    Vector2 positionBloc_inter = CellGrid[i, j].getPositionCoin();
                    positionBloc_inter.x += -(int)(nbGrilles * deplacement.x);
                    positionBloc_inter.y += (int)(nbGrilles * deplacement.y);

                    Destroy(CellGrid[i, j].gameObject);

                    if (Utils.isInGridMonde((int)positionBloc_inter.x, (int)positionBloc_inter.y))
                        CellGrid[i, j] = GameObject.Instantiate(cellPrefab.allCell[Utils.getTypeFromXY((int)positionBloc_inter.x, (int)positionBloc_inter.y)]);
                    else
                        CellGrid[i, j] = GameObject.Instantiate(cellPrefab.allCell[0]);

                    CellGrid[i, j].SetNouvInfo((int)positionBloc_inter.x, (int)positionBloc_inter.y);
                    CellGrid[i, j].gameObject.transform.parent = papa.transform;

                    nouv_I = (nouv_I % (nbBlocs * nbGrilles) + (nbBlocs * nbGrilles)) % (nbBlocs * nbGrilles);
                    nouv_J = (nouv_J % (nbBlocs * nbGrilles) + (nbBlocs * nbGrilles)) % (nbBlocs * nbGrilles);
                }
                CellGrid_Inter[nouv_I, nouv_J] = CellGrid[i, j];
            }
        }
        CellGrid = CellGrid_Inter;
        coin.x -= (int)deplacement.x;
        coin.y += (int)deplacement.y;

    }
    private bool isResize()
    {
        bool isResize = false;
        Vector2 p = player.position;
        deplacement.Set(0, 0);

        if (p.x > coin.x + nbBlocs)
        {
            deplacement.x = -nbBlocs;
            isResize = !isResize;
        }

        if (p.x < coin.x)
        {
            deplacement.x = nbBlocs;
            isResize = !isResize;
        }

        if (p.y > coin.y)
        {
            deplacement.y = nbBlocs;
            isResize = !isResize;
        }

        if (p.y < coin.y - nbBlocs)
        {
            deplacement.y = -nbBlocs;
            isResize = !isResize; ;
        }

        return isResize;
    }
    private int determineMateriel(int i)
    {
        int typeMat;

        int nbRendu = 0;
        int valeurRetour = listBlocByStage.Count;

        for (int k = 0; k < listBlocByStage.Count; k++)
        {
            if (i >= nbRendu && i < nbRendu + listBlocByStage[k])
            {
                valeurRetour = k;
            }
            nbRendu += listBlocByStage[k];
        }
        typeMat = RandomFunction.getNumber(valeurRetour);

        //argentTot += cellPrefab.allCell[typeMat].valeur;

        //if (cellPrefab.allCell[typeMat].typeCell == EnumCell.Black_Onyx)
        //    nbBlocsTot++;

        return typeMat;
    }
    private string determinationDecouverte(int i)
    {
        if (i == 0 || i == 1)
        {
            return "19";
        }
        else
        {
            return "0-1";
        }
    }
    private void determinationSpecifique()
    {
        //Mine shaft: (NOT FOR NOW) pourrait etre un concept, mais compliqué à implémenter
        //List<Vector2Int>[] allPositionsMineShaft = RandomFunction.newPositionMineShaft();
        //for (int i = 0; i < allPositionsMineShaft.Length; i++)
        //{
        //    for (int k = 0; k < allPositionsMineShaft[i].Count; k++)
        //    {
        //        Vector2Int SharftIJ = allPositionsMineShaft[i][k];
        //        Utils.setTypeGrid(SharftIJ.y, SharftIJ.x, (int)EnumCell.Treuil);
        //    }
        //}



        //Tresor:
        List<Vector2Int>[] allPositionsChest = RandomFunction.newPositionCoffre();
        int[] typeBlocContour = { (int)EnumCell.Stone, (int)EnumCell.Corundum, (int)EnumCell.Obsidian };
        for (int i = 0; i < allPositionsChest.Length; i++)
        {
            for (int k = 0; k < allPositionsChest[i].Count; k++)
            {
                Vector2Int TreasureIJ = allPositionsChest[i][k];

                //Bloc tresore
                Utils.setTypeGrid(TreasureIJ.y, TreasureIJ.x, (int)EnumCell.Treasure);
                Utils.setStateGrid(TreasureIJ.y, TreasureIJ.x, i);


                //Bloc Contour:
                Utils.setTypeGrid(TreasureIJ.y, TreasureIJ.x - 1, typeBlocContour[i]);
                Utils.setTypeGrid(TreasureIJ.y - 1, TreasureIJ.x, typeBlocContour[i]);
                Utils.setTypeGrid(TreasureIJ.y + 1, TreasureIJ.x, typeBlocContour[i]);
                Utils.setTypeGrid(TreasureIJ.y, TreasureIJ.x + 1, typeBlocContour[i]);

                //Stalactmite
                Utils.setStateGrid(TreasureIJ.y - 1, TreasureIJ.x, 1);
            }
        }

        //Ouverture gazon:
        for (int i = 1; i <= 3; i++)
        {
            Utils.setTypeFromXY(0, -i, (int)EnumCell.Rien);
            Utils.setExplorationFromXY(0, -i, 1);
        }

        //EndMap:
        int deepY = Utils.gridMonde.GetLength(0) - 2;
        int index = Random.Range(0, endMaps.Length - 1);
        for (int x = 0; x < endMaps[index].width; x++)
        {
            for (int y = 0; y < endMaps[index].height; y++)
            {
                Color pixelColor = endMaps[index].GetPixel(x, y);

                if (pixelColor == Color.blue)
                {
                    Utils.setTypeGrid(deepY - y, x, (int)EnumCell.Rien);
                    Utils.setExplorationGrid(deepY - y, x, 1);
                }
                else if (pixelColor == Color.black)
                    Utils.setTypeGrid(deepY - y, x, (int)EnumCell.Magma);
                else if (pixelColor == Color.white)
                    Utils.setTypeGrid(deepY - y, x, (int)EnumCell.Cursed);

            }
        }

        //Teleporteur room:
        const int profondeur = 70;
        int milieu = Utils.largeur / 2;

        for (int y = 0; y < Utils.largeur; y++)
        {
            int odd = UnityEngine.Random.Range(0, 9);

            if (odd == 0)
            {
                Utils.setTypeGrid(profondeur - 1, y, (int)EnumCell.Stone);
                Utils.setStateGrid(profondeur - 1, y, 1);
            }
            else if (odd == 1)
            {
                Utils.setTypeGrid(profondeur - 1, y, (int)EnumCell.Corundum);
                Utils.setStateGrid(profondeur - 1, y, 1);
            }

            Utils.setTypeGrid(profondeur + 1, y, (int)EnumCell.Stone);
            Utils.setTypeGrid(profondeur, y, (int)EnumCell.Rien);
            Utils.setExplorationGrid(profondeur, y, 1);
        }

        Utils.setTypeGrid(profondeur - 1, milieu - 1, (int)EnumCell.BedRock);
        Utils.setExplorationGrid(profondeur - 1, milieu - 1, 1);
        Utils.setTypeGrid(profondeur - 2, milieu, (int)EnumCell.BedRock);
        Utils.setExplorationGrid(profondeur - 2, milieu, 1);
        Utils.setTypeGrid(profondeur - 1, milieu + 1, (int)EnumCell.BedRock);
        Utils.setExplorationGrid(profondeur - 1, milieu + 1, 1);

        Utils.setTypeGrid(profondeur + 1, milieu - 1, (int)EnumCell.BedRock);
        Utils.setExplorationGrid(profondeur + 1, milieu - 1, 1);
        Utils.setTypeGrid(profondeur + 1, milieu, (int)EnumCell.BedRock);
        Utils.setExplorationGrid(profondeur + 1, milieu, 1);
        Utils.setTypeGrid(profondeur + 1, milieu + 1, (int)EnumCell.BedRock);
        Utils.setExplorationGrid(profondeur + 1, milieu + 1, 1);


        Utils.setTypeGrid(profondeur - 1, milieu, (int)EnumCell.Rien);
        Utils.setTypeGrid(profondeur, milieu, (int)EnumCell.Rien);

        //hole in the bedRock:
        int i_Hole = Utils.hauteur / 2 - 1;
        int j_Hole = Utils.largeur / 2;

        Utils.gridMonde[i_Hole, j_Hole] = "0" + (int)EnumCell.Rien;

        //Mystery:
        List<Vector2Int> allPositionMystery = RandomFunction.newPositionMysterious();
        for (int i = 0; i < allPositionMystery.Count; i++)
        {
            Vector2Int pos = allPositionMystery[i];

            Utils.setTypeGrid(pos.x, pos.y, (int)EnumCell.Mystery);
            Utils.setStateGrid(pos.x, pos.y, i);
        }
    }
    private void determinationSpecifiqueTutoriel()
    {
        //Ouverture gazon:
        for (int i = 1; i <= 3; i++)
        {
            Utils.setTypeFromXY(0, -i, (int)EnumCell.Rien);
        }

        //Tunel Dirt:
        Utils.setTypeFromXY(1, -2, (int)EnumCell.Rien);
        Utils.setTypeFromXY(2, -2, (int)EnumCell.Rien);
        Utils.setTypeFromXY(2, -3, (int)EnumCell.Rien);
        Utils.setTypeFromXY(2, -4, (int)EnumCell.Rien);

        //BlocDirt#Trigger:
        Utils.setTypeFromXY(0, 0, (int)EnumCell.TriggerTutoriel);
        Utils.setTypeFromXY(2, -3, (int)EnumCell.TriggerTutoriel);
        Utils.setTypeFromXY(2, -4, (int)EnumCell.TriggerTutoriel);

        //BlocIndestrictable:
        Utils.setTypeStateFromXY(1, -3, (int)EnumCell.BedRock, 1);
        Utils.setTypeStateFromXY(1, -4, (int)EnumCell.BedRock, 1);
        Utils.setTypeStateFromXY(3, -2, (int)EnumCell.BedRock, 1);
        Utils.setTypeStateFromXY(3, -3, (int)EnumCell.BedRock, 1);
        Utils.setTypeStateFromXY(3, -4, (int)EnumCell.BedRock, 1);
        Utils.setTypeStateFromXY(2, -5, (int)EnumCell.BedRock, 1);

    }

    //Public function:
    public Vector2Int getIJ(int x, int y)
    {
        int min_X, max_X;
        int min_Y, max_Y;

        min_X = (int)(coin.x - nbBlocs * (int)(nbGrilles / 2));
        max_X = (int)(min_X + nbBlocs * (int)(nbGrilles / 2 + 1));
        max_Y = (int)(coin.y + nbBlocs * (int)(nbGrilles / 2));
        min_Y = (int)(max_Y - nbBlocs * nbGrilles * (int)(nbGrilles / 2 + 1));

        int i = (int)(nbBlocs * (int)(nbGrilles / 2) - (y - coin.y));
        int j = (int)(x - coin.x) + nbBlocs * (int)(nbGrilles / 2);

        //x <= max_X && x >= min_X && y <= max_Y && y >= min_Y

        if (i >= 0 && j >= 0 && i < CellGrid.GetLength(0) && j < CellGrid.GetLength(1))
        {
            return new Vector2Int(i, j);
        }
        else
            return new Vector2Int(-1, -1);
    }
    public Cell getCellFromXY(int x, int y)
    {
        Vector2Int vecIJ = getIJ(x, y);

        if (vecIJ.x != -1 && vecIJ.y != -1)
            return CellGrid[vecIJ.x, vecIJ.y];
        else
            return null;
    }
    public void setGridCell(int x, int y)
    {
        Vector2Int vec = getIJ(x, y);
        Vector2 positionCoin;
        int type = Utils.getTypeFromXY(x, y);
        int i = vec.x;
        int j = vec.y;

        if (i != -1 && j != -1)
        {
            positionCoin = CellGrid[i, j].getPositionCoin();
            Destroy(CellGrid[i, j].gameObject);
            CellGrid[i, j] = GameObject.Instantiate(cellPrefab.allCell[type]);
            CellGrid[i, j].SetNouvInfo((int)positionCoin.x, (int)positionCoin.y);
            CellGrid[i, j].gameObject.transform.parent = papa.transform;
        }
    }

    //SAVE FUNCTIONS:
    public void RecenterGrid()
    {
        setUpGridCell();
    }

    public void loadEmptyResize()
    {
        Utils.gridMonde = null;
        setUpGridMonde();
    }
    public void LoadMonde()
    {
        MondeData data = SaveSystem.loadMonde();

        if (data != null)
        {
            Utils.gridMonde = data.gridMonde;
            Utils.gridExploration = data.gridExploration;

            coin = new Vector2(player.position.x, player.position.y);

            Utils.largeur = (int)bonderies.transform.localScale.x;
            Utils.hauteur = (int)bonderies.transform.localScale.y;
            Utils.position = bonderies.transform.position;

            setUpGridCell();
        }
        else
        {
            LoadNew();
        }
    }

    public void LoadNew()
    {
        coin = new Vector2(player.position.x, player.position.y);
        setUpGridMonde();
        setUpGridCell();
    }
    public void loadGodMode()
    {
        Utils.setExplorationAll(true);
    }
}
