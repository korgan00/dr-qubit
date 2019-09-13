using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuBitMovement : MonoBehaviour {

    public Vector2 position;

    public void MoveVertical(float amount) {
        position.y -= amount;
        transform.position = transform.position - (Vector3.up * amount);
    }

    public void MoveHorizontal(float amount) {
        position.x += amount;
        transform.position = transform.position + (Vector3.right * amount);
    }

    public void StopMoving() {
        Destroy(this);
    }
}
