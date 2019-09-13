using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Qubit : MonoBehaviour, IBit
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
        return 0;
    }
}
