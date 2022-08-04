using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class DataLibrary : MonoBehaviour
{
    // Don't forget to put these assets in the Resources folder!

    #region Statics
    static DataLibrary instance;
    public static DataLibrary I
    {
        get
        {
            return instance;
        }
        set
        {
            if (instance) { return; }
            else
            {
                instance = value;
            }
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Already A DataLibrary in scene. Deleting this one!");
            Destroy(gameObject);
            return;
        }

        Setup();
    }

    #endregion

    public Getter Structures { get; private set; }
    public Getter Grounds { get; private set; }
    public Getter Prefabs { get; private set; }
    public Getter Plants { get; private set; }
    public Getter Enemies { get; private set; }
    public Getter Items { get; private set; }

    void Setup()
    {
        Structures = new Getter(Resources.LoadAll<Structure>("ScriptableObjects/Structures"));
        Grounds = new Getter(Resources.LoadAll<Ground>("ScriptableObjects/Grounds"));
        Plants = new Getter(Resources.LoadAll<Plant>("ScriptableObjects/Plants"));
        Items = new Getter(Resources.LoadAll<Item>("ScriptableObjects/Items"));
        Prefabs = new Getter(Resources.LoadAll<GameObject>("Prefabs"));
        Enemies = new Getter(Resources.LoadAll<Enemy>("Prefabs"));
    }
}
