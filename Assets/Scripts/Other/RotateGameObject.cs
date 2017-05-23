using UnityEngine;

public class RotateGameObject : MonoBehaviour
{
    public Vector3 angle;

	private void Update ()
    {
        transform.Rotate(angle * Time.deltaTime);
	}
}
