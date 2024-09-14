using UnityEngine;
using Pathfinding;

public class CrossRoadGraphModifier : MonoBehaviour
{
	[SerializeField] private Collider m_Collider;
	[SerializeField] private int m_CrossRoadPenalty = 2;
	[SerializeField] private string m_CrossRoadTag = "Basic Ground";

	public bool IsOpen = false;
	private int m_DeltaPenalty;

    private void Start()
	{
		m_DeltaPenalty = m_CrossRoadPenalty;
		Apply();
		m_DeltaPenalty = 0;
    }

    public void Apply()
    {
        var guo = GetGraphUpdateObject();
        if (guo != null)
            AstarPath.active.UpdateGraphs(guo);
    }

    private GraphUpdateObject GetGraphUpdateObject()
    {
		return new(m_Collider.bounds)
		{
			nnConstraint = NNConstraint.Walkable,
			modifyWalkability = true,
			setWalkability = IsOpen,
			addPenalty = m_DeltaPenalty,
			modifyTag = true,
			setTag = PathfindingTag.FromName(m_CrossRoadTag),
		};
	}

    private void Reset()
    {
		if (m_Collider == null)
			TryGetComponent(out m_Collider);
    }
}
