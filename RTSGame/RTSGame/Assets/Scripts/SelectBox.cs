using UnityEngine;
using System.Collections.Generic;

namespace RTS
{
    public class SelectBox : MonoBehaviour
    {
        [SerializeField] private Box box;
        [SerializeField] private Projector projector;
        [SerializeField] private Collider[] selections;
        private List<Unit> unitsSelected = new List<Unit>();
        private Camera cam;
        private Vector3 startPos, dragPos;
        private Ray ray;

        private void Start()
        {
            cam = Camera.main;
        }

        void Update()
        {
            if (Input.GetMouseButton(0))
            {
                ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        startPos = hit.point;
                        box.baseMin = startPos;
                        projector.enabled = true;
                    }

                    dragPos = hit.point;
                    box.baseMax = dragPos;
                    projector.aspectRatio = box.Size.x / box.Size.z;
                    projector.orthographicSize = box.Size.z * 0.5f;
                    projector.transform.position = box.Center;
                }
                if (Physics.Raycast(ray, out hit) && unitsSelected.Count > 0 && Input.GetMouseButtonDown(0)) 
                {
                    foreach (var unit in unitsSelected)
                    {
                        unit.MoveUnit(hit.point);
                    }
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                foreach(var unit in unitsSelected)
                {
                    unit.Selected(false);
                }

                unitsSelected.Clear();

                if (box.Size.magnitude < 0.1f)
                    return;

                projector.enabled = false;
                selections = Physics.OverlapBox(box.Center, box.Extents, Quaternion.identity);

                foreach (var obj in selections)
                {
                    Unit unit = obj.GetComponent<Unit>();

                    if (unit != null)
                    {
                        unit.Selected(true);
                        unitsSelected.Add(unit);
                    }
                }
            }
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            Gizmos.DrawWireCube(box.Center, box.Size);
        }
    }

    [System.Serializable]
    public class Box
    {
        public Vector3 baseMin, baseMax;

        public Vector3 Center
        {
            get
            {
                Vector3 center = baseMin + (baseMax - baseMin) * 0.5f;
                center.y = (baseMax - baseMin).magnitude * 0.5f;
                return center;
            }
        }

        public Vector3 Size
        {
            get { return new Vector3(Mathf.Abs(baseMax.x - baseMin.x), (baseMax - baseMin).magnitude, Mathf.Abs(baseMax.z - baseMin.z)); }
        }

        public Vector3 Extents
        {
            get { return Size * 0.5f; }
        }
    }
}
