//
// Copyright (C) 2014 CyberGlove Systems LLC.
// All Rights Reserved
//
// CyberGlove Unity Plugin
// Version 1.0
// Sidhant Sharma -> ssharma@cyberglovesystems.com



using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;


/// <summary>
/// Handedness of connected gloves.
/// </summary>
public enum CyberGloveHandedness
{
	RIGHT = 0,
	LEFT = 1,
	UNKNOWN = -1,
}

/// <summary>
/// G4 sensor number corresponding to handedness of connected glove
/// </summary>
public enum AttachedG4SensorNo
{
	RIGHT = 0,
	LEFT = 1,
	UNKNOWN = -1,
}


public class CyberGloveInput : MonoBehaviour {

	/// <summary>
	/// Glove objects provide access to glove data.
	/// </summary>
	/// 

	//public GameController gameController; 

	public class CyberGlove
	{
		/// <summary>
		/// The glove connected state.
		/// </summary>
		public bool Connected { get { return m_Connected; } }

		/// <summary>
		/// The G4 tracker connected state.
		/// </summary>
		public bool G4Connected { get { return m_G4Connected; } }

		/// <summary>
		/// Handedness of the glove object.
		/// </summary>
		public CyberGloveHandedness Handedness { get { return m_Handedness ; } }

		/// <summary>
		/// Attached G4 sensor number
		/// </summary>
		public AttachedG4SensorNo G4sensorNo { get { return m_G4sensorNo ; } }
		
		/// <summary>
		/// The palm position in Unity coordinates.
		/// </summary>
		public Vector3 Position { get { return new Vector3( m_PalmPosition.x, m_PalmPosition.y, m_PalmPosition.z ); } }
		
		/// <summary>
		/// The tracker/palm rotation in Unity coordinates.
		/// </summary>
		public Quaternion Rotation { get { return new Quaternion( m_PalmRotation.x, m_PalmRotation.y, m_PalmRotation.z, m_PalmRotation.w ); } }

		// <summary>
		/// The finger/joint position in Unity coordinates.
		/// </summary>
		public Vector3[,] m_fingerJointPos = new Vector3[5,4];

		// <summary>
		/// The finger/joint rotation in Unity coordinates.
		/// </summary>
		public Quaternion[,] m_fingerJointRot_quat = new Quaternion[5,4];


		internal CyberGlove()
		{
			m_Connected = false;
			m_G4Connected = false;
			m_Handedness = CyberGloveHandedness.UNKNOWN;
			m_G4sensorNo = AttachedG4SensorNo.UNKNOWN;
			m_PalmPosition.Set( 0.0f, 0.0f, 0.0f );
			m_PalmRotation.Set( 0.0f, 0.0f, 0.0f, 1.0f );
			G4Cp.Set (0.0f,0.0f,0.0f);
			G4Cori.Set(0.0f,0.0f,0.0f);
			G4HomeSet = false;


			for(int finger=0;finger<5;finger++) //5 fingers: Thumb, Index, Middle, Ring, Pinky
			{ 
				for(int joint=0;joint<4;joint++)//4 joints: Inner, Middle, Outer, FingerTip
				{   
					m_fingerJointPos[finger,joint].Set(0.0f,0.0f,0.0f);
					m_fingerJointRot_quat[finger,joint].Set(0.0f,0.0f,0.0f,1.0f);
				}
			}
		}

		// <summary>
		/// Glove connection state
		/// </summary>
		internal void SetEnabled( bool connected )
		{
			m_Connected = connected;
		}

		// <summary>
		/// G4 connection state
		/// </summary>
		internal void SetG4Enabled( bool G4connected )
		{
			m_G4Connected = G4connected;
		}

		// <summary>
		/// Actuate Finger/Palm Motors
		/// </summary>
		public bool SetActuatorMotor(int hand,int finger,double amplitude)
		{
			bool result = CyberGloveUnityPlugin.SetActuator (hand, finger, amplitude);
			return result;
		}

		// <summary>
		/// Stop Active Finger/Palm Motor after 1 second
		/// </summary>
		public bool StopActuatorMotor(int hand,int finger)
		{
			bool result = CyberGloveUnityPlugin.StopActuator (hand, finger);
			return result;
		}

		// <summary>
		/// Update latest glove data
		/// </summary>
		internal void Update( ref CyberGloveUnityPlugin.CyberGloveData cd )
		{
			m_Handedness = ( CyberGloveHandedness )cd.which_hand;
			m_PalmPosition.Set( cd.palmPos [0], cd.palmPos[1], cd.palmPos[2] );
			m_PalmRotation.Set( cd.palmRot_quat[0], cd.palmRot_quat[1], cd.palmRot_quat[2], cd.palmRot_quat[3] );


			for(int finger=0;finger<5;finger++) 
			{ 
				for(int joint=0;joint<4;joint++)
				{   

					m_fingerJointPos[finger,joint].Set(cd.fingerJointPos[finger,joint].x,cd.fingerJointPos[finger,joint].y,cd.fingerJointPos[finger,joint].z);
					m_fingerJointRot_quat[finger,joint].Set(cd.fingerJointRot_quat[finger,joint].x,cd.fingerJointRot_quat[finger,joint].y,cd.fingerJointRot_quat[finger,joint].z,cd.fingerJointRot_quat[finger,joint].w);

				}
			}
		}

		// <summary>
		/// Update latest G4 sensor data
		/// </summary>
		internal void G4Update( ref CyberGloveUnityPlugin.G4Data gd )
		{
			m_G4sensorNo = ( AttachedG4SensorNo )gd.sensor_No;

			if (G4HomeSet == false) 
			{

				if(Mathf.Abs(gd.G4Pos.x)<=0.01f&&Mathf.Abs(gd.G4Pos.y)<=0.01f&&Mathf.Abs(gd.G4Pos.z)<=0.10f)
				{
					
				}
				else
				{
					G4Cp.Set (gd.G4Pos.x,gd.G4Pos.y,gd.G4Pos.z);
					G4Cori.Set(gd.G4Rot.x,gd.G4Rot.y,gd.G4Rot.z); //yaw pitch roll order reported by G4
					G4HomeSet = true;
				}
						
			} 
			else 
			{
				Vector3 g4position = new Vector3(gd.G4Pos.x,gd.G4Pos.y,gd.G4Pos.z);
				Vector3 g4rotation = new Vector3(gd.G4Rot.x,gd.G4Rot.y,gd.G4Rot.z);
				
				if(Mathf.Abs(G4Cp.x)<=0.01f&&Mathf.Abs(G4Cp.y)<=0.01f&&Mathf.Abs(G4Cp.z)<=0.01f)
				{
					G4Cp = g4position;
					G4Cori = g4rotation;
				}
				else
				{   Vector3 G4trans = g4position - G4Cp;
					m_PalmPosition.Set(	G4trans.x,G4trans.y,G4trans.z);	

					//Vector3 rotation = new Vector3(gd.G4Rot.x,gd.G4Rot.y,gd.G4Rot.z); //x:yaw y:pitch z:roll order reported by G4
					Vector3 rotation = new Vector3(gd.G4Rot.x,0.0f,gd.G4Rot.z);
					Vector3 G4ori = G4Cori - rotation;

					Quaternion G4rot_quat = new Quaternion(0.0f,0.0f,0.0f,1.0f);
					G4rot_quat = EulerToQuat(-G4ori.y,-(pi-G4ori.z),G4ori.x);
					m_PalmRotation = G4rot_quat*m_PalmRotation;
				}


			}
		}


		private Quaternion EulerToQuat(float roll, float pitch, float yaw)
		{
			float cr, cp, cy, sr, sp, sy, cpcy, spsy;
			// calculate trig identities
			cr = Mathf.Cos(roll/2);
			cp = Mathf.Cos(pitch/2);
			cy = Mathf.Cos(yaw/2);
			sr = Mathf.Sin(roll/2);
			sp = Mathf.Sin(pitch/2);
			sy = Mathf.Sin(yaw/2);
			cpcy = cp * cy;
			spsy = sp * sy;

			Quaternion quat = new Quaternion (0.0f, 0.0f, 0.0f, 1.0f);
			quat.w = cr * cpcy + sr * spsy;
			quat.x = sr * cpcy - cr * spsy;
			quat.y = cr * sp * cy + sr * cp * sy;
			quat.z = cr * cp * sy - sr * sp * cy;

			return quat;
		}

		private bool m_Connected;
		private CyberGloveHandedness m_Handedness;
		private Vector3 m_PalmPosition;
		private Quaternion m_PalmRotation;
		private bool m_G4Connected;
		private AttachedG4SensorNo m_G4sensorNo;
		private Vector3 G4Cp;
		private Vector3 G4Cori;
		private bool G4HomeSet;

		private const float pi = 22/7;
	}


	/// <summary>
	/// Max number of gloves and G4 sensors allowed by plugin.
	/// </summary>
	public const uint MAX_GLOVES = 2;
		
	/// <summary>
	/// Access to CyberGlove objects.
	/// </summary>
	public static CyberGlove[] Gloves { get { return m_Gloves; } }
	
	/// <summary>
	/// Gets the glove object bound to the specified hand.
	/// </summary>
	public static CyberGlove GetGlove( CyberGloveHandedness hand )
	{
		for ( int i = 0; i < MAX_GLOVES; i++ )
		{
			if ( ( m_Gloves[i] != null ) && ( m_Gloves[i].Handedness == hand ) )
			{
				return m_Gloves[i];
			}
		}
		
		return null;
	}

		
	private static CyberGlove[] m_Gloves = new CyberGlove[MAX_GLOVES];

	/// <summary>
	/// Right Glove connection state
	/// </summary>
	private static bool RGConn;

	/// <summary>
	/// Left Glove connection state
	/// </summary>
	private static bool LGConn;

	/// <summary>
	/// G4 connection state
	/// </summary>
	private static bool G4Conn;
	
	/// <summary>
	/// Initialize the cybergloves, check connection in DCU and allocate.
	/// </summary>
	void Start()
	{
		//ClearConsole ();

		for (int i = 0; i < MAX_GLOVES; i++) 
		{m_Gloves [i] = new CyberGlove ();
		}
        RGConn = CyberGloveUnityPlugin.InitRightGrasp();
	    Debug.Log (RGConn);

		LGConn = CyberGloveUnityPlugin.InitLeftGrasp();
		Debug.Log (LGConn);

		if (!RGConn && !LGConn) 
		{
			Debug.Log ("DCU is not running or gloves not connected.\nIf connected check naming convention - RightGrasp and LeftGrasp. Devices must be added as cyberglove devices in DCU.");
		}
		else if(!RGConn && LGConn)
		{
			Debug.Log ("RightGrasp not connected.\nIf connected check naming convention - RightGrasp. Device must be added as a cyberglove device in DCU.");
		}
		else if(RGConn && !LGConn)
		{
			Debug.Log ("LeftGrasp not connected.\nIf connected check naming convention - LeftGrasp. Device must be added as a cyberglove device in DCU.");
		}
		else
		{
			Debug.Log ("Nice!!\nPress Space to move hands down");

		}

	}
	
	/// <summary>
	/// Update the glove data once per frame.
	/// </summary>
	void Update()
	{
		// update glove data
		uint numGlovesEnabled = 0;
		CyberGloveUnityPlugin.CyberGloveData cd = new CyberGloveUnityPlugin.CyberGloveData ();
		cd.palmPos = new Vector3 (0.0f, 0.0f, 0.0f);
		cd.palmRot_quat = new Quaternion (0.0f, 0.0f, 0.0f, 1.0f);

		for ( int i = 0; i < MAX_GLOVES; i++ )
		{
			if ( m_Gloves[i] != null )
			{
				if(i==0) // GLOVE 0 IS TAKEN AS RIGHT GLOVE
				{
					if ( RGConn==true )
				
					{
						CyberGloveUnityPlugin.UpdateRightGrasp();
						float[] palmPos_arr = {0.0f,0.0f,0.0f};
						float[] palmRot_arr = {0.0f,0.0f,0.0f,1.0f};

						CyberGloveUnityPlugin.GetPalmsPositionRT (palmPos_arr);
						cd.palmPos.Set(palmPos_arr[0],palmPos_arr[1],palmPos_arr[2]);

						cd.which_hand = (int)CyberGloveHandedness.RIGHT;

						CyberGloveUnityPlugin.GetPalmsRotationRT (palmRot_arr);
						cd.palmRot_quat.Set(palmRot_arr[0],palmRot_arr[1],palmRot_arr[2],palmRot_arr[3]);


						for(int finger=0;finger<5;finger++) 
						{ 
							for(int joint=0;joint<4;joint++)
							{   
								float[] fingerJointPos_arr = {0.0f,0.0f,0.0f};
								float[] fingerJointRot_arr = {0.0f,0.0f,0.0f,1.0f};

								CyberGloveUnityPlugin.GetFingersPositionRT (finger,joint,fingerJointPos_arr);
								cd.fingerJointPos[finger,joint].Set(fingerJointPos_arr[0],fingerJointPos_arr[1],fingerJointPos_arr[2]);

								CyberGloveUnityPlugin.GetFingersRotationRT (finger,joint,fingerJointRot_arr);
								cd.fingerJointRot_quat[finger,joint].Set(fingerJointRot_arr[0],fingerJointRot_arr[1],fingerJointRot_arr[2],fingerJointRot_arr[3]);
							}
						}
						m_Gloves[i].Update( ref cd );
						m_Gloves[i].SetEnabled( true );
						numGlovesEnabled++;
				

					}
					else
					{
						m_Gloves[i].SetEnabled( false );
					}
				}
				else if(i==1) // GLOVE 1 IS TAKEN AS LEFT GLOVE
				{
					if ( LGConn==true )
						
					{
						CyberGloveUnityPlugin.UpdateLeftGrasp();
						float[] palmPos_arr = {0.0f,0.0f,0.0f};
						float[] palmRot_arr = {0.0f,0.0f,0.0f,1.0f};
						
						CyberGloveUnityPlugin.GetPalmsPositionLT (palmPos_arr);
						cd.palmPos.Set(palmPos_arr[0],palmPos_arr[1],palmPos_arr[2]);
						
						cd.which_hand = (int)CyberGloveHandedness.LEFT;
						
						CyberGloveUnityPlugin.GetPalmsRotationLT (palmRot_arr);
						cd.palmRot_quat.Set(palmRot_arr[0],palmRot_arr[1],palmRot_arr[2],palmRot_arr[3]);
						
						
						for(int finger=0;finger<5;finger++) 
						{ 
							for(int joint=0;joint<4;joint++)
							{   
								float[] fingerJointPos_arr = {0.0f,0.0f,0.0f};
								float[] fingerJointRot_arr = {0.0f,0.0f,0.0f,1.0f};
								
								CyberGloveUnityPlugin.GetFingersPositionLT (finger,joint,fingerJointPos_arr);
								cd.fingerJointPos[finger,joint].Set(fingerJointPos_arr[0],fingerJointPos_arr[1],fingerJointPos_arr[2]);
								
								CyberGloveUnityPlugin.GetFingersRotationLT (finger,joint,fingerJointRot_arr);
								cd.fingerJointRot_quat[finger,joint].Set(fingerJointRot_arr[0],fingerJointRot_arr[1],fingerJointRot_arr[2],fingerJointRot_arr[3]);
							}
						}
						m_Gloves[i].Update( ref cd );
						m_Gloves[i].SetEnabled( true );
						numGlovesEnabled++;
						
						
					}
					else
					{
						m_Gloves[i].SetEnabled( false );
					}
				}
			}
		}


	}
	
   
    


	/// <summary>
	/// Updates the GUI.
	/// </summary>
	void OnGUI()
	{

	}
	
	/// <summary>
	/// Close G4 when application quits.
	/// </summary>
	void OnApplicationQuit()
	{

		if ((RGConn==true)||(LGConn==true)) 
		{
			CyberGloveUnityPlugin.Fini();
		}
		
	}

	static void ClearConsole () 
	{
		// This simply does "LogEntries.Clear()" the long way:
		var logEntries = System.Type.GetType("UnityEditorInternal.LogEntries,UnityEditor.dll");
		var clearMethod = logEntries.GetMethod("Clear", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
		clearMethod.Invoke(null,null);
	}


}
