using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissileButton : MonoBehaviour {
	public int type;
	public float cooltime;
	private float nextActiveTime = 0;

	private bool pressed = false;
	private bool ready = true;

	private Image shade;

	private void Start() {
		BoxCollider box = GetComponent<BoxCollider>();
		RectTransform rt = GetComponent<RectTransform>();
		box.size = new Vector3(rt.rect.width, rt.rect.height, 1f);

		shade = transform.FindChild("IconShade").GetComponent<Image>();
	}

	public bool Press() {
		if (pressed || !ready) return false;
		pressed = true;
		return true;
	}

	public void UnPress(bool canceled) {
		pressed = false;
		if (!canceled) StartCoroutine("CoolingCo");
	}

	private IEnumerator CoolingCo() {
		ready = false;
		shade.fillAmount = 1f;
		nextActiveTime = Time.time + cooltime;
		while (Time.time <= nextActiveTime) {
			shade.fillAmount = (nextActiveTime - Time.time) / cooltime;
			yield return null;
		}
		ready = true;
		shade.fillAmount = 0;
	}
}
