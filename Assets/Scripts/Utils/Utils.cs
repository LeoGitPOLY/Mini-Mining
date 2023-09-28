using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    //Info sur le monde
    public static int hauteur, largeur;
    public static Vector3 position;

    //Info sur les cellules ("État (1), Type(11)")
    public static string[,] gridMonde;
    //Info sur les cellules ("Exploration (0:1), Range(11)")
    public static string[,] gridExploration;

    //GRID MONDE:
    public static void setStateGrid(int i, int j, int state)
    {
        string cellule = gridMonde[i, j];
        cellule = cellule.Substring(1);
        cellule = state.ToString() + cellule;

        if (isPositionSetable(i, j))
            gridMonde[i, j] = cellule;
    }
    public static int getStateGrid(int i, int j)
    {
        int state = -1;
        string strState;

        if (isPositionGetable(i, j))
        {
            strState = gridMonde[i, j];
            state = int.Parse(strState[0].ToString());
        }

        return state;
    }
    public static void setTypeGrid(int i, int j, int type)
    {
        string cellule;
        cellule = "0" + type.ToString();

        if (isPositionSetable(i, j))
        {
            gridMonde[i, j] = cellule;
        }
    }
    public static int getTypeGrid(int i, int j)
    {
        int type = -1;
        string strState;

        if (isPositionGetable(i, j))
        {
            strState = gridMonde[i, j];
            strState = strState.Substring(1);
            type = int.Parse(strState);
        }
        return type;
    }

    //GRID DECOUVERTE:
    public static void setExplorationGrid(int i, int j, int exploration)
    {
        if (isPositionGetable(i, j, true))
        {
            string cellule = gridExploration[i, j];
            cellule = cellule.Substring(1);
            cellule = exploration.ToString() + cellule;

            if (isPositionSetable(i, j, true))
                gridExploration[i, j] = cellule;
        }
    }

    public static void setExplorationAll(bool isExplored)
    {
        for (int i = 0; i < gridMonde.GetLength(0); i++)
        {
            for (int j = 0; j < gridMonde.GetLength(1); j++)
            {
                int explored = isExplored ? 1 : 0;
                setExplorationGrid(i, j, explored);
            }
        }
    }

    public static int getExplorationGrid(int i, int j)
    {
        int exploration = -1;
        string strExplortaion;

        if (isPositionGetable(i, j, true))
        {
            strExplortaion = gridExploration[i, j];
            exploration = int.Parse(strExplortaion[0].ToString());
        }

        return exploration;
    }
    public static void setRangeGrid(int i, int j, int range)
    {
        if (isPositionGetable(i, j, true))
        {
            string cellule = gridExploration[i, j];
            cellule = cellule[0] + range.ToString();

            if (isPositionSetable(i, j, true))
            {
                gridExploration[i, j] = cellule;
            }
        }
    }

    public static int getRangeGrid(int i, int j)
    {
        int range = -1;
        string strRange;

        if (isPositionGetable(i, j))
        {
            strRange = gridExploration[i, j];
            strRange = strRange.Substring(1);
            range = int.Parse(strRange);
        }
        return range;
    }


    // Transformation
    public static Vector3 GridToWorldPosition(int x, int y)
    {
        return new Vector3(position.x - (largeur / 2 - y - 0.5f), -(x + 0.5f));
    }
    public static Vector2Int PositionInt(Transform transform)
    {
        double x = transform.position.x;
        double y = transform.position.y;

        //Coordonnée du bloc le plus proche du centre
        if (x > 0)
            x = (int)x;
        else
            x = (int)(x - 1);

        if (y > 0)
            y = (int)(y + 1);
        else
            y = (int)y;

        return new Vector2Int((int)x, (int)y);
    }
    public static Vector2Int PositionInt(Vector2 vector)
    {
        double x = vector.x;
        double y = vector.y;

        //Coordonnée du bloc le plus proche du centre
        if (x > 0)
            x = (int)x;
        else
            x = (int)(x - 1);

        if (y > 0)
            y = (int)(y + 1);
        else
            y = (int)y;

        return new Vector2Int((int)x, (int)y);
    }

    //Verification
    public static bool isInGridMonde(int x, int y)
    {
        int i = -y;
        int j = (int)(x + Utils.largeur / 2 - Utils.position.x);
        bool inCell = false;

        if ((i >= 0 && i < Utils.gridMonde.GetLength(0)) && ((j >= 0 && j < Utils.gridMonde.GetLength(1))))
        {
            inCell = true;
        }
        else
        {
            //Debug.Log("NOT IN GRID MONDE AT xy (" + x + "," + y+ ")");
        }

        return inCell;
    }
    public static bool isPositionSetable(int i, int j, bool exploration = false)
    {
        //OVERIDE POUR PT ENLEVER LA BEDROCK
        bool setable = false;
        string[,] gridLocal = exploration ? gridExploration : gridMonde;

        if ((i >= 0 && i < gridLocal.GetLength(0)) && ((j >= 0 && j < gridLocal.GetLength(1))))
        {
            setable = true;
        }
        else
        {
            Debug.Log("NOT IN GRID MONDE AT ij (" + i + "," + j + ")");
        }

        return setable;
    }
    public static bool isPositionGetable(int i, int j, bool exploration = false)
    {
        //OVERIDE POUR PT ENLEVER LA BEDROCK
        bool getable = false;
        string[,] gridLocal = exploration ? gridExploration : gridMonde;

        if ((i >= 0 && i < gridLocal.GetLength(0)) && ((j >= 0 && j < gridLocal.GetLength(1))))
        {
            getable = true;
        }
        else
        {
            //Debug.Log("NOT IN GRID MONDE AT ij (" + i + "," + j + ")");
        }

        return getable;
    }


    //From XY, only a call back => GRID MONDE!
    public static int getTypeFromXY(int x, int y)
    {
        if (!isInGridMonde(x, y))
            return -1;

        int i = -y;
        int j = (int)(x + Utils.largeur / 2 - Utils.position.x);

        return getTypeGrid(i, j);
    }
    public static int getStateFromXY(int x, int y)
    {
        if (!isInGridMonde(x, y))
            return -1;

        int i = -y;
        int j = (int)(x + Utils.largeur / 2 - Utils.position.x);

        return getStateGrid(i, j);
    }
    public static void setStateFromXY(int x, int y, int state)
    {
        int i = -y;
        int j = (int)(x + Utils.largeur / 2 - Utils.position.x);

        setStateGrid(i, j, state);
    }
    public static void setTypeFromXY(int x, int y, int type)
    {
        int i = -y;
        int j = (int)(x + Utils.largeur / 2 - Utils.position.x);

        setTypeGrid(i, j, type);
    }
    public static void setTypeStateFromXY(int x, int y, int type, int state)
    {
        int i = -y;
        int j = (int)(x + Utils.largeur / 2 - Utils.position.x);

        setTypeGrid(i, j, type);
        setStateGrid(i, j, state);
    }

    //From XY, only a call back => GRID DECOUVERTE!
    public static void setExplorationFromXY(int x, int y, int exploration)
    {
        int i = -y;
        int j = (int)(x + Utils.largeur / 2 - Utils.position.x);

        setExplorationGrid(i, j, exploration);
    }
    public static int getExplorationFromXY(int x, int y)
    {
        if (!isInGridMonde(x, y))
            return -1;

        int i = -y;
        int j = (int)(x + Utils.largeur / 2 - Utils.position.x);

        return getExplorationGrid(i, j);
    }
    public static void setRangeGridFromXY(int x, int y, int range)
    {
        int i = -y;
        int j = (int)(x + Utils.largeur / 2 - Utils.position.x);

        setRangeGrid(i, j, range);
    }
    public static int getRangeFromXY(int x, int y)
    {
        if (!isInGridMonde(x, y))
            return -2;

        int i = -y;
        int j = (int)(x + Utils.largeur / 2 - Utils.position.x);

        return getRangeGrid(i, j);
    }


    //RESET:
    public static void resetWorld()
    {
        gridMonde = null;
        gridExploration = null;
    }
}
