using UnityEngine;

namespace PedestrianSystem {

    public class PedestrianSystem : MonoBehaviour {

        [SerializeField] private PedestrianSpawner m_PedestrianSpawner;
        [SerializeField] private FovSpawnZone m_SpawnZone;
        [SerializeField] private float m_DespawnZoneRadius;

        private void Start() {
            CircleXzZone zone = new(m_DespawnZoneRadius);

            m_PedestrianSpawner.Initialize(Camera.main, new ISpawnZone[] { m_SpawnZone }, new IDespawnZone[] { zone });
        }

        private void Update()
        {
            foreach (var ped in m_PedestrianSpawner.ActivePedestrians)
                ped.IsVisible = IsBoundsInFrustum(ped.Bounds);
        }

        private bool IsBoundsInFrustum(Bounds bounds)
        {
            return true;
        }
    }
}