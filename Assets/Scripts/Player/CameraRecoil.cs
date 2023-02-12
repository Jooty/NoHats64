using UnityEngine;

public class CameraRecoil : MonoBehaviour
{

	[Header("Recoil Settings")]
	public float rotationSpeed = 6;
	public float returnSpeed = 25;

	private Vector3 currentRotation;
	private Vector3 Rot;

	private void FixedUpdate()
	{
		currentRotation = Vector3.Lerp(currentRotation, Vector3.zero, returnSpeed * Time.deltaTime);
		Rot = Vector3.Slerp(Rot, currentRotation, rotationSpeed * Time.fixedDeltaTime);
		transform.localRotation = Quaternion.Euler(Rot);
	}

	public void DoRecoil(Vector3 recoilRotation)
	{
		currentRotation += new Vector3(
			-recoilRotation.x,
			Random.Range(-recoilRotation.y, recoilRotation.y),
			Random.Range(-recoilRotation.z, recoilRotation.z));
	}

}