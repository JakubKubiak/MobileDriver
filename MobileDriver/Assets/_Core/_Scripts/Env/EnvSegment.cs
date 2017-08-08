using UnityEngine;
using System.Collections;

public class EnvSegment : MonoBehaviour 
{
	
	[SerializeField] private bool	m_usesComplexPhys;	

	private Transform m_transform;

	void Awake()
	{
		m_transform = transform;
	}

	public EDifficultyLevel Difficulty
	{ get; set; }

	public int Index
	{ get; set; }

	public Transform CachedTransform
	{
		get{ return m_transform; }
	}
}
