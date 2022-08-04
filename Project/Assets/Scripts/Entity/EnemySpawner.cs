using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    Dictionary<WorldPlant, Enemy> enemyTargets;
    List<WorldPlant> availablePlants;
    List<WorldPlant> takenPlants;
    List<WorldPlant> plants;
    List<Enemy> noPlants;
    Character player;
    World world;

    void Start()
    {
        player = FindObjectOfType<Character>();

        enemyTargets = new Dictionary<WorldPlant, Enemy>();
        availablePlants = new List<WorldPlant>();
        takenPlants = new List<WorldPlant>();
        plants = new List<WorldPlant>();
        noPlants = new List<Enemy>();

        world = WorldController.I.World;

        for (int x = 0; x < world.Width; x++)
            for (int y = 0; y < world.Height; y++)
            {
                WorldCell cell = world.GetCell(x, y);

                cell.PlantCreated += NewPlant;
            }

        StartCoroutine(Difficulty());
        StartCoroutine(Spawn());
    }

    float difficulty = 1;

    IEnumerator Difficulty()
    {
        yield return new WaitForSeconds(20 * difficulty);
        difficulty += .5f;
        StopCoroutine(Spawn());
        yield return new WaitForSeconds(15 * difficulty);
        StartCoroutine(Spawn());
        StartCoroutine(Difficulty());
    }


    IEnumerator Spawn()
    {
        Vector3 pos = new Vector3(Random.Range(0, 50), Random.Range(0, 50));

        while ((pos.x < player.transform.position.x + 10 && pos.x > player.transform.position.x - 10) || (pos.y < player.transform.position.y + 5 && pos.y > player.transform.position.y - 5))
            pos = new Vector3(Random.Range(0, 50), Random.Range(0, 50));

        SpawnEnemy(pos);
        yield return new WaitForSeconds(Mathf.Clamp(5 / difficulty, .6f, 100));
        StartCoroutine(Spawn());
    }

    void NewPlant(WorldPlant plant)
    {
        plants.Add(plant);
        availablePlants.Add(plant);

        if (noPlants.Count > 0)
        {
            if (noPlants[0] == null) noPlants.RemoveAt(0);
            noPlants[0].AssignPlant(plant);
            enemyTargets.Add(plant, noPlants[0]);
            availablePlants.Remove(plant);
            noPlants.RemoveAt(0);
        }

        plant.Destroyed += PlantDestroyed;
    }

    void PlantDestroyed(WorldObject p)
    {
        WorldPlant plant = p as WorldPlant;

        plants.Remove(plant);

        if (availablePlants.Contains(plant))
            availablePlants.Remove(plant);

        if (takenPlants.Contains(plant))
            takenPlants.Remove(plant);

        if (enemyTargets.TryGetValue(plant, out Enemy e))
        {
            enemyTargets.Remove(plant);

            WorldPlant newPlant = AvailablePlant();

            if (newPlant == null)
                noPlants.Add(e);
            else
                e.AssignPlant(newPlant);
        }
    }

    WorldPlant AvailablePlant()
    {
        if (availablePlants.Count > 0)
        {
            int index = Random.Range(0, availablePlants.Count);
            WorldPlant plant = availablePlants[index];
            availablePlants.RemoveAt(index);
            takenPlants.Add(plant);
            return plant;
        }
        //else if (takenPlants.Count > 0)
        //{
        //    int index = Random.Range(0, takenPlants.Count);
        //    WorldPlant plant = takenPlants[index];
        //    takenPlants.RemoveAt(index);
        //    return plant;
        //}

        return null;
    }

    void EnemyDied(Entity ent)
    {
        Enemy enemy = (ent as Enemy);

        WorldPlant plant = enemy.Plant;

        if (plant == null) return;

        if (takenPlants.Contains(plant))
            takenPlants.Remove(plant);
        if (enemyTargets.ContainsValue(enemy))
            enemyTargets.Remove(plant);
        if (noPlants.Contains(enemy))
            noPlants.Remove(enemy);
    }

    void SpawnEnemy(Vector2 pos)
    {
        Enemy enemy = Instantiate(DataLibrary.I.Enemies["Slug"] as Enemy, pos, Quaternion.identity);

        WorldPlant plant = AvailablePlant();

        if (plant != null)
        {
            enemyTargets.Add(plant, enemy);
            enemy.AssignPlant(plant);
        }
        else
            noPlants.Add(enemy);

        enemy.Died += EnemyDied;

        enemy.Setup(player);
    }
}














//Dictionary<WorldPlant, List<Enemy>> enemyTargets;
//List<WorldPlant> availablePlants;
//List<WorldPlant> takenPlants;
//List<WorldPlant> plants;
//List<Enemy> noPlants;
//Character player;
//World world;

//void Start()
//{
//    player = FindObjectOfType<Character>();

//    enemyTargets = new Dictionary<WorldPlant, List<Enemy>>();
//    availablePlants = new List<WorldPlant>();
//    takenPlants = new List<WorldPlant>();
//    plants = new List<WorldPlant>();
//    noPlants = new List<Enemy>();

//    world = WorldController.I.World;

//    for (int x = 0; x < world.Width; x++)
//        for (int y = 0; y < world.Height; y++)
//        {
//            WorldCell cell = world.GetCell(x, y);

//            cell.PlantCreated += NewPlant;
//        }

//    StartCoroutine(Spawn());
//}

//IEnumerator Spawn()
//{
//    SpawnEnemy(new Vector2(Random.Range(10, 20), Random.Range(10, 20)));
//    yield return new WaitForSeconds(4);
//    StartCoroutine(Spawn());
//}

//void NewPlant(WorldPlant plant)
//{
//    plants.Add(plant);
//    availablePlants.Add(plant);

//    for (int i = noPlants.Count - 1; i >= 0; i--)
//    {
//        noPlants[i].AssignPlant(plant);
//        noPlants.RemoveAt(i);
//    }

//    plant.Destroyed += PlantDestroyed;
//}

//void PlantDestroyed(WorldObject p)
//{
//    WorldPlant plant = p as WorldPlant;

//    plants.Remove(plant);

//    if (availablePlants.Contains(plant))
//        availablePlants.Remove(plant);

//    if (takenPlants.Contains(plant))
//        takenPlants.Remove(plant);

//    if (enemyTargets.TryGetValue(plant, out List<Enemy> e))
//    {
//        enemyTargets.Remove(plant);

//        for (int i = 0; i < e.Count; i++)
//        {
//            WorldPlant newPlant = AvailablePlant();

//            if (newPlant == null)
//                noPlants.Add(e[i]);
//            else
//                e[i].AssignPlant(newPlant);
//        }
//    }
//}

//WorldPlant AvailablePlant()
//{
//    if (availablePlants.Count > 0)
//    {
//        int index = Random.Range(0, availablePlants.Count);
//        WorldPlant plant = availablePlants[index];
//        availablePlants.RemoveAt(index);
//        takenPlants.Add(plant);
//        return plant;
//    }
//    else if (takenPlants.Count > 0)
//    {
//        int index = Random.Range(0, takenPlants.Count);
//        WorldPlant plant = takenPlants[index];
//        takenPlants.RemoveAt(index);
//        return plant;
//    }

//    return null;
//}

//void EnemyDied(Entity ent)
//{
//    Enemy enemy = (ent as Enemy);

//    WorldPlant plant = enemy.Plant;

//    if (plant == null) return;

//    if (takenPlants.Contains(plant))
//    {
//        if (enemyTargets.TryGetValue(plant, out List<Enemy> e))
//        {
//            for (int i = 0; i < e.Count; i++)
//            {
//                if (e[i] == enemy)
//                {
//                    e.RemoveAt(i);
//                    break;
//                }
//            }

//            if (e.Count == 0)
//            {
//                takenPlants.Remove(plant);
//                enemyTargets.Remove(plant);
//            }
//        }
//    }
//}

//void SpawnEnemy(Vector2 pos)
//{
//    Enemy enemy = Instantiate(DataLibrary.I.Enemies["Slug"] as Enemy, pos, Quaternion.identity);

//    WorldPlant plant = AvailablePlant();

//    if (plant != null)
//    {
//        if (enemyTargets.TryGetValue(plant, out List<Enemy> e) == false)
//            enemyTargets.Add(plant, new List<Enemy>());
//        else
//            enemyTargets[plant].Add(enemy);

//        enemy.AssignPlant(plant);
//    }
//    else
//        noPlants.Add(enemy);

//    enemy.Died += EnemyDied;

//    enemy.Setup(player);
//}