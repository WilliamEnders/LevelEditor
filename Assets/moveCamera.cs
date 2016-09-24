using UnityEngine;
using System.Collections;

public class moveCamera : MonoBehaviour {

	private Vector3 move;
	public float speed;

	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	public RotationAxes axes = RotationAxes.MouseXAndY;
	public float sensitivityX = 15F;
	public float sensitivityY = 15F;
	public float minimumX = -360F;
	public float maximumX = 360F;
	public float minimumY = -60F;
	public float maximumY = 60F;
	float rotationX = 0F;
	float rotationY = 0F;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		move = new Vector3 (Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"),Input.GetAxis("Forward"));
		move *= speed;

		transform.position += ((transform.right * move.x) + (transform.up * move.y) + (transform.forward * move.z)) * Time.deltaTime;

		RightClick ();

	}

	void RightClick ()
	{
		if (Input.GetMouseButton(1)) {
			if (axes == RotationAxes.MouseXAndY) {
				rotationX = transform.localEulerAngles.y + Input.GetAxis ("Mouse X") * sensitivityX;
				rotationY += Input.GetAxis ("Mouse Y") * sensitivityY;
				rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
				transform.localEulerAngles = new Vector3 (-rotationY, rotationX, 0);
			} else if (axes == RotationAxes.MouseX) {
				transform.Rotate (0, Input.GetAxis ("Mouse X") * sensitivityX, 0);
			} else {
				rotationY += Input.GetAxis ("Mouse Y") * sensitivityY;
				rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
				transform.localEulerAngles = new Vector3 (-rotationY, transform.localEulerAngles.y, 0);
			}
		}
	}
}
