using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Mission")]
public class Mission : ScriptableObject
{
    public int nombreDemande;
    public EnumCell typeDemande;

    public int prix;
}
