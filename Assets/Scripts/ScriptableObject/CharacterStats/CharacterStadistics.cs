using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterStadistic", menuName = "ScriptableObjects/Stadistic", order = 2)]
public class CharacterStadistics : ScriptableObject
{
    public string characterName;
    public CharacterType characterType;
    public Sprite icon;
    public Color teamColor;

    public float initialLife;
    public float powerAttack;
    public float healing;

    public int maxDamageIndex;
    public int maxAttackIndex;
}