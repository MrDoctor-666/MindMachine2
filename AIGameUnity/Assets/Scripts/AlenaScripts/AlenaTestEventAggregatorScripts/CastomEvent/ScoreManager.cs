using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
	public int Scores = 0;

	public void Awake()
	{
		EventAggregator0.UnitDied.Subscribe(OnUnitDied);
	}

	private void OnUnitDied(Unit unit)
	{
		Scores += CalculateScores(unit);
		Debug.Log(Scores);
	}

	private int CalculateScores(Unit unit)
	{
		return unit.point;
	}
}
