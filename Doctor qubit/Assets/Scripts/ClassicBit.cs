using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassicBit : MonoBehaviour, IBit {

    [SerializeField]
    private bool _value;

    public int Value() => _value ? 1 : 0;
}
