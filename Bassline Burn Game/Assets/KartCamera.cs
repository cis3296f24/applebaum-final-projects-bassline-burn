using UnityEngine;

public class KartCamera : KartComponent, ICameraController
{

	public Transform camNode;
	public Transform finishVP;
    public Transform forwardVP;
	public float lerpFactorVP = 3f;
	public float lerpFactorFOV = 1.5f;
	public float normalFOV = 60;
	public float boostFOV = 70;
	public float finishFOV = 45;
	public bool useFinishVP;

	private float _currentFOV = 60;
	private Transform _viewpoint;
	private bool _shouldLerpCamera = true;
	private bool _lastFrameLookBehind;

	public override void OnLapCompleted(int lap, bool isFinish)
	{
		base.OnLapCompleted(lap, isFinish);

		if (isFinish)
		{
			useFinishVP = true;
		}
	}

	public override void Render()
	{
		base.Render();

		if (Object.HasInputAuthority && _shouldLerpCamera && !GameManager.IsCameraControlled)
		{
			GameManager.GetCameraControl(this);
		}
	}

	public bool ControlCamera(Camera cam)
	{
		if (this.Equals(null))
		{
			Debug.LogWarning("Releasing camera from kart");
			return false;
		}

		_viewpoint = GetViewpoint();

		if (_shouldLerpCamera)
			ControlCameraLerp(cam);
		else
			ControlCameraDriving(cam);

		return true;
	}

	private void ControlCameraDriving(Camera cam)
	{


		cam.transform.position = camNode.position;
		
		SetFOV(cam);
	}

	private void ControlCameraLerp(Camera cam)
	{
		cam.transform.position = Vector3.Lerp(cam.transform.position, camNode.position, Time.deltaTime * 2f);
		cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, camNode.rotation, Time.deltaTime * 2f);
		if (Vector3.Distance(cam.transform.position, camNode.position) < 0.05f &&
			Vector3.Dot(cam.transform.forward, camNode.forward) > 0.95f)
		{
			_shouldLerpCamera = false;
		}
	}

	private void SetFOV(Camera cam)
	{
		_currentFOV = useFinishVP ? finishFOV : normalFOV;
		cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, _currentFOV, Time.deltaTime * lerpFactorFOV);
	}

	private Transform GetViewpoint()
	{
		if (Kart.Controller == null) return null;
		if (useFinishVP) return finishVP;


		return forwardVP;
	}
}