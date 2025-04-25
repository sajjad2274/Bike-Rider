var camera : GameObject;

function OnMouseOver () {
	if (Mathf.Approximately (Input.GetAxis("MouseLeft"), 1))
	 {
		Camera.main.GetComponent (MouseOrbit).AutoRotate = true;
	}
}