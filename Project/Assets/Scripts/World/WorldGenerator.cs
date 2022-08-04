using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator
{
    World world;
    int height;
    int width;

    public WorldGenerator(World world)
    {
        this.world = world;
        this.width = world.Width;
        this.height = world.Height;
    }

    public void GenerateWorld()
    {
        Ground dirt = DataLibrary.I.Grounds["Soil"] as Ground;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                world.SetCell(x, y, new WorldCell(dirt, x, y));
            }
        }
    }
}
