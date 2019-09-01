using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Flock : MonoBehaviour
{
	public FlockAgent agentPrefab;
	List<FlockAgent> agents = new List<FlockAgent>();
	public FlockBehavior behaviour;

	[Range(10, 500)]
	public int startingCount = 250;
	const float AgentDensity = 0.08f;

	[Range(1f, 100f)]
	public float driveFactor = 10f;
	[Range(1f, 100f)]
	public float maxSpeed = 5f;

	[Range(1f, 10f)]
	public float neighborRadius = 1.5f;
	[Range(0f, 1f)]
	public float avoidanceRadiusMultiplayer = 0.5f;

	float squareMaxSpeed;
	float squareNeigborRadius;
	float squareAvoidanceRadius;
	public float SquareAvoidanceRadius { get { return squareAvoidanceRadius; } }

	private void Start()
	{
		squareMaxSpeed = maxSpeed * maxSpeed;
		squareNeigborRadius = neighborRadius * neighborRadius;
		squareAvoidanceRadius = squareNeigborRadius * avoidanceRadiusMultiplayer * avoidanceRadiusMultiplayer;

		for (int i = 0; i < startingCount; i++)
		{
			FlockAgent newAgent = Instantiate(
				agentPrefab,
				Random.insideUnitCircle * startingCount * AgentDensity,
				Quaternion.Euler(0, 0, UnityEngine.Random.Range(0f, 360f)),
				transform
				);

			newAgent.name = "Agent" + i;
			newAgent.InitializeFlock(this);
			agents.Add(newAgent);
		}

	}

	private void Update()
	{
		foreach (var agent in agents)
		{
			List<Transform> context = GetNearbyObjects(agent);
			//demo
			//agent.GetComponentInChildren<SpriteRenderer>().color = Color.Lerp(Color.white, Color.red, context.Count / 6f);

			Vector2 move = behaviour.CalculateMove(agent, context, this);
			move *= driveFactor;
			if (move.sqrMagnitude > squareMaxSpeed)
				move = move.normalized * maxSpeed;
			agent.Move(move);
		}
	}

	private List<Transform> GetNearbyObjects(FlockAgent agent)
	{
		//Physics overlap is better performarnce wise than calculating distance for everyone
		List<Transform> context = new List<Transform>();
		Collider2D[] contextColliders = Physics2D.OverlapCircleAll(agent.transform.position, neighborRadius);

		foreach (Collider2D collider in contextColliders)
		{
			if(collider != agent.AgentCollider)
			{
				context.Add(collider.transform);
			}
		}

		return context;

	}
}
