using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-1)]
public class WorldController : MonoBehaviour
{
    #region Statics
    static WorldController instance;
    public static WorldController I
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

    #endregion

    [SerializeField] int width;
    [SerializeField] int height;

    public World World { get; private set; }

    private void Awake()
    {
        #region Statics
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning($"Already a {name} in scene. Deleting this one!");
            Destroy(gameObject);
            return;
        }
        #endregion

        World = new World(width, height);
        //Camera.main.GetComponent<CameraFollowWithBarriers>().followObject = FindObjectOfType<Player>().transform;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
    }
}
