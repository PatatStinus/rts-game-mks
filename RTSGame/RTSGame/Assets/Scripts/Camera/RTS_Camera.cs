using UnityEngine;
using System.Collections;

namespace RTS_Cam
{
    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("RTS Camera")]
    public class RTS_Camera : MonoBehaviour
    {
        public int lastTab = 0;
        public bool movementSettingsFoldout;
        public bool zoomingSettingsFoldout;
        public bool heightSettingsFoldout;
        public bool inputSettingsFoldout;
        private Transform m_Transform;
        public bool useFixedUpdate = false;
        public float keyboardMovementSpeed = 5f;
        public float panningSpeed = 5f;
        public bool autoHeight = true;
        public LayerMask groundMask = -1;
        public float maxHeight = 5f;
        public float minHeight = 5f;
        public float heightDampening = 1f; 
        public float scrollWheelZoomingSensitivity = 25f;
        private float zoomPos = 0;
        public bool useKeyboardInput = true;
        public string horizontalAxis = "Horizontal";
        public string verticalAxis = "Vertical";
        public bool usePanning = true;
        public KeyCode panningKey = KeyCode.Mouse1;
        public bool useScrollwheelZooming = true;
        public string zoomingAxis = "Mouse ScrollWheel";

        private Vector2 KeyboardInput
        {
            get { return useKeyboardInput ? new Vector2(Input.GetAxis(horizontalAxis), Input.GetAxis(verticalAxis)) : Vector2.zero; }
        }
        private Vector2 MouseInput
        {
            get { return Input.mousePosition; }
        }
        private float ScrollWheel
        {
            get { return -Input.GetAxis(zoomingAxis); }
        }
        private Vector2 MouseAxis
        {
            get { return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")); }
        }
        private void Start()
        {
            m_Transform = transform;
        }
        private void Update()
        {
            if (!useFixedUpdate)
                CameraUpdate();
        }
        private void FixedUpdate()
        {
            if (useFixedUpdate)
                CameraUpdate();
        }
        private void CameraUpdate()
        {
            Move();
            HeightCalculation();
        }
        private void Move()
        {
            if (useKeyboardInput)
            {
                Vector3 desiredMove = new Vector3(KeyboardInput.x, 0, KeyboardInput.y);

                desiredMove *= keyboardMovementSpeed;
                desiredMove *= Time.deltaTime;
                desiredMove = Quaternion.Euler(new Vector3(0f, transform.eulerAngles.y, 0f)) * desiredMove;
                desiredMove = m_Transform.InverseTransformDirection(desiredMove);

                m_Transform.Translate(desiredMove, Space.Self);
            }
            if(usePanning && Input.GetKey(panningKey) && MouseAxis != Vector2.zero)
            {
                Vector3 desiredMove = new Vector3(-MouseAxis.x, 0, -MouseAxis.y);

                desiredMove *= panningSpeed;
                desiredMove *= Time.deltaTime;
                desiredMove = Quaternion.Euler(new Vector3(0f, transform.eulerAngles.y, 0f)) * desiredMove;
                desiredMove = m_Transform.InverseTransformDirection(desiredMove);

                m_Transform.Translate(desiredMove, Space.Self);
                Cursor.visible = false;
            }
            if (Input.GetKeyUp(panningKey))
                Cursor.visible = true;
        }
        private void HeightCalculation()
        {
            float distanceToGround = DistanceToGround();
            if(useScrollwheelZooming)
                zoomPos += ScrollWheel * Time.deltaTime * scrollWheelZoomingSensitivity;

            zoomPos = Mathf.Clamp01(zoomPos);

            float targetHeight = Mathf.Lerp(minHeight, maxHeight, zoomPos);
            float difference = 0; 

            if(distanceToGround != targetHeight)
                difference = targetHeight - distanceToGround;

            m_Transform.position = Vector3.Lerp(m_Transform.position, 
                new Vector3(m_Transform.position.x, targetHeight + difference, m_Transform.position.z), Time.deltaTime * heightDampening);
        }
        private float DistanceToGround()
        {
            Ray ray = new Ray(m_Transform.position, Vector3.down);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, groundMask.value))
                return (hit.point - m_Transform.position).magnitude;

            return 0f;
        }
    }
}