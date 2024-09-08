using System.Collections;
using UnityEngine;

namespace PedestrianSystem {

    public class PedestrianSystem : MonoBehaviour {

        [SerializeField] private PedestrianSpawner m_PedestrianSpawner;
        [SerializeField] private float m_SpawnZoneRadius;
        [SerializeField] private float m_DespawnZoneRadius;

        private void Start() {
            CircleXzZone zone = new(m_SpawnZoneRadius, m_DespawnZoneRadius, AstarPath.active);

            m_PedestrianSpawner.Initialize(Camera.main, new ISpawnZone[] { zone }, new IDespawnZone[] { zone });
        }
    }
}