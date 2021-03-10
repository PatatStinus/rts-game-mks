using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RTS
{
    public class Unit : MonoBehaviour
    {
        private List<Material> mat = new List<Material>();
        private List<Color> orgColors = new List<Color>();
        private NavMeshAgent agent;

        void Start()
        {
            MeshRenderer[] meshes = GetComponentsInChildren<MeshRenderer>();
            agent = GetComponent<NavMeshAgent>();

            foreach (var mesh in meshes)
            {
                foreach (var mat in mesh.materials)
                {
                    this.mat.Add(mat);
                    orgColors.Add(mat.GetColor("_Color"));
                }
            }
        }

        public void Selected(bool select)
        {
            for (int i = 0; i < mat.Count; i++)
                mat[i].SetColor("_Color", select ? Color.blue : orgColors[i]);
        }

        public void MoveUnit(Vector3 position)
        {
            agent.SetDestination(position);
        }
    }
}
