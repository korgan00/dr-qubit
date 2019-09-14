using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuBit : MonoBehaviour, IBit {
    public Vector3 state;
    private List<string> appliedQuGates;

    private void Start() {
        state = Vector3.forward;
        appliedQuGates = new List<string>();
    }

    public void ApplyGate(QuGate quGate) {
        state = quGate.modification * state;
        appliedQuGates.Add(quGate.code);
        Debug.Log(state);
    }

    public int Value() {
        float probability0 = (state.z + 1) / 2;
        float probability1 = (1 - state.z) / 2;

        return probability0 > probability1 ? 0 : 1;
    }
}
