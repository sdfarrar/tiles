using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {

    public float MoveSpeed = 4f;
    public Animator Animator;
    public Vector2 LookFacing;
    public Vector3 Movemovent;

    private Rigidbody2D rb;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {

        Vector3 tryMove = Vector3.zero;

        if (Input.GetKey(KeyCode.LeftArrow))
            tryMove += Vector3Int.left;
        if (Input.GetKey(KeyCode.RightArrow))
            tryMove += Vector3Int.right;
        if (Input.GetKey(KeyCode.UpArrow))
            tryMove += Vector3Int.up;
        if (Input.GetKey(KeyCode.DownArrow))
            tryMove += Vector3Int.down;

        rb.velocity = Vector3.ClampMagnitude(tryMove, 1f) * MoveSpeed;
        if (tryMove.magnitude > 0f) {
            LookFacing = tryMove;
        }

    }
}
