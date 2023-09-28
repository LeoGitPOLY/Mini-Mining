using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IblocDigable 
{
    bool isDiging(Cell cellDig, float frameRate);
    void stopDiging();
}
