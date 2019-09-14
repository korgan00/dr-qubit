using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(QuBit))]
public class QuBitCollapsedForm : MonoBehaviour {

    public GameObject qubitSuperposition;
    public GameObject qubit0;
    public GameObject qubit1;

    private QuBit quBit;

    void Start() {
        quBit = GetComponent<QuBit>();
    }

    private void Update() {
        qubitSuperposition.SetActive(!quBit.collapsed);
        qubit0.SetActive(quBit.collapsed && quBit.Value() == 0);
        qubit1.SetActive(quBit.collapsed && quBit.Value() == 1);
    }

}
