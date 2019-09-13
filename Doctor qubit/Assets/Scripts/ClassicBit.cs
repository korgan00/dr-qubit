using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassicBit : MonoBehaviour {

    [SerializeField]
    private bool _value;

    public int Measure() => _value ? 1 : 0;
}
