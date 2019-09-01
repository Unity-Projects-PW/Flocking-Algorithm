using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Flock/Filter/Same Flock"))]
public class SameFlockFilter : ContextFilter
{
	public override List<Transform> Filter(FlockAgent agent, List<Transform> originalNeighbors)
	{
		List<Transform> filtered = new List<Transform>();
		foreach (var item in originalNeighbors)
		{
			FlockAgent itemAgent = item.GetComponent<FlockAgent>();
			if(itemAgent != null && itemAgent.AgentFlock == agent.AgentFlock)
			{
				filtered.Add(item);
			}
		}

		return filtered;
	}
}
