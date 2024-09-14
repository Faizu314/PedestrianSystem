using Pathfinding;
using UnityEngine;

namespace PedestrianSystem
{

    public class FovSpawnZone : MonoBehaviour, ISpawnZone
    {
        [SerializeField] private float m_SpawnRadius;
        [SerializeField] private Camera m_DebugCamera;

        private AstarPath m_AstarPath;

        private void Start()
        {
            m_AstarPath = AstarPath.active;
        }

        public Vector3 GetSpawnPosition(Camera cam)
        {
            float horizontalFov = Mathf.PI; //Camera.VerticalToHorizontalFieldOfView(cam.fieldOfView, Screen.width / Screen.height) * Mathf.Deg2Rad;
            float fullCircle = Mathf.PI * 2f;
            float sectorRatio = (fullCircle - horizontalFov) / fullCircle;
            float randAngle = Random.value * fullCircle * sectorRatio;
            float randLength = Random.value;

            Vector3 camForward = cam.transform.forward;
            camForward.y = 0f;
            Vector3 zoneForward = new(-Mathf.Sin(horizontalFov / 2f), 0f, Mathf.Cos(horizontalFov / 2f));
            Quaternion zoneForwardToCamForward = Quaternion.FromToRotation(zoneForward, camForward);

            Vector2 rand = new Vector2(Mathf.Sin(randAngle), Mathf.Cos(randAngle)) * Mathf.Lerp(0f, m_SpawnRadius, randLength);

            var localPos = zoneForwardToCamForward * new Vector3(rand.x, 0f, rand.y);
            var pos = cam.transform.position + localPos;
            pos.y = 0f;

            return m_AstarPath.GetNearest(pos, NNConstraint.Walkable).position;
        }

        private void OnDrawGizmos()
        {
            float horizontalFov = Mathf.PI; //Camera.VerticalToHorizontalFieldOfView(m_DebugCamera.fieldOfView, Screen.width / Screen.height) * Mathf.Deg2Rad;
            float fullCircle = Mathf.PI * 2f;
            float sectorRatio = (fullCircle - horizontalFov) / fullCircle;

            Vector3 camForward = m_DebugCamera.transform.forward;
            camForward.y = 0f;
            Vector3 zoneForward = new(-Mathf.Sin(horizontalFov / 2f), 0f, Mathf.Cos(horizontalFov / 2f));
            Quaternion zoneForwardToCamForward = Quaternion.FromToRotation(zoneForward, camForward);

            float maxAngle = fullCircle * sectorRatio;

            Gizmos.color = Color.red;

            for (int i = 0; i < 20; i++)
            {
                float angle = maxAngle * ((float)i / 20);

                Vector2 rand = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * m_SpawnRadius;

                var localPos = zoneForwardToCamForward * new Vector3(rand.x, 0f, rand.y);
                var pos = m_DebugCamera.transform.position + localPos;
                pos.y = 1f;

                Gizmos.DrawSphere(pos, 0.5f);
            }
        } 
    }
}