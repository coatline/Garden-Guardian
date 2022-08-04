using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Ground", fileName = "New Ground")]

public class Ground : ScriptableObject
{
    [TextArea()]
    [SerializeField] string description;

    [SerializeField] Tile tile;
    [SerializeField] bool tilled;
    [Range(0, 1f)]
    [SerializeField] float fertility;

    public Tile Tile { get { return tile; } }
    public bool Tilled { get { return tilled; } }
    public float Fertility { get { return fertility; } }
    public string Description { get { return description; } }
}
