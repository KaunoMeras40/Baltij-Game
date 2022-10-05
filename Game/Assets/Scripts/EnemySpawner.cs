using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject EnemyV1Prefab;
    [SerializeField] private GameObject EnemyV2Prefab;
    [SerializeField] private GameObject EnemyV3Prefab;
    [SerializeField] private GameObject EnemyV4Prefab;

    [SerializeField] TextMeshProUGUI waveNumber;
    [SerializeField] TextMeshProUGUI enemyAmount;

    [SerializeField] private string[] weapons;

    int healthrange = 25;
    int armorrange = 10;
    int damagerange = 10;

    public AnimationCurve HealthCurve;
    public AnimationCurve DamageCurve;
    public AnimationCurve ArmorCurve;
    public AnimationCurve SpawnRateCurve;
    public AnimationCurve SpawnCountCurve;

    public int currentWave = 1;
    int spawnedEnemies = 0;
    int enemiesAlive = 0;
    private int InitialEnemiesToSpawn;
    private float InitialSpawnDelay;

    private NavMeshTriangulation Triangulation;

    public int NumberOfEnemiesToSpawn = 5;
    public float SpawnDelay = 1f;

    public string[] enemies;

    int shootingEnemiesAmount = 0;

    private void Awake()
    {
        InitialEnemiesToSpawn = NumberOfEnemiesToSpawn;
        InitialSpawnDelay = SpawnDelay;
    }
    void Start()
    {
        Triangulation = NavMesh.CalculateTriangulation();

        StartCoroutine(SpawnEnemies());
    }
    private IEnumerator SpawnEnemies()
    {
        currentWave++;
        spawnedEnemies = 0;
        enemiesAlive = 0;
        ScaleUpSpawns();
        yield return new WaitForSeconds(5f);

        WaitForSeconds Wait = new WaitForSeconds(SpawnDelay);

        while (spawnedEnemies < NumberOfEnemiesToSpawn)
        {
            SpawnEnemy();
            yield return Wait;
        }
    }
    private void Update()
    {
        GameObject[] alive = GameObject.FindGameObjectsWithTag("Enemy");

        if (alive.Length == 0 && spawnedEnemies == NumberOfEnemiesToSpawn)
        {
            StartCoroutine(SpawnEnemies());
        }
        waveNumber.text = "Wave " + currentWave.ToString();
        enemyAmount.text = "Enemies Left: " + alive.Length;
    }
    void ScaleEnemy(int wave)
    {
        healthrange = 25;
        armorrange = 10;
        damagerange = 10;

        healthrange = Mathf.FloorToInt(healthrange * HealthCurve.Evaluate(wave));
        armorrange = Mathf.FloorToInt(armorrange * ArmorCurve.Evaluate(wave));
        damagerange = Mathf.FloorToInt(damagerange * DamageCurve.Evaluate(wave));
    }

    void CreateEnemyV1(Vector3 pos)
    {
        GameObject enemy = Instantiate(EnemyV1Prefab, pos, Quaternion.identity);
        ScaleEnemy(currentWave);
        int health = Random.Range(0, healthrange);
        int armor = Random.Range(0, armorrange);
        int damage = Random.Range(5, damagerange);

        float attackRange = 1f;

        int wpnIndex = Random.Range(0, weapons.Length);
        string weapon = weapons[wpnIndex];

        if (weapon == "Vape")
        {
            attackRange = 1.5f;
        }
        else if (weapon == "Brick")
        {
            attackRange = 1.3f;
        }
        else if (weapon == "Baseball")
        {
            attackRange = 1.7f;
        }

        CharacterStats enemyStats = enemy.GetComponent<CharacterStats>();
        enemyStats.NPCHealthModifier(health);
        enemyStats.AddArmor(armor);
        enemyStats.AddDamage(damage);

        enemy.GetComponent<EnemyV1_Attack>().attackRange = attackRange;
        enemy.GetComponent<EnemyMovement>().attackRange = attackRange;
        enemy.GetComponent<Appearance>().RandomAppearance();
        enemy.GetComponent<Appearance>().EquipWeapon(weapon);
    }

    void CreateEnemyV3(Vector3 pos)
    {
        GameObject enemy = Instantiate(EnemyV3Prefab, pos, Quaternion.identity);
        ScaleEnemy(currentWave);
        int health = Random.Range(0, healthrange);
        int armor = Random.Range(0, armorrange);
        int damage = Random.Range(5, damagerange);

        float attackRange = 1.2f;

        CharacterStats enemyStats = enemy.GetComponent<CharacterStats>();
        enemyStats.NPCHealthModifier(health);
        enemyStats.AddArmor(armor);
        enemyStats.AddDamage(damage);

        enemy.GetComponent<EnemyV3_Attack>().attackRange = attackRange;
        enemy.GetComponent<EnemyMovement>().attackRange = attackRange;
        enemy.GetComponent<Appearance>().RandomAppearance();
        //enemy.GetComponent<Appearance>().EquipWeapon(weapon);
    }

    void CreateEnemyV4(Vector3 pos)
    {
        GameObject enemy = Instantiate(EnemyV4Prefab, pos, Quaternion.identity);
        ScaleEnemy(currentWave);
        int health = Random.Range(0, healthrange);
        int armor = Random.Range(0, armorrange);
        int damage = Random.Range(5, damagerange);

        CharacterStats enemyStats = enemy.GetComponent<CharacterStats>();
        enemyStats.NPCHealthModifier(health);
        enemyStats.AddArmor(armor);
        enemyStats.AddDamage(damage);

        enemy.GetComponent<Appearance>().RandomAppearance();
        //enemy.GetComponent<Appearance>().EquipWeapon(weapon);

        shootingEnemiesAmount++;
    }

    void CreateEnemyV2(Vector3 pos)
    {
        GameObject enemy = Instantiate(EnemyV2Prefab, pos, Quaternion.identity);
        ScaleEnemy(currentWave);
        int health = Random.Range(0, Mathf.FloorToInt(healthrange * 0.5f));
        int armor = Random.Range(0, Mathf.FloorToInt(armorrange * 0.5f));
        int damage = Random.Range(5, Mathf.FloorToInt(damagerange * 1.5f));

        CharacterStats enemyStats = enemy.GetComponent<CharacterStats>();
        enemyStats.NPCHealthModifier(health);
        enemyStats.AddArmor(armor);
        enemyStats.AddDamage(damage);

        enemy.GetComponent<Appearance>().RandomAppearance();
    }

    void SpawnEnemy()
    {
        int enemyindex = Random.Range(0, enemies.Length);
        Vector3 pos = ChooseRandomPositionOnNavMesh();
        if (enemies[enemyindex] == "V1")
        {
            CreateEnemyV1(pos);
        }
        else if (enemies[enemyindex] == "V2")
        {
            CreateEnemyV2(pos);
        }
        else if (enemies[enemyindex] == "V3")
        {
            CreateEnemyV3(pos);
        }
        else if (enemies[enemyindex] == "V4")
        {
            if (shootingEnemiesAmount >= 2)
            {
                return;
            }
            else
            {
                CreateEnemyV4(pos);
            }
        }
        spawnedEnemies++;
        enemiesAlive++;
    }

    private Vector3 ChooseRandomPositionOnNavMesh()
    {
        GameObject[] spawns = GameObject.FindGameObjectsWithTag("EnemySpawn");
        int index = Random.Range(0, spawns.Length);
        return spawns[index].transform.position;

        //int VertexIndex = Random.Range(0, Triangulation.vertices.Length);
       // return Triangulation.vertices[VertexIndex];
    }

    private void ScaleUpSpawns()
    {
        NumberOfEnemiesToSpawn = InitialEnemiesToSpawn;
        SpawnDelay = InitialSpawnDelay;
        NumberOfEnemiesToSpawn = Mathf.FloorToInt(InitialEnemiesToSpawn + SpawnCountCurve.Evaluate(currentWave + 1));
        Debug.Log(NumberOfEnemiesToSpawn);
        SpawnDelay = InitialSpawnDelay * SpawnRateCurve.Evaluate(currentWave + 1);
    }

}
