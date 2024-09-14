using UnityEngine;

namespace PedestrianSystem {

    public interface ISpawnZone {
        Vector3 GetSpawnPosition(Camera cam);
    }

    public interface IDespawnZone {
        bool IsPointInZone(Camera cam, Vector3 point);
    }

    public class CircleXzZone : IDespawnZone {

        private readonly float m_DespawnRadius;

        public CircleXzZone(float despawnZoneRadius) {
            m_DespawnRadius = despawnZoneRadius;
        }

        bool IDespawnZone.IsPointInZone(Camera cam, Vector3 point) {
            return Vector3.SqrMagnitude(point - cam.transform.position) > m_DespawnRadius * m_DespawnRadius;
        }
    }
}