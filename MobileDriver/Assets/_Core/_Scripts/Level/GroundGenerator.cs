using UnityEngine;
using System.Collections;

public class GroundGenerator : MonoBehaviour 
{
	[SerializeField] private Transform 	m_tilePattern;
	[SerializeField] private Vector3 	m_initPosition;
	[SerializeField] private float		m_tileDistance			= 5.6f;
	[SerializeField] private int		m_tilesInSegments		= 4;
	[SerializeField] private int		m_amountOfSegments		= 20;
	[SerializeField] private bool		m_spawnAtStart		= false;
	


	private Transform 	m_transform;
	private float		m_groundLevel;
	private float		m_cellingLevel;
	private float		m_segmentsDistance;

	void Awake()
	{
		m_segmentsDistance = m_tilesInSegments * m_tileDistance;

		m_transform = transform;
		
		m_cellingLevel 	= Camera.main.orthographicSize - 0.5f;
		m_groundLevel 	= -Camera.main.orthographicSize + 0.5f;
		
		Vector3 l_spawnPosition 	= m_initPosition;
		
		if( m_spawnAtStart )
		{
			for( int i=0; i < m_amountOfSegments; ++i )
			{
				SpawnSegment( l_spawnPosition );
				l_spawnPosition.x += m_segmentsDistance;
			}
		}
	}

	public void SpawnSegment( Vector3 _segmentMiddle, Transform _parent, bool _shift )
	{
		if( _shift )
		{
			_segmentMiddle.x -= m_tileDistance;
		}
		Vector3 l_cellingTilePos = _segmentMiddle;
		l_cellingTilePos.x -= m_tilesInSegments/2;
		l_cellingTilePos.y  = m_cellingLevel; 
		
		Vector3 l_groundTilePos = _segmentMiddle;
		l_groundTilePos.x -= m_tilesInSegments/2;
		l_groundTilePos.y  = m_groundLevel; 
		
		for( int i=0; i<m_tilesInSegments; ++i )
		{
			Transform l_spawnedCeill = ( Transform ) Instantiate( m_tilePattern, l_cellingTilePos, Quaternion.identity );
			Transform l_spawnedGround = ( Transform ) Instantiate( m_tilePattern, l_groundTilePos, Quaternion.identity );
			
			l_spawnedCeill.parent 	= _parent;
			l_spawnedGround.parent 	= _parent;
			
			l_cellingTilePos.x += m_tileDistance;
			l_groundTilePos.x += m_tileDistance;
		}
	}

	private void SpawnSegment( Vector3 _segmentMiddle )
	{
		SpawnSegment( _segmentMiddle, m_transform, false );
	}


	public float SegmentsDistance
	{
		get{ return m_segmentsDistance; }
	}

	public Vector3 InitSpawnPos
	{
		get{ return m_initPosition; }
	}
}
