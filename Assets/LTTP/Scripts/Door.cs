using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    public Door Exit;

    public void OnTriggerEnter2D(Collider2D other) {
        print("triggerd");
    }

}
