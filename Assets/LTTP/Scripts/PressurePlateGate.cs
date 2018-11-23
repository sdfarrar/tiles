using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateGate : MonoBehaviour {

	public PressurePlate PressurePlate;

	public Collider2D Blocker;
	public Animator Animator;
	public bool Open = false;

	public void PlateTriggered(PressurePlate plate) {
		Open = !Open;
		Blocker.enabled = Open;
		Animator.SetBool("open", Open);
	}

}
