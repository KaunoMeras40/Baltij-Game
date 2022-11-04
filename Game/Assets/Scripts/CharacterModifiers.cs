using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterModifiers : MonoBehaviour
{
    #region Singleton

    public static CharacterModifiers instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion

    public Modifier SpeedModifier;
    public Modifier HealModifier;
    public Modifier LifestealModifier;
    public Modifier CriticalStrikeModifier;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
