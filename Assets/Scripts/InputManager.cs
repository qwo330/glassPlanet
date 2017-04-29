using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
	public Missile[] missilePrefs;
	public MissileIcon[] missileIconPrefs;
	public MissileButton[] missileButtons;

	private Bounds panelBounds;

	private Dictionary<int, MissileIcon> iconDict = new Dictionary<int, MissileIcon>();

	void Start() {
		BoxCollider2D box = GetComponent<BoxCollider2D>();
		RectTransform rt = GetComponent<RectTransform>();
		box.size = new Vector2(rt.rect.width, rt.rect.height);
		panelBounds = box.bounds;
	}

	void Update() {
		foreach (var touch in Input.touches) {
			switch (touch.phase) {
				case TouchPhase.Began:
					// Check missile button press
					Ray ray = Camera.main.ScreenPointToRay(touch.position);
					RaycastHit hit;

					if (Physics.Raycast(ray, out hit)) {
						MissileButton button = hit.transform.GetComponent<MissileButton>();
						if (button != null && button.Press()) {
							// New touch pressed button
							int type = button.type;
							MissileIcon iconInstance = Instantiate(
								missileIconPrefs[type],
								GetWorldPos(touch.position),
								Quaternion.identity);

							iconInstance.type = type;
							iconDict.Add(touch.fingerId, iconInstance);
						}
					}
					break;
				case TouchPhase.Moved:
					if (iconDict.ContainsKey(touch.fingerId)) {
						MissileIcon icon = iconDict[touch.fingerId];
						icon.transform.position = GetWorldPos(touch.position);

						if (!IsInPanel(icon.transform.position)) {
							// Fire
							missileButtons[icon.type].UnPress(false);
							iconDict.Remove(touch.fingerId);
							FireMissile(icon.type, icon.transform.position, touch.deltaPosition);
							Destroy(icon.gameObject);
						}
					}
					break;
				case TouchPhase.Ended:
					if (iconDict.ContainsKey(touch.fingerId)) {
						MissileIcon icon = iconDict[touch.fingerId];
						missileButtons[icon.type].UnPress(true);
						iconDict.Remove(touch.fingerId);
						Destroy(icon.gameObject);
					}
					break;
			}
		}
	}

	private void FireMissile(int type, Vector3 pos, Vector2 dir) {
		Missile missile = Instantiate(missilePrefs[type], pos, Quaternion.identity);
		missile.Init(dir, type);
	}

	private Vector3 GetWorldPos(Vector3 touchPos, float z = 0) {
		Vector3 pos = Camera.main.ScreenToWorldPoint(touchPos);
		pos.z = z;
		return pos;
	}

	private bool IsInPanel(Vector3 pos) {
		pos.z = panelBounds.center.z;
		return panelBounds.Contains(pos);
	}
}