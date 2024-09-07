using UnityEngine;

namespace MS.OpenWorld.Occluder
{
    [RequireComponent(typeof(Renderer))]
    public class BuildingOccluder : MonoBehaviour, IOccluder
    {
        [SerializeField] private OccluderConfig _occluderConfigs;
        [SerializeField] private OcclusionPortal _occlusionPortal;

        private Renderer _renderer;
        private Material[] _originalMaterials;

        public void OnLoadGame()
        {
            if (!gameObject.activeInHierarchy)
                return;

            _renderer = GetComponent<Renderer>();

            _originalMaterials = _renderer.materials.Clone() as Material[];
            if (_occlusionPortal)
                _occlusionPortal.open = false;
        }

        void IOccluder.OnOccluding() {
            _renderer.materials = new Material[] { _occluderConfigs.BuildingOccludedMaterial };
        }

        void IOccluder.OnDeoccluding() {
            _renderer.materials = _originalMaterials;
        }
    }
}