using Pathfinding;
using System.Collections;
using UnityEngine;

namespace PedestrianSystem {

    public class Pedestrian : MonoBehaviour {

        [SerializeField] private AIPath m_AiPath;
        [SerializeField] private float m_WanderRadius;
        [SerializeField] private Transform m_DestDebug;

        private AstarPath m_AstarPath;
        private NNConstraint m_NnConstraint = NNConstraint.Walkable;
        private Coroutine m_BehaviourCo;

        public virtual void Initialize(PedestrianSpawner spawner) {
            m_AstarPath = AstarPath.active;
        }

        public virtual void OnSpawned() {
            SetRandomDestination();
            m_BehaviourCo = StartCoroutine(nameof(BehaviourCoroutine));
        }

        public virtual void OnDespawned() {
            StopCoroutine(m_BehaviourCo);
        }

        private void SetRandomDestination() {
            float randAngle = Random.value * Mathf.PI * 2f;
            var randOffset = new Vector3(Mathf.Cos(randAngle), 0f, Mathf.Sin(randAngle)) * m_WanderRadius;
            var randDest = transform.position + randOffset;

            var randDestInfo = m_AstarPath.GetNearest(randDest, m_NnConstraint);
            m_AiPath.destination = randDestInfo.position;
            m_DestDebug.position = randDestInfo.position;
        }

        private IEnumerator BehaviourCoroutine() {
            while (true) {
                if (m_AiPath.reachedDestination)
                    SetRandomDestination();
                
                yield return null;
            }
        }
    }
}