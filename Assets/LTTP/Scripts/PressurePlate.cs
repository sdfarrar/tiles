using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour {

	public PressurePlateGate Gate;
	public Animator Animator;

	void OnTriggerEnter2D(Collider2D other) {
		Gate.PlateTriggered(this);
		Animator.SetBool("active", true);
	}

	void OnTriggerExit2D(Collider2D other) {
		Animator.SetBool("active", false);
	}

}
