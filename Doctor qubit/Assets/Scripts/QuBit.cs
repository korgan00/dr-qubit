using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuBit : MonoBehaviour, IBit
{
    private Vector3 state;
    private List<string> appliedQuGates;

    public void ApplyGate(QuGate quGate)
    {
        state = quGate.modification * state;
        appliedQuGates.Add(quGate.code);
    }

    public int Value()
    {
        private probability0 = (state.Z + 1) / 2;
        private probability1 = (1 - state.Z) / 2;

        return probability0 > probability1 ? 0 : 1;
    }
}
