using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopManager : MonoBehaviour
{

    #region Singleton

    public static LoopManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    [SerializeField] private Transform VendingMachine1;
    [SerializeField] private Transform VendingMachine2;

    [SerializeField] List<Transform> VendingMachine_Spawns;
    [SerializeField] List<Transform> WeaponBoards;

    void Start()
    {
        Invoke(nameof(SpawnWeaponBoards), 2f);
        Invoke(nameof(SpawnVendingMachines), 2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnWeaponBoards()
    {
        List<Transform> allBoards = new List<Transform>(WeaponBoards);
        List<Item> allWeapons = new List<Item>(PlayerManager.Instance.VM_Weapons);
        ShuffleList(allWeapons);
        foreach (Transform board in allBoards)
        {
            foreach (Item item in allWeapons)
            {
                board.GetComponent<Interactable>().item = item;
                Transform wpn = Instantiate(item.itemPrefab, board.position, Quaternion.identity, board).transform;
                Destroy(wpn.GetComponent<Rigidbody>());
                Destroy(wpn.GetComponent<Interactable>());
                Destroy(wpn.GetComponent<BoxCollider>());
                wpn.localPosition = Vector3.zero;
                allWeapons.Remove(item);
                break;
            }
        }
    }
    void ShuffleList(List<Item> alpha)
    {
        for (int i = 0; i < alpha.Count; i++)
        {
            Item temp = alpha[i];
            int randomIndex = Random.Range(i, alpha.Count);
            alpha[i] = alpha[randomIndex];
            alpha[randomIndex] = temp;
        }
    }

    void SpawnVendingMachines()
    {
        List<Transform> allSpawns = new List<Transform>(VendingMachine_Spawns);

        int int1 = Random.Range(0, allSpawns.Count);
        Transform spawn = allSpawns[int1];
        allSpawns.Remove(spawn);
        int int2 = Random.Range(0, allSpawns.Count);
        Transform spawn2 = allSpawns[int2];
        allSpawns.Remove(spawn2);

        VendingMachine1.position = spawn.position;
        VendingMachine2.position = spawn2.position;

        VendingMachine1.GetComponent<VendingMachine>().isSetup = false;
        VendingMachine2.GetComponent<VendingMachine>().isSetup = false;
    }
}
