using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Modifier
{
    [SerializeField] private float BaseValue;

    private List<float> modifiers = new List<float>();

    public float GetValue()
    {
        float finalValue = BaseValue;
        modifiers.ForEach(x => finalValue += x);
        return finalValue;
    }

    public void AddModifier(float modifier)
    {
        if (modifier != 0)
        {
            modifiers.Add(modifier);
        }
    }

    public void RemoveModifier(float modifier)
    {
        if (modifier != 0)
        {
            modifiers.Remove(modifier);
        }
    }

}
