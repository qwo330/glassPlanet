using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour {
	[HideInInspector]
	public int type;
	public float moveSpeed;
	private Vector2 dir;

	public void Init(Vector2 dir, int type) {
		this.dir = dir.normalized;
		this.type = type;
		// Rotate according to dir
	}

	private void Update() {
		transform.Translate(dir * moveSpeed);
	}
}
