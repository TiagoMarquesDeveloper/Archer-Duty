using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aim : MonoBehaviour {
    [SerializeField]
    float verticalMouseSensitivity = 3;
    [SerializeField]
    bool invertAim = false;
    Rigidbody rigidB;
    [SerializeField]
    Camera cam;
	// Use this for initialization
	void Start () {
        rigidB = GetComponent<Rigidbody>();
        cam = GetComponentInChildren<Camera>();
        Cursor.visible = false;
	}
	
	// Update is called once per frame
	void Update () {
        AimLogic();
	}

    void AimLogic()
    {
        int _aimModifier = -1;
        if (invertAim)
            _aimModifier = 1;
        float _leftRightValue = Input.GetAxisRaw("Mouse X");
        float _upDownValue = _aimModifier*Input.GetAxisRaw("Mouse Y");
        Vector3 _rotationX = new Vector3(_upDownValue, 0, 0);
        Vector3 _rotationY = new Vector3(0, _leftRightValue, 0);
        rigidB.MoveRotation(rigidB.rotation * Quaternion.Euler(_rotationY));
        cam.transform.Rotate(_rotationX / verticalMouseSensitivity);

    }
}
