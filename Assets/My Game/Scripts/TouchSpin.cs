using UnityEngine;
using System.Collections;

public class TouchSpin : MonoBehaviour {

   public GameObject _objectToRotate;
   public float _rotationSpeed;
   private Vector3 _oldPos;
   private bool _rotating;

   private Transform trans
   {
      get { return _objectToRotate.transform; }
   }

	// Use this for initialization
	void Start ()
	{
	   //transform.rotation = trans.rotation;
	}

   void MoveCubeRay(Vector3 posOrig, Vector3 posEnd)
   {
      RaycastHit hitInfo1, hitInfo2;
      var ray1 = Camera.main.ScreenPointToRay(posOrig);
      var ray2 = Camera.main.ScreenPointToRay(posEnd);
      if (Physics.Raycast(ray1, out hitInfo1) && Physics.Raycast(ray2, out hitInfo2))
      {
         Vector3 startPoint = hitInfo1.point;
         Vector3 endPoint = hitInfo2.point;
         Vector3 rotationAxis = Vector3.Cross(startPoint, endPoint);
         float angle = Mathf.Acos(Vector3.Dot(startPoint.normalized, endPoint.normalized));
         angle *= Time.deltaTime * _rotationSpeed; 
         Debug.Log("Rotating: " + rotationAxis + " angles: " + angle);
         trans.Rotate(rotationAxis, angle);
         transform.Rotate(rotationAxis, angle);
      }
   }

   void MoveCube(Vector3 posOrig, Vector3 posEnd)
   {
      RaycastHit hitInfo1, hitInfo2;
      var ray1 = Camera.main.ScreenPointToRay(posOrig);
      var ray2 = Camera.main.ScreenPointToRay(posEnd);
      if (Physics.Raycast(ray1, out hitInfo1) && Physics.Raycast(ray2, out hitInfo2))
      {
         Vector3 startPoint = hitInfo1.point;
         Vector3 endPoint = hitInfo2.point;
         Vector3 rotationAxis = Vector3.Cross(startPoint, endPoint);
         float angle = Mathf.Acos(Vector3.Dot(startPoint.normalized, endPoint.normalized));
         angle *= Time.deltaTime * _rotationSpeed;
         //rotationAxis *= trans.rotation.eulerAngles;
         Debug.Log("Rotating: " + rotationAxis + " angles: " + angle);
         var localRotAxis = trans.InverseTransformDirection(rotationAxis);
         trans.Rotate(localRotAxis, angle);
         //transform.Rotate(localRotAxis, angle);
      }
   }
	
	// Update is called once per frame
	void Update ()
	{
      foreach (var touch in Input.touches)
      {
         if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Ended)
         {
            //var ray = Camera.main.ScreenPointToRay (touch.position);
            //if (Physics.Raycast (ray)) {
            //   // Create a particle if hit
            //   Instantiate (particle, transform.position, transform.rotation);
            //}
            MoveCube(touch.position - touch.deltaPosition, touch.position);
         }
      }

      if (Input.GetMouseButtonDown(0))
      {
         _oldPos = Input.mousePosition;
         _rotating = true;
         return;
      }
      if (Input.GetMouseButtonUp(0))
         _rotating = false;
      if (!_rotating && !Input.GetMouseButtonUp(0))
         return;

      var newPos = Input.mousePosition;
      Debug.Log("Moving cube: " + _oldPos + " -> " + newPos);
      MoveCube(_oldPos, newPos);
	   _oldPos = newPos;
	}
}
