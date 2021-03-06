﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(QuBit))]
public class QuBitVectorRotator : MonoBehaviour {

    private QuBit quBit;
    [SerializeField]
    private GameObject axis;
    private Quaternion startRotation;

    void Start() {
        quBit = GetComponent<QuBit>();
        startRotation = axis.transform.rotation;
    }

    void Update() {
        if (System.Math.Abs(quBit.state.x - -1f) < 0.0001) {
            axis.transform.rotation = startRotation * Quaternion.Euler(-90, 0, 0);
        } else {
            axis.transform.rotation = startRotation * Quaternion.LookRotation(quBit.state, Vector3.forward);
        }
    }
}
