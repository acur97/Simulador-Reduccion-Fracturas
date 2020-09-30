using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Offsets", menuName = "Offsets Object", order = 1)]
public class Offsets : ScriptableObject
{
    [Header("Upper Limb")]
    public float offSetUpLimbX;
    public float offSetUpLimbY;
    public float offSetUpLimbZ;
    [Space]
    public float offSetUpLimbRotX;
    public float offSetUpLimbRotY;
    public float offSetUpLimbRotZ;

    [Header("Lower Limb")]
    public float offSetLoLimbX;
    public float offSetLoLimbY;
    public float offSetLoLimbZ;
    [Space]
    public float offSetLoLimbRotX;
    public float offSetLoLimbRotY;
    public float offSetLoLimbRotZ;
}