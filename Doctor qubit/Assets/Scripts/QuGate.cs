using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class QuGate
{
    public string code;
    public Quaternion modification => Quaternion.Euler(angle.x, angle.y, angle.z);
    [SerializeField] private Vector3 angle;
}
