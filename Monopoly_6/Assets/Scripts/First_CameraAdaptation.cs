using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAdaptation_First : MonoBehaviour {

	public float baseWidth = 1080;
	public float baseHeight = 1920;
	public float baseOrthographicSize = 5;
	public Camera cam;

	private float baseAspect;
	private float targetAspect;
	private float m03;
	private float m13;
	private float m33;

	void Awake(){

		float newOrthographicSize = (float)Screen.height / (float)Screen.width * this.baseWidth / this.baseHeight * this.baseOrthographicSize;
		cam.orthographicSize = Mathf.Max(newOrthographicSize , this.baseOrthographicSize);

		float targetWidth = (float)Screen.width;
		float targetHeight = (float)Screen.height;

		this.baseAspect = this.baseWidth / this.baseHeight;
		this.targetAspect = targetWidth / targetHeight;

		float factor = this.targetAspect > this.baseAspect ? targetHeight / this.baseHeight : targetWidth / this.baseWidth;

		this.m33 = 1 / factor;
		this.m03 = (targetWidth - this.baseWidth * factor) / 2 * this.m33;
		this.m13 = (targetHeight - this.baseHeight * factor) / 2 * this.m33;
	}

	void OnGUI(){

		Matrix4x4 _matrix = GUI.matrix;

		_matrix.m33 = this.m33;

		if(this.targetAspect > this.baseAspect) _matrix.m03 = this.m03;
		else _matrix.m13 = this.m13;

		GUI.matrix = _matrix;

		//... Your GUI ...
	}
}

