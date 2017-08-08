using UnityEngine;
using System.Collections;

[System.Serializable]
public class DifficultyLevelProbability
{
	public int 		m_startingSegment	= -4;
	public int 		m_endingSegment		= 100;
	public float	m_weight			= 1;
}

public class SegmentsSpawningManager : MonoBehaviour 
{
	private const int MANAGED_SEGMENTS		= 2;
	private const int SLOPE_LEN 			= 4;
	private const int SLOPE_LEN_RED			= SLOPE_LEN - 1;
	private const int MAX_DIFF_LEVELS 		= 5;
	
    //----> need to add segment spawn position override or sepratate init sapwn for ground and level
	[SerializeField] private float 						m_segmentsDistance;
	[SerializeField] private Vector3					m_initSpawnpos;
	[SerializeField] private Transform					m_segmentTemplatePattern;
	[SerializeField] private DifficultyLevelProbability m_veryEasyPorb;
	[SerializeField] private DifficultyLevelProbability m_easyPorb;
	[SerializeField] private DifficultyLevelProbability m_medium;
	[SerializeField] private DifficultyLevelProbability m_hardPorb;
	[SerializeField] private DifficultyLevelProbability m_veryHardPorb;

	[SerializeField] private bool				m_forceDifficultyLevel	= false;
	[SerializeField] private EDifficultyLevel	m_forcedDifficultyLevel;

	[SerializeField] private int				m_checkopintFreqFrom 	= 3;
	[SerializeField] private int				m_checkopintFreqTo 		= 5;
	[SerializeField] private Checkpoint			m_initEmptyCheckpointPattern;
	[SerializeField] private Transform			m_checkpointPattern;
	private int									m_nextCheckpoint;

	//private GroundGenerator 	m_groundGenerator;
	private SegmensDictionary	m_segmentsDictionary;

	private float		m_despawnDegmentDistance;
	private Vector3		m_nextFreePosition;
	private int			m_totalSegmentCounter;
	private Transform	m_transform;
	private Transform	m_playerTransform;

	private EnvSegment[]	m_managedSegments;
	private Transform		m_firstManagedTransform;
    private Checkpoint      m_currentCheckopint;
    private Checkpoint      m_checkpoinToReplayFrom;

	void Start () 
	{
		m_transform				= transform;
		m_segmentsDictionary	= GetComponent< SegmensDictionary >();

		m_playerTransform		= GameObject.FindGameObjectWithTag( "Player" ).transform;
		m_nextFreePosition			= m_initSpawnpos;
		m_despawnDegmentDistance	= m_segmentsDistance * 1.5f;
		m_totalSegmentCounter		= 0;
		m_nextCheckpoint			= Random.Range( m_checkopintFreqFrom, m_checkopintFreqTo );
        m_currentCheckopint         = null;
		InitManagedSegments();
	}

	private void InitManagedSegments()
	{
		m_currentCheckopint = ( Checkpoint ) Instantiate( m_initEmptyCheckpointPattern, Vector3.zero, Quaternion.identity );
		m_currentCheckopint.SegmentsCount = 0;
		m_currentCheckopint.NextSpawnPos = m_nextFreePosition;

		m_managedSegments = new EnvSegment[ MANAGED_SEGMENTS ];

		for( int i=0; i<MANAGED_SEGMENTS; ++i )
		{
			m_managedSegments[ i ] = SpawnSegment( false );
		}

		m_firstManagedTransform = m_managedSegments[ 0 ].CachedTransform;
	}

    private EnvSegment SpawnSegment( bool _empty )
	{
        if (m_checkpoinToReplayFrom != null)
        {
            EnvSegment l_spawned = ReplaySpawn();
            if (l_spawned == null)
            {
				if( m_checkpoinToReplayFrom.IfNextSpawned )
				{
					m_nextCheckpoint = m_totalSegmentCounter;
				}
				l_spawned = NormalSpawn(_empty);
				m_checkpoinToReplayFrom = null;
				return l_spawned;
            }
            else
            {
                return l_spawned;
            }
        }
        else
        {
            return NormalSpawn( _empty );
        }
	}

    private EnvSegment ReplaySpawn()
    {        
        EnvSegment l_pattern = m_checkpoinToReplayFrom.NextPlayback();
        if (l_pattern == null)
        {            
            return null;
        }

        EnvSegment l_instance = Instantiate(l_pattern, m_nextFreePosition, Quaternion.identity) as EnvSegment;

        l_instance.transform.parent = m_transform;

        Transform l_spawnedSegment = (Transform)Instantiate(m_segmentTemplatePattern, m_nextFreePosition, m_segmentTemplatePattern.rotation);
        l_spawnedSegment.parent = l_instance.CachedTransform;
        m_nextFreePosition.z += m_segmentsDistance;
        ++m_totalSegmentCounter;
        return l_instance;
    }

	private EnvSegment SpawnEmptyBeforeCheckpoint()
	{
		EnvSegment l_pattern = m_segmentsDictionary.EmptySegment;

        if (l_pattern == null)
        {
            EDifficultyLevel l_dLevel = ChooseCurrentDifficulty();           
            l_pattern = m_segmentsDictionary.RandSegment(l_dLevel);
        }
        EnvSegment l_instance = Instantiate(l_pattern, m_nextFreePosition, Quaternion.identity) as EnvSegment;
		
		l_instance.transform.parent = m_transform;
		Transform l_spawnedSegment = (Transform)Instantiate(m_segmentTemplatePattern, m_nextFreePosition, m_segmentTemplatePattern.rotation);
		l_spawnedSegment.parent = l_instance.CachedTransform;
		return l_instance;
	}

    private EnvSegment NormalSpawn( bool _empty )
    {
        ++m_totalSegmentCounter;
        SpawnCheckpointIfNeeded();
        EnvSegment l_pattern = null;
        if (_empty)
        {
            l_pattern = m_segmentsDictionary.EmptySegment;
        }
        else if ( !_empty || l_pattern == null )
        {
            EDifficultyLevel l_dLevel = ChooseCurrentDifficulty();
            //Debug.Log( "chosen difficulty is " + l_dLevel );
            l_pattern = m_segmentsDictionary.RandSegment(l_dLevel);
        }

		Transform l_spawnedSegment = (Transform)Instantiate(m_segmentTemplatePattern, m_nextFreePosition, m_segmentTemplatePattern.rotation);      
        EnvSegment l_instance = Instantiate(l_pattern, m_nextFreePosition, l_pattern.transform.rotation) as EnvSegment;
        l_instance.transform.parent = m_transform;
		        
        l_spawnedSegment.parent = l_instance.CachedTransform;
        if (m_currentCheckopint != null)
        {
            m_currentCheckopint.Record(l_pattern);
        }      

        m_nextFreePosition.z += m_segmentsDistance;

        return l_instance;
    }

	private void SpawnCheckpointIfNeeded()
	{
		if( m_totalSegmentCounter < m_nextCheckpoint )
			return;
		m_nextFreePosition.z += m_segmentsDistance/4;
		Vector3 l_pos = m_nextFreePosition;
		l_pos.z -= 30;

		Transform l_spawnedCheckopint = ( Transform ) Instantiate( m_checkpointPattern, l_pos, Quaternion.identity );
        Checkpoint l_checkpointCmp = l_spawnedCheckopint.GetComponent<Checkpoint>();
        if (m_currentCheckopint != null)
        {
            m_currentCheckopint.NextCheckpoint = l_checkpointCmp;
        }
        m_currentCheckopint = l_checkpointCmp;
		m_currentCheckopint.SegmentsCount = m_totalSegmentCounter;
		m_currentCheckopint.NextSpawnPos = m_nextFreePosition;
		m_nextCheckpoint = m_totalSegmentCounter + Random.Range( m_checkopintFreqFrom, m_checkopintFreqTo );
	}

	void Update () 
	{
		float l_distance = ( m_playerTransform.position - m_firstManagedTransform.position ).z;

		if( l_distance < m_despawnDegmentDistance )
			return;

		Destroy( m_firstManagedTransform.gameObject );

		for( int i=0; i<MANAGED_SEGMENTS-1; ++i )
		{
			m_managedSegments[ i ] = m_managedSegments[ i + 1 ];
		}

		m_managedSegments[ MANAGED_SEGMENTS - 1 ] = SpawnSegment( false );
		m_firstManagedTransform = m_managedSegments[ 0 ].CachedTransform;
	}


	struct ConsideredDiffLevel
	{
		public EDifficultyLevel m_level;
		public float			m_weight;
	}

	private ConsideredDiffLevel[] m_consideredDiffLevels = new ConsideredDiffLevel[ MAX_DIFF_LEVELS ];
	private int m_consideredAmount;

	private EDifficultyLevel ChooseCurrentDifficulty()
	{
		if( m_forceDifficultyLevel )
			return m_forcedDifficultyLevel;

		FillConsideredData();

		if( m_consideredAmount == 0 )
		{
			return EDifficultyLevel.Medium;
		}

		float l_totalW = 0;
		
		for( int i=0; i< m_consideredAmount; ++i )
		{
			l_totalW += m_consideredDiffLevels[ i ].m_weight;
		}


		float l_random = Random.value * l_totalW;
		l_totalW = 0;
		for( int i=0; i< m_consideredAmount; ++i )
		{
			l_totalW += m_consideredDiffLevels[ i ].m_weight;
			if( l_random <= l_totalW )
				return m_consideredDiffLevels[ i ].m_level;
		}

		return m_forcedDifficultyLevel;
	}

	private void FillConsideredData()
	{
		m_consideredAmount = 0;

		FillConsideredData( ref m_veryEasyPorb	, EDifficultyLevel.VeryEasy 	);
		FillConsideredData( ref m_easyPorb		, EDifficultyLevel.Easy 		);
		FillConsideredData( ref m_medium		, EDifficultyLevel.Medium 		);
		FillConsideredData( ref m_hardPorb		, EDifficultyLevel.Hard 		);
		FillConsideredData( ref m_veryHardPorb	, EDifficultyLevel.VeryHard 	);
	}

	private void FillConsideredData( ref DifficultyLevelProbability _diffLevel, EDifficultyLevel _level )
	{

		if( m_totalSegmentCounter < _diffLevel.m_startingSegment || m_totalSegmentCounter > _diffLevel.m_endingSegment )
			return;

		m_consideredDiffLevels[ m_consideredAmount ].m_level 	= _level;

		if( m_totalSegmentCounter >= _diffLevel.m_startingSegment + SLOPE_LEN_RED || m_totalSegmentCounter >= _diffLevel.m_endingSegment - SLOPE_LEN_RED )
		{
			m_consideredDiffLevels[ m_consideredAmount ].m_weight = _diffLevel.m_weight;
		}
		else
		{
			float l_dif = Mathf.Min(
				m_totalSegmentCounter - _diffLevel.m_startingSegment,
				_diffLevel.m_endingSegment - m_totalSegmentCounter
				)  + 1;
			l_dif /= SLOPE_LEN;
			l_dif = Mathf.Abs( l_dif );
			m_consideredDiffLevels[ m_consideredAmount ].m_weight = _diffLevel.m_weight * l_dif;
		}

		++m_consideredAmount;
	}

    public void ReplayFromCheckpoint( Checkpoint _checkpoint )
    {
		_checkpoint.PrepareToReplay();
        _checkpoint.DestroyAscendings();
        m_checkpoinToReplayFrom = _checkpoint;
        m_totalSegmentCounter = _checkpoint.m_savedData.m_segmentsCounter;
		m_nextFreePosition = _checkpoint.m_savedData.m_nextSpawnPos;
		m_nextFreePosition.z -= m_segmentsDistance + 11;

        for (int i = 0; i < MANAGED_SEGMENTS; ++i)
        {
            Destroy( m_managedSegments[i].gameObject );
        }

		m_managedSegments[ 0 ] = SpawnEmptyBeforeCheckpoint();
		m_nextFreePosition = _checkpoint.m_savedData.m_nextSpawnPos;

        for (int i = 1; i < MANAGED_SEGMENTS; ++i)
        {
            m_managedSegments[i] = SpawnSegment(false);
        }

        m_firstManagedTransform = m_managedSegments[0].CachedTransform;
    }

    public int SegmentsCounter
    {
        get { return m_totalSegmentCounter;  }
    }
}
