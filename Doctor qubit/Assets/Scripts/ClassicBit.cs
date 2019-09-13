using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassicBit : MonoBehaviour, IBit {

    [SerializeField]
    public bool value;

    public int Value() => value ? 1 : 0;
}
