using Pathfinding;
using System.Collections;
using UnityEngine;

namespace PedestrianSystem {

    public class Pedestrian : MonoBehaviour {

        [SerializeField] private float m_WanderRadius;
        [SerializeField] private Animator m_Animator;
        [SerializeField] private float m_DefaultAnimatorSpeedMultiplier = 0.25f;
        [SerializeField] private Transform m_DestDebug;

        private IAstarAI m_Ai;
        private AstarPath m_AstarPath;
        private NNConstraint m_NnConstraint = NNConstraint.Walkable;
        private Coroutine m_BehaviourCo;

        public virtual void Initialize(PedestrianSpawner spawner) {
            m_AstarPath = AstarPath.active;
            m_Ai = GetComponent<IAstarAI>();
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
            m_Ai.destination = randDestInfo.position;
        }

        private IEnumerator BehaviourCoroutine() {
            while (true) {
                if (m_Ai.reachedDestination)
                    SetRandomDestination();

                var vel = m_Ai.velocity;
                vel.y = 0f;
                float speedNormalized = Mathf.Clamp(vel.magnitude, 0f, m_Ai.maxSpeed) / m_Ai.maxSpeed;

                m_Animator.SetFloat("Speed", speedNormalized * m_DefaultAnimatorSpeedMultiplier);
                    
                if (m_DestDebug != null)
                    m_DestDebug.position = m_Ai.destination;

                yield return null;
            }
        }
    }
}