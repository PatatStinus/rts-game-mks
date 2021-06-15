using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.UI;
using TMPro;

namespace RTS
{
    public class SelectBox : MonoBehaviour
    {
        [SerializeField] private Box box;
        [SerializeField] private Collider[] selections;
        [SerializeField] private GameObject projectorObject;
        [SerializeField] private GameObject dropdownJobs;
        [SerializeField] private TextMeshProUGUI unitsText;
        private DecalProjector projector;
        private List<Unit> unitsSelected = new List<Unit>();
        private List<Buildings> buildingSelected = new List<Buildings>();
        private Camera cam;
        private Vector3 startPos, dragPos;
        private Ray ray;
        private string unitJob;

        private void Start()
        {
            cam = Camera.main;
            projector = projectorObject.transform.GetChild(0).gameObject.GetComponent<DecalProjector>();
        }

        void Update()
        {
            //shoot ray from cam
            ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            //Make box size
            if (Input.GetMouseButton(0) && Physics.Raycast(ray, out hit))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    startPos = hit.point;
                    box.baseMin = startPos;
                    projector.enabled = true;
                }

                dragPos = hit.point;
                box.baseMax = dragPos;
                projector.size = new Vector3(box.Size.x, box.Size.y + 5, box.Size.z);
                projectorObject.transform.position = box.Center;
            }
            else if (Input.GetMouseButtonUp(0)) //Get all villagers in box
            {
                if (box.Size.magnitude < 0.3f)
                    return;

                foreach (var unit in unitsSelected)
                    unit.Selected(false);
                if(buildingSelected.Count > 0)
                    buildingSelected[0].selected = false;

                unitsSelected.Clear();
                buildingSelected.Clear();

                projector.enabled = false;
                selections = Physics.OverlapBox(box.Center, box.Extents, Quaternion.identity);

                foreach (var obj in selections)
                {
                    Unit unit = obj.GetComponent<Unit>();
                    Buildings building = obj.GetComponent<Buildings>();

                    if (unit != null)
                    {
                        unit.Selected(true);
                        unitsSelected.Add(unit);
                    }

                    if(building != null)
                    {
                        buildingSelected.Add(building);
                        buildingSelected[0].selected = true;
                    }
                }
                for(int i = 0; i < unitsSelected.Count; i++)
                {
                    unitsSelected[i].totalVillagers = unitsSelected.Count;
                    unitsSelected[i].villagerArray = i;
                }
            }
            //Move villagers
            if (Physics.Raycast(ray, out hit) && unitsSelected.Count > 0 && Input.GetMouseButtonDown(1))
            {
                foreach (var unit in unitsSelected)
                {
                    unit.MoveUnit(hit.point);
                    unit.Selected(false);
                }
            }

            //UI Dropdown
            if (unitsSelected.Count == 0)
            {
                dropdownJobs.GetComponent<Dropdown>().value = 0;
                unitsText.text = "";
            }
            if (unitsSelected.Count > 0)
            {
                unitsText.text = "";
                for(int i = 0; i < unitsSelected.Count; i++)
                {
                    if (unitsSelected[i].job == 0)
                        unitJob = "None";
                    if (unitsSelected[i].job == 1)
                        unitJob = "Farmer";
                    if (unitsSelected[i].job == 2)
                        unitJob = "Miner";
                    if (unitsSelected[i].job == 3)
                        unitJob = "Lumberjack";

                    unitsText.text += $"Villager {i} | Job = {unitJob} | Wood = {unitsSelected[i].thisWood} | Stone = {unitsSelected[i].thisStone}";
                    unitsText.text += "\n";
                }
            }
        }

        public void ChangeJob(int job)
        {
            //Dropdown menu to switch job of villager
            foreach (var unit in unitsSelected)
                unit.job = job;
        }
    }

    //Select box size
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
