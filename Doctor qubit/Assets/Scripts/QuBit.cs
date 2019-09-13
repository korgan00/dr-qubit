using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuBit : MonoBehaviour, IBit {
    private Vector3 state;
    private List<string> appliedQuGates;

    public void ApplyGate(QuGate quGate) {
        state = quGate.modification * state;
        appliedQuGates.Add(quGate.code);
    }

    public int Value() {
        float probability0 = (state.z + 1) / 2;
        float probability1 = (1 - state.z) / 2;

        return probability0 > probability1 ? 0 : 1;
    }
}
