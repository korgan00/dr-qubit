using UnityEngine;
using System.Collections;

public class QuGateTests : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        Vector3 vector0 = new Vector3(0, 0, 1);
        Vector3 vector1 = new Vector3(0, 0, -1);
        Vector3 vectorSuperpositionPlus = new Vector3(1, 0, 0);
        Vector3 vectorSuperpositionMinus = new Vector3(-1, 0, 0);

        Vector3 rotationVectorForX = new Vector3(180, 0, 0);
        Quaternion rotationForX = Quaternion.Euler(rotationVectorForX);
        Debug.Log($"X * |0>: {rotationForX * vector0} == 0 0 -1");
        Debug.Log($"X * |1>: {rotationForX * vector1} == 0 0 1");
        Debug.Log($"X * |0> + |1>: {rotationForX * vectorSuperpositionPlus} == 1 0 0");
        Debug.Log($"X * |0> - |1>: {rotationForX * vectorSuperpositionMinus} == -1 0 0");

        Vector3 rotationVectorForZ = new Vector3(0, 0, 180);
        Quaternion rotationForZ = Quaternion.Euler(rotationVectorForZ);
        Debug.Log($"Z * |0>: {rotationForZ * vector0} == 0 0 1");
        Debug.Log($"Z * |1>: {rotationForZ * vector1} == 0 0 -1");
        Debug.Log($"Z * |0> + |1>: {rotationForZ * vectorSuperpositionPlus} == -1 0 0");
        Debug.Log($"Z * |0> - |1>: {rotationForZ * vectorSuperpositionMinus} == 1 0 0");

        Vector3 rotationVectorForH = new Vector3(0, 90, 180);
        Quaternion rotationForH = Quaternion.Euler(rotationVectorForH);
        Debug.Log($"H * |0>: {rotationForH * vector0} == 1 0 0");
        Debug.Log($"H * |1>: {rotationForH * vector1} == -1 0 0");
        Debug.Log($"H * |0> + |1>: {rotationForH * vectorSuperpositionPlus} == 0 0 1");
        Debug.Log($"H * |0> - |1>: {rotationForH * vectorSuperpositionMinus} == 0 0 -1");


        Debug.Log($"H * X * |0>: {rotationForH * rotationForX * vector0} == -1 0 0");
        Debug.Log($"X * H * |0>: {rotationForX * rotationForH * vector0} == 1 0 0");

    }
}
