using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuBit : MonoBehaviour, IBit {
    public Vector3 state;
    private List<string> appliedQuGates;
    public bool collapsed => _value != -1;
    private int _value;

    [Header("Sounds")]
    public AudioSource collapse0;
    public AudioSource collapse1;

    private void Start() {
        state = Vector3.forward;
        appliedQuGates = new List<string>();
        _value = -1;
    }

    public void ApplyGate(QuGate quGate) {
        state = quGate.modification * state;
        appliedQuGates.Add(quGate.code);
        Debug.Log(state);
    }

    public int Value() {
        if (_value == -1) {
            float probability0 = (state.z + 1) / 2;
            //float probability1 = (1 - state.z) / 2;

            float rnd = Random.value;

            Debug.Log($"Collapsed: rnd({rnd}) prob({probability0})");

            _value = probability0 > rnd ? 0 : 1;
            if (_value == 0) {
                collapse0.Play();
            } else {
                collapse1.Play();
            }
        }

        return _value;
    }
}
