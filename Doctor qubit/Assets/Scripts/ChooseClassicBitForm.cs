using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ClassicBit))]
public class ChooseClassicBitForm : MonoBehaviour {

    public GameObject form0;
    public GameObject form1;
    private ClassicBit classicBit;

    private void Start() {
        classicBit = GetComponent<ClassicBit>();
    }

    public void Update() {
        form0.SetActive(!classicBit.value);
        form1.SetActive(classicBit.value);
    }

}
