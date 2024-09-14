using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace PedestrianSystem {

    public class PedestrianSpawner : MonoBehaviour {

        [SerializeField] private Pedestrian m_PedestrianPrefab;
        [SerializeField] private int m_PedestrianDensity = 10;
        [SerializeField] private int m_MaxPedestrians = 100;

        private Camera m_Camera;
        private ISpawnZone[] m_SpawnZones;
        private IDespawnZone[] m_DespawnZones;
        private ObjectPool<Pedestrian> m_PedestrianPool;
        private List<Pedestrian> m_ActivePedestrians = new();

        public IReadOnlyList<Pedestrian> ActivePedestrians => m_ActivePedestrians;

        public void Initialize(Camera camera, ISpawnZone[] spawnZones, IDespawnZone[] despawnZones) {
            m_Camera = camera;
            m_SpawnZones = spawnZones;
            m_DespawnZones = despawnZones;

            m_PedestrianPool = new(
                OnCreate,
                OnGet,
                OnRelease,
                actionOnDestroy: null,
                defaultCapacity: m_PedestrianDensity,
                maxSize: m_MaxPedestrians
            );

            StartCoroutine(nameof(UpdateLoop));
        }

        private Pedestrian OnCreate() {
            var ped = Instantiate(m_PedestrianPrefab);
            ped.Initialize(this);

            return ped;
        }

        private void OnGet(Pedestrian ped) {
            ped.gameObject.SetActive(true);
        }

        private void OnRelease(Pedestrian ped) {
            ped.gameObject.SetActive(false);
        }

        private IEnumerator UpdateLoop() {
            List<Pedestrian> temp = new();

            while (true) {

                foreach (var ped in m_ActivePedestrians) {
                    bool inDespawnZone = false;

                    foreach (var despawnZone in m_DespawnZones) {
                        if (despawnZone.IsPointInZone(m_Camera, ped.transform.position)) {
                            inDespawnZone = true;
                            break;
                        }
                    }

                    if (inDespawnZone || ped.ShouldDespawn)
                    {
                        ped.OnDespawned();
                        m_PedestrianPool.Release(ped);
                        temp.Add(ped);
                    }
                }

                foreach (var ped in temp)
                    m_ActivePedestrians.Remove(ped);
                temp.Clear();

                int pedsToAdd = m_PedestrianDensity - m_ActivePedestrians.Count;

                for (int i = 0; i < pedsToAdd; i++) {
                    var randSpawnZone = m_SpawnZones[Random.Range(0, m_SpawnZones.Length)];
                    var randPos = randSpawnZone.GetSpawnPosition(m_Camera);
                    var randPed = m_PedestrianPool.Get();
                    randPed.transform.position = randPos;
                    randPed.OnSpawned();
                    temp.Add(randPed);
                }

                foreach (var ped in temp)
                    m_ActivePedestrians.Add(ped);
                temp.Clear();

                yield return null;
            }
        }
    }
    
}
