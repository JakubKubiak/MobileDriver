using UnityEngine;
using System.Collections;

public struct CheckpointSaveData
{
    public Vector3  m_dragonPos;
	public float	m_dragonSpeed;
    public Vector3  m_playerPos;
    public int      m_segmentsCounter;
    public int      m_collectedStars;
	public Vector3	m_nextSpawnPos;
}

public class Checkpoint : MonoBehaviour 
{
    private const int   CHECKPOINT_RECORD_LIMIT = 20;    
    private const float SCALE_AMPLITUDE     = 0.25f;
    private const float SHAKE_CYCLE_TIME    = 0.2f;
    private const float SHAKES_AMOUNT       = 5;

	public bool		m_isHidden	= false;

    private EnvSegment[]       m_recordPatterns = new EnvSegment[CHECKPOINT_RECORD_LIMIT];
    private int                m_recorderEntries = 0;
    private Checkpoint          m_nextCheckpoint;

    public CheckpointSaveData m_savedData;

    private int m_playbackIndex = 0;

	private bool m_activated;
	private bool m_nextSpawned;

    public void Record( EnvSegment _segment )
    {
        if (m_recorderEntries >= CHECKPOINT_RECORD_LIMIT)
        {
            throw new System.Exception( "Too many entries recorded" );
        }

        m_recordPatterns[ m_recorderEntries++ ] = _segment;        
    }

    public EnvSegment NextPlayback()
    {
        if (m_playbackIndex > m_recorderEntries)
            return null;

        return m_recordPatterns[m_playbackIndex ++ ];
    }

	public void PrepareToReplay()
	{
		m_playbackIndex = 0;
	}

    IEnumerator Co_Shake()
    {
		Transform l_transform = transform.Find( "Flag" );

        float l_totalTime   = SHAKE_CYCLE_TIME * SHAKES_AMOUNT;
        float l_elapsedTime = 0;

        Vector3 l_scale = l_transform.localScale;
        float l_yInitScale = l_scale.y;
        float l_rad = 0;
		float l_coef = 0;
        while( l_elapsedTime < l_totalTime )
        {
			l_coef = 1 - l_elapsedTime / l_totalTime;
            l_rad = 3.14f * l_elapsedTime / SHAKE_CYCLE_TIME;
			l_scale.y = l_yInitScale + SCALE_AMPLITUDE * l_coef * Mathf.Sin(l_rad);
            l_transform.localScale = l_scale;
            l_elapsedTime += Time.deltaTime;
            yield return null;
        }
		l_scale.y = l_yInitScale;
		l_transform.localScale = l_scale;
    }


    private void StoreSavedData()
    {        
        Transform l_player = GameObject.FindGameObjectWithTag("Player").transform;

        m_savedData.m_playerPos 		= l_player.position;
		m_savedData.m_nextSpawnPos	 	= NextSpawnPos;
        m_savedData.m_segmentsCounter 	= SegmentsCount;

    }

	public int SegmentsCount
	{ get;set;}
	public Vector3 NextSpawnPos
	{ get;set;}

    public void DestroyWithNext()
    {
        if( m_nextCheckpoint != null )
        {
            m_nextCheckpoint.DestroyWithNext();
			m_nextCheckpoint = null;
        }
        Destroy( gameObject );
    }

    public void DestroyAscendings()
    {
        if( m_nextCheckpoint != null )
        {
            m_nextCheckpoint.DestroyWithNext();
			m_nextCheckpoint = null;
        }
    }

    public Checkpoint NextCheckpoint
    {
		set { m_nextCheckpoint = value; m_nextSpawned = true; }
    }

	public bool IfNextSpawned
	{
		get{ return m_nextSpawned; }
	}
}
