using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


//タイルの種類分け

[CreateAssetMenu(menuName ="TilesInfo")]
public class TilesInfomation : ScriptableObject
{
    public TileBase tile;
    public new string name;
    public string description;
    public Type type;
    public enum Type{
        wall,
        obstacle,
        derivation,
        item,
    }
}
