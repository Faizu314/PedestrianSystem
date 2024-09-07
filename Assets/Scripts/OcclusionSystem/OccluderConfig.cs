using UnityEngine;

[CreateAssetMenu(fileName="OccluderConfig", menuName="OpenWorld/Occluder/Configs")]
public class OccluderConfig : ScriptableObject
{
    public Material BuildingOccludedMaterial;
    public string BuildingOccludedMaterialColorProperty;
}
