using TMPro;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

using static Unity.Mathematics.math;
using Random = UnityEngine.Random;

public class MazeGen : MonoBehaviour
{
	[SerializeField, Tooltip("Use zero for random seed.")]
	int seed;

	[SerializeField, Range(0f, 1f)]
	float pickLastProbability, openDeadEndProbability, openArbitraryProbability;

	[SerializeField]
	MazeVisualization visualization;

	[SerializeField]
	int2 mazeSize;

	Maze maze;
	MazeCellObject[] cellObjects;

	void Awake ()
	{
		maze = new Maze(mazeSize);
		new GenerateMazeJob
		{
			maze = this.maze,
			seed = this.seed != 0 ? seed : Random.Range(1, int.MaxValue),
			pickLastProbability = this.pickLastProbability,
			openDeadEndProbability = this.openDeadEndProbability
		}.Schedule().Complete();
		if (cellObjects == null || cellObjects.Length != maze.Length){
			cellObjects = new MazeCellObject[maze.Length];
		}
		visualization.Visualize(maze, cellObjects);
	}

	void OnDestroy(){
		maze.Dispose();
	}
}