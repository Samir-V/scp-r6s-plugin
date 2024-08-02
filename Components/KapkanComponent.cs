using Exiled.API.Features.Toys;
using MEC;
using System.Collections.Generic;
using UnityEngine;

namespace SCPR6SPlugin
{
    internal class KapkanComponent : MonoBehaviour
    {
        public void Start()
        {
            laserCubes = new List<Primitive> ();

            MEC.Timing.RunCoroutine(LaserAnim());
        }

        public Vector3 direction { get; set; }
        public Vector3 position { get; set; }

        public bool active { get; set; } = true;
        
        private List<Primitive> laserCubes {  get; set; }

        private IEnumerator<float> LaserAnim()
        {
            while (active)
            {
                Vector3 offset = Vector3.zero;

                if (direction.x > 0)
                {
                    offset = new Vector3(0, 0, -0.2f);
                }
                else if (direction.x < 0)
                {
                    offset = new Vector3(0, 0, 0.2f);
                }
                else if (direction.z > 0)
                {
                    offset = new Vector3(0.2f, 0, 0);
                }
                else if (direction.z < 0)
                {
                    offset = new Vector3(-0.2f, 0, 0);
                }

                Vector3 startPos = position + (offset.normalized * 0.4f);

                for (int index = 0; index < 7; ++index)
                {
                    if (!active)
                    {
                        break;
                    }

                    Vector3 laserCubePos = startPos + offset * index;

                    Primitive laserCube = Primitive.Create(PrimitiveType.Cube, AdminToys.PrimitiveFlags.Visible, laserCubePos, null, new Vector3(0.1f, 0.1f, 0.1f), true, Color.red);

                    laserCubes.Add(laserCube);

                    yield return Timing.WaitForSeconds(0.67f);
                }

                for (int index = 0; index < laserCubes.Count; ++index)
                {
                    laserCubes[index].Destroy();
                }

                laserCubes.Clear();
            }

            if (!active)
            {
                for (int index = 0; index < laserCubes.Count; ++index)
                {
                    laserCubes[index].Destroy();
                }

                laserCubes.Clear();

                yield break;
            }
        }
    }
}
