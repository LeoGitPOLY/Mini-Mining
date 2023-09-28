using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "NewSkin")]
public class SkinToBuy : ScriptableObject
{
    [SerializeField] private Sprite spriteToShow;
    [SerializeField] private string nameSkin;
    [SerializeField] private int coinsToBuy;
    [SerializeField] private string description;
    [SerializeField] private double realMoneyToBuy;
    [SerializeField] private RuntimeAnimatorController animatorControllers;
    [SerializeField] private EnumSkinName skinType;


    public Sprite SpriteToShow { get => spriteToShow; }
    public string NameSkin { get => nameSkin;}
    public int CoinsToBuy { get => coinsToBuy;}
    public double RealMoneyToBuy { get => realMoneyToBuy;}
    public RuntimeAnimatorController AnimatorControllers { get => animatorControllers;}
    public EnumSkinName SkinType { get => skinType;}
    public string Description { get => description; }
}
