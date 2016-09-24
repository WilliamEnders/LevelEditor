using UnityEngine;
using System.Collections;

public class reSizeObject : MonoBehaviour {
	public Transform cam;
	private Vector3 mousePos;

	public Transform obj;
	private Vector3 objSize,objPos,newSize,deltaSize;

	public Transform[] handle;
	private Vector3 handlePos, screenHandlePos, newHandlePos;

	public int axis;
	public float sizeDamp;
	public int flip;

	public bool drag;

	// Use this for initialization
	void Start () {
		drag = false;
		objSize = obj.localScale;

	}
	
	// Update is called once per frame

	void FixedUpdate(){
		UpdateLinePos ();
		UpdateGizmoPos ();
	}

	void Update () {



		mousePos = Input.mousePosition;
		
		if(Input.GetMouseButtonDown(0)){
			RaycastHit hit;
			if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit)) {
				for (int i = 0; i < handle.Length; i++) {
					if (hit.transform == handle [i]) {
						
						axis = i;
						drag = true;

						handlePos = handle [axis].localPosition;
						screenHandlePos = Camera.main.WorldToScreenPoint(handle [axis].position);
						objPos = Camera.main.WorldToScreenPoint(obj.position);

						if (axis == 0 || axis == 1) {
							flip = screenHandlePos [axis] < objPos [axis] ? -1 : 1;
						} else {
							flip = screenHandlePos [0] < objPos [0] ? -1 : 1;
						}

						objSize = obj.localScale;
					}
				}
			}
		}
		if (drag) {
			if (Input.GetMouseButton (0)) {
				
					UpdateSize ();
					newHandlePos = handle [axis].localPosition;
					if (axis == 0 || axis == 1) {
						newHandlePos [axis] = 1 - deltaSize [axis];
					} else {
						newHandlePos [axis] = 1 - deltaSize [0];
					}
					UpdateHandlePos (newHandlePos);

			}

			if (Input.GetMouseButtonUp (0)) {
					UpdateHandlePos (handlePos);
				drag = false;
			}
		}
	}

	void UpdateHandlePos(Vector3 updatePos){
		handle [axis].localPosition = updatePos;
	}

	void UpdateLinePos(){
		for (int i = 0; i < handle.Length; i++) {
			handle [i].GetComponent<LineRenderer> ().SetPosition (0,transform.root.transform.position);
			handle [i].GetComponent<LineRenderer> ().SetPosition (1,handle[i].position);
		}
	}

	void UpdateGizmoPos(){
		Vector3 half = Vector3.Lerp (cam.position, obj.position, 0.5f);
		transform.position = cam.position - Vector3.Normalize(half) * 4;
	}

	void UpdateSize(){
		newSize = obj.localScale;
		deltaSize = (screenHandlePos - mousePos) / sizeDamp * flip;
		Vector3 newDeltaSize = deltaSize * Vector3.Distance (cam.position,obj.position);
		if (axis == 0 || axis == 1) {
			newSize [axis] = objSize [axis] - newDeltaSize [axis];
		} else {
			newSize [axis] = objSize [axis] - newDeltaSize [0];
		}
		obj.localScale = newSize;
	}
}
