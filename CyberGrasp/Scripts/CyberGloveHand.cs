// Copyright (C) 2014 CyberGlove Systems LLC.
//

using UnityEngine;
using System.Collections;

public class CyberGloveHand : MonoBehaviour {

	public CyberGloveHandedness	m_hand;
	public CyberGloveInput.CyberGlove m_glove = null;

	private double amplitude = 0.2;

	Vector3		m_initialPosition; // (Inmo) These two variable just seem to be for memorising initial positions/rotations of the hands. They do not seem to be used in fact. 
	Quaternion 	m_initialRotation;
	

	private bool[] isMotorActive = new bool[6];
	private float BuzzPeriod;
	private float[] timer = new float[6];

	public Vector3 CenterPosition = new Vector3(0,24,-44);

	//public GameObject Active_Sphere;


	protected void Start() 
	{
		m_initialRotation = transform.rotation;
		m_initialPosition = transform.position;

		BuzzPeriod = 0.1f;
		for(int i=0;i<6;i++)
		{
			isMotorActive [i] = false;
			timer[i] = BuzzPeriod;
		}


	}
	
	
	protected void Update()
	{
		if ( m_glove == null )
		{
			m_glove = CyberGloveInput.GetGlove(m_hand);
		}

		UpdateMotorsState ();

	}
	

	public bool ActivateFingerMotor(int finger)
	{

		bool result = false;

		if (m_glove!= null) 
		{
			result = m_glove.SetActuatorMotor ((int)m_hand, finger, amplitude);
			isMotorActive[finger] = true;

		}
		return result;

	}

    public bool ActivateFingerMotorDynamic(int finger, float amplitude)
    {

        bool result = false;

        if (m_glove != null)
        {
            result = m_glove.SetActuatorMotor((int)m_hand, finger, amplitude);
            isMotorActive[finger] = true;

        }
        return result;

    }

    public bool StopFingerMotor(int finger)
	{
		bool result = false;

		if(m_glove!=null)
		{
			result = m_glove.StopActuatorMotor ((int)m_hand, finger);

		}
		return result;
	}

	public Quaternion InitialRotation
	{
		get { return m_initialRotation; }
	}
	
	public Vector3 InitialPosition
	{
		get { return m_initialPosition; }
	}

	public Vector3 HandStartCenterPosition
	{
		get { return CenterPosition; }
	}

	protected void UpdateMotorsState() // Generate force feedback to each finger
	{
		for(int i=0;i<6;i++)
		{
			if(isMotorActive[i]) 
			{
				timer[i]-=Time.deltaTime;
				if (timer[i]<0.0f)
				{
					
					isMotorActive[i] = false;
					timer[i] = BuzzPeriod;
					StopFingerMotor(i);
				}
                // (Inmo) These lines make sure that force feedback should be provided within BuzzPeriod, otherwise just ignore to give the feedback. 
			}
		}
	}

	public bool GetMotorState(int finger)
	{
		return isMotorActive [finger];
	}

	public void CenterHand()
	{
		transform.position = CenterPosition;
	}

	public bool IsHandActive()
	{
		return ( m_glove != null && m_glove.Connected ); 

	}



}
