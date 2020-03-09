//
// Copyright (C) 2014 CyberGlove Systems LLC.
// All Rights Reserved
//
// CyberGlove Unity Plugin
// Version 1.0
// Sidhant Sharma -> ssharma@cyberglovesystems.com


using UnityEngine;
using System.Collections;

public class CyberGloveHandsController : MonoBehaviour {

	CyberGloveHand[] 	m_hands;

	/// <summary>
	/// Workspace Factor for G4 movement
	/// </summary>
	public float WorkspaceFactor;
	public float G4WorkspaceFactor;

	//private GameController gameController;


	/// <summary>
	/// Right Hand Finger/Joint Tags
	/// </summary>
	string[,] RH_fingerJointTag = new string[5,3]
	{{"RH_ThumbRoll","RH_ThumbIJ","RH_ThumbOJ"},
		{"RH_IndexIJ","RH_IndexMJ","RH_IndexOJ"},
		{"RH_MiddleIJ","RH_MiddleMJ","RH_MiddleOJ"},
		{"RH_RingIJ","RH_RingMJ","RH_RingOJ"},
		{"RH_PinkyIJ","RH_PinkyMJ","RH_PinkyOJ"}};
	
	/// <summary>
	/// Right Hand Finger/Joint GameObject Array
	/// </summary>
	public GameObject[,] RH_FingerJointGameObject = new GameObject[5,3];

	/// <summary>
	/// Right Hand Finger/Joint Initial Orientation Array
	/// </summary>
	Quaternion[,] RH_FingerJointInitial = new Quaternion[5, 3];
	

	///// <summary>
	///// Left Hand Finger/Joint Tags
	///// </summary>
	//string[,] LH_fingerJointTag = new string[5,3]
	//{{"LH_ThumbRoll","LH_ThumbIJ","LH_ThumbOJ"},
	//	{"LH_IndexIJ","LH_IndexMJ","LH_IndexOJ"},
	//	{"LH_MiddleIJ","LH_MiddleMJ","LH_MiddleOJ"},
	//	{"LH_RingIJ","LH_RingMJ","LH_RingOJ"},
	//	{"LH_PinkyIJ","LH_PinkyMJ","LH_PinkyOJ"}};
	
	///// <summary>
	///// Left Hand Finger/Joint GameObject Array
	///// </summary>
	//GameObject[,] LH_FingerJointGameObject = new GameObject[5,3];

	///// <summary>
	///// Left Hand Finger/Joint Initial Orientation Array
	///// </summary>
	//Quaternion[,] LH_FingerJointInitial = new Quaternion[5, 3];


	// Use this for initialization
	void Start () 
	{

		//GameObject gamecontrollerobject = GameObject.FindWithTag("GameController");
		//if (gamecontrollerobject != null) 
		//{gameController = gamecontrollerobject.GetComponent<GameController>();
		//}
		//if (gameController == null) 
		//{Debug.Log ("Cannot find 'GameController' script");
		//}

		m_hands = GetComponentsInChildren<CyberGloveHand>();

		for (int finger =0; finger<5; finger++) 
		{
		   for (int joint=0; joint<3; joint++) 
			{
				//Right Hand Initialiazation 
				GameObject tempRight = GameObject.FindWithTag(RH_fingerJointTag[finger,joint]);
				if (tempRight != null) 
				{   RH_FingerJointGameObject[finger,joint] = tempRight;
					RH_FingerJointInitial[finger,joint] = tempRight.transform.rotation;
				}

				//Left Hand Initialiazation
				//GameObject tempLeft = GameObject.FindWithTag(LH_fingerJointTag[finger,joint]);
				//if (tempLeft != null) 
				//{   LH_FingerJointGameObject[finger,joint] = tempLeft;
				//	LH_FingerJointInitial[finger,joint] = tempLeft.transform.rotation;
				//}
			}
		}


	}
	
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		m_hands = GetComponentsInChildren<CyberGloveHand>();

		foreach ( CyberGloveHand hand in m_hands )
		{


			//if ( IsGloveActive( hand.m_glove ) )
			//{
			//		hand.Active_Sphere.GetComponent<Renderer>().material.color = Color.green;
			//}
			//else
			//{
			//		hand.Active_Sphere.GetComponent<Renderer>().material.color = Color.red;
			//}


			if ( IsGloveActive( hand.m_glove ) )
			{
				UpdateHand( hand );
			}

		}
		

	}
	
	/// <summary>
	/// Applies palm translation and rotation to the hand model
	/// </summary>
	void UpdateHand( CyberGloveHand hand )
	{
		bool bControllerActive = IsGloveActive( hand.m_glove );
		
		if ( bControllerActive )
		{

			//float moveHorizontal = 0.0f;
			//float moveVertical = 0.0f;
			//float moveHeight = 0.0f;

			//Vector3 handposition;
			//Quaternion handrotation;
			
			//if(!gameController.IsG4Active())
			//{
			//	moveHorizontal = Input.GetAxis ("Horizontal");
			//	moveVertical =	Input.GetAxis("Vertical");

			//	if (Input.GetKey (KeyCode.Space))
			//	{
			//		moveHeight = hand.transform.localPosition.z+0.2f;
			//	}
			//	else
			//	{
			//		moveHeight = hand.transform.localPosition.z-0.2f;
			//	}

			//	Vector3 movement = new Vector3 (moveHorizontal, moveVertical, 0.0f);
			//	handposition = hand.transform.localPosition + 10*movement*WorkspaceFactor*Time.deltaTime;

			//	hand.transform.localPosition = new Vector3 
			//		(Mathf.Clamp (handposition.x, -22.0f, 22.0f),
			//		 Mathf.Clamp (handposition.y, 23.0f, 27.0f),
			//		 Mathf.Clamp (moveHeight, hand.CenterPosition.z, hand.CenterPosition.z+2.0f));


			//	handrotation = hand.m_glove.Rotation;
			//	handrotation.eulerAngles = new Vector3(-handrotation.eulerAngles.x,handrotation.eulerAngles.y,-handrotation.eulerAngles.z);
			//	hand.transform.rotation = handrotation * Quaternion.Euler(180,180,0)*hand.InitialRotation;
			//}

			//else if(gameController.IsG4Active())
			//{
			//	// IF POLHEMUS G4 IS TO BE USED
			//	Vector3 movement = new Vector3 (hand.m_glove.Position.x, -hand.m_glove.Position.y, hand.m_glove.Position.z);
			//	//handposition = hand.InitialPosition+ WorkspaceFactor*movement;
			//	handposition = hand.HandStartCenterPosition + G4WorkspaceFactor*movement;

			//	hand.transform.localPosition = new Vector3 
			//		(Mathf.Clamp (handposition.x, -22.0f, 22.0f),
			//		 Mathf.Clamp (handposition.y, 15.0f, 29.0f),
			//		 Mathf.Clamp (handposition.z, -12.0f, -2.0f));
			//	//

			//	hand.transform.rotation = hand.m_glove.Rotation * Quaternion.Euler(180,0,0)*hand.InitialRotation;
			//}




			UpdateFingers(hand);

		}
		
		else
		{
			// use the inital position and orientation because the controller is not active
			hand.transform.position = hand.InitialPosition;
			hand.transform.rotation  = hand.InitialRotation;
		}
	}
	

	/// <summary>
	/// Applies finger/joint rotations to the hand model
	/// </summary>
	void UpdateFingers(CyberGloveHand hand)
	{
		if(hand.m_hand == CyberGloveHandedness.RIGHT)
		{
			for (int finger =0; finger<5; finger++) 
			{
				for (int joint=0; joint<3; joint++) 
				{   
					float thumbangle=0; //adjustment for thumb model
					if(finger==0){thumbangle = 20;}

					RH_FingerJointGameObject[finger,joint].transform.rotation = (hand.transform.rotation)*hand.m_glove.m_fingerJointRot_quat[finger,joint]*Quaternion.Euler(0,0,thumbangle)*RH_FingerJointInitial[finger,joint];


				}
			}
		}
		//else if (hand.m_hand == CyberGloveHandedness.LEFT)
		//{
		//	for (int finger =0; finger<5; finger++) 
		//	{
		//		for (int joint=0; joint<3; joint++) 
		//		{   
		//			float thumbangle=0; //adjustment for thumb model
		//			if(finger==0){thumbangle = 0;}
					
		//			LH_FingerJointGameObject[finger,joint].transform.rotation = (hand.transform.rotation)*hand.m_glove.m_fingerJointRot_quat[finger,joint]*Quaternion.Euler(0,0,thumbangle)*LH_FingerJointInitial[finger,joint];
					
		//		}
		//	}
		//}
	}

	/// <summary>
	/// Returns true if the glove is active
	/// </summary>
	bool IsGloveActive( CyberGloveInput.CyberGlove glove )
	{
		return ( glove != null && glove.Connected );
	}

    public GameObject[,] Get_RH_FingerJointGameObject()
    {
        return this.RH_FingerJointGameObject;
    }
}
