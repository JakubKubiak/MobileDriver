using UnityEngine;
using System.Collections;

public enum EDifficultyLevel
{
	VeryEasy,
	Easy,
	Medium,
	Hard,
	VeryHard
}

public class SegmensDictionary : MonoBehaviour 
{
	[SerializeField] private EnvSegment	  m_empty;
	[SerializeField] private EnvSegment[] m_veryEasy;
	[SerializeField] private EnvSegment[] m_easy;
	[SerializeField] private EnvSegment[] m_medium;
	[SerializeField] private EnvSegment[] m_hard;
	[SerializeField] private EnvSegment[] m_veryHard;

	private BitArray m_veryEasyUsed;
	private BitArray m_easyUsed;
	private BitArray m_meduimUsed;
	private BitArray m_hardUsed;
	private BitArray m_veryHardUsed;


    void Awake()
    {
        SetupSegments( m_veryEasy, EDifficultyLevel.VeryEasy );
        SetupSegments( m_easy, EDifficultyLevel.Easy );
        SetupSegments( m_medium, EDifficultyLevel.Medium );
        SetupSegments( m_hard, EDifficultyLevel.Hard );
        SetupSegments( m_veryHard, EDifficultyLevel.VeryHard );    

		m_veryEasyUsed 	= new BitArray( m_veryEasy.Length, false );
		m_easyUsed		= new BitArray( m_easy.Length, false );
		m_meduimUsed	= new BitArray( m_medium.Length, false );
		m_hardUsed		= new BitArray( m_hard.Length, false );
		m_veryHardUsed	= new BitArray( m_veryHard.Length, false );
    }

    private void SetupSegments( EnvSegment[] _segments, EDifficultyLevel _level )
    {
        for( int i=0; i<_segments.Length; ++i )
        {
            _segments[ i ].Difficulty   = _level;
            _segments[ i ].Index        = i;
        }
    }

	public EnvSegment RandSegment( EDifficultyLevel _difficulty )
	{
		EnvSegment[] l_source 	= GetArray( _difficulty );
		BitArray l_usage		= GetUsage( _difficulty );


		int l_randIndex = Random.Range( 0, l_source.Length );

		//if not used yet, then return
		if( !l_usage[ l_randIndex ] )
		{
			l_usage[ l_randIndex ] = true;
			return l_source[ l_randIndex ];
		}
		else
		{
			//look for not used segment
			int l_found = l_randIndex;
			do
			{
				l_found =( l_found + 1 ) % l_source.Length;
				if( !l_usage[ l_found ] )
				{
					l_usage[ l_found ] = true;
					return l_source[ l_found ];
				}
			}while( l_found != l_randIndex );

			//if all used, then reset
			l_usage.SetAll( false );
			l_usage[ l_randIndex ] = true;
			return l_source[ l_randIndex ];
		}
//		return null;
	}

	private BitArray GetUsage( EDifficultyLevel _difficulty )
	{
		switch( _difficulty )
		{
		case EDifficultyLevel.VeryEasy 	: return m_veryEasyUsed;
		case EDifficultyLevel.Easy 		: return m_easyUsed;
		case EDifficultyLevel.Medium 	: return m_meduimUsed;
		case EDifficultyLevel.Hard 		: return m_hardUsed;
		case EDifficultyLevel.VeryHard 	: return m_veryHardUsed;
		}
		
		return null;
	}

	private EnvSegment[] GetArray( EDifficultyLevel _difficulty )
	{
		switch( _difficulty )
		{
		case EDifficultyLevel.VeryEasy 	: return m_veryEasy;
		case EDifficultyLevel.Easy 		: return m_easy;
		case EDifficultyLevel.Medium 	: return m_medium;
		case EDifficultyLevel.Hard 		: return m_hard;
		case EDifficultyLevel.VeryHard 	: return m_veryHard;
		}

		return null;
	}

	public EnvSegment EmptySegment
	{ 
		get{ return m_empty; }
	}

}
