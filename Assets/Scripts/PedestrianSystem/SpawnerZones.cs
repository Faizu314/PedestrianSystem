using UnityEngine;

namespace PedestrianSystem {

    public interface ISpawnZone {
        Vector3 GetSpawnPosition(Camera cam);
    }

    public interface IDespawnZone {
        bool IsPointInZone(Camera cam, Vector3 point);
    }

    public class CircleXzZone : ISpawnZone, IDespawnZone {

        private readonly float m_SpawnRadius;
        private readonly float m_DespawnRadius;

        public CircleXzZone(float spawnZoneRadius, float despawnZoneRadius) {
            m_SpawnRadius = spawnZoneRadius;
            m_DespawnRadius = despawnZoneRadius;
        }

        Vector3 ISpawnZone.GetSpawnPosition(Camera cam) {
            float randAngle = Random.value * Mathf.PI * 2f;
            Vector2 rand = new Vector2(Mathf.Cos(randAngle), Mathf.Sin(randAngle)) * m_SpawnRadius;

            var pos = cam.transform.position + new Vector3(rand.x, 0f, rand.y);
            pos.y = 0f;

            return pos;
        }

        bool IDespawnZone.IsPointInZone(Camera cam, Vector3 point) {
            return Vector3.SqrMagnitude(point - cam.transform.position) > m_DespawnRadius * m_DespawnRadius;
        }
    }
}