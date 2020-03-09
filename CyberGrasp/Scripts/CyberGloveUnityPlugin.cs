//
// Copyright (C) 2014 CyberGlove Systems LLC.
// All Rights Reserved
//
// CyberGlove Unity Plugin
// Version 1.0
// Sidhant Sharma -> ssharma@cyberglovesystems.com


using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;

public partial class CyberGloveUnityPlugin {
	//Lets make our calls from the Plugins
	
	public class CyberGloveData
	{
		public Vector3 palmPos;
		public Quaternion palmRot_quat;

		public Vector3[,] fingerJointPos = new Vector3[5,4];
		public Quaternion[,] fingerJointRot_quat = new Quaternion[5,4];

		public bool connected;
		public int which_hand;

	}

	public class G4Data
	{
		public Vector3 G4Pos = new Vector3(-1.0f,-1.0f,-1.0f);
		public Vector3 G4Rot = new Vector3 (0.0f, 0.0f, 0.0f);
		
		public bool G4connected;
		public int sensor_No;
		
	}
	
	//RIGHT CYBERGLOVE FUNCTIONS *******************************************************************************
	[DllImport ("VHTProxy")]
	public static extern bool InitRightGlove();

	[DllImport ("VHTProxy")]
	public static extern bool GetPalmsPositionRG(float[] palmPos);

	[DllImport ("VHTProxy")]
	public static extern bool GetPalmsRotationRG(float[] palmRot);

	[DllImport ("VHTProxy")]
	public static extern bool GetFingersPositionRG(int finger, int phalanx, float[] fingerPos);
	
	[DllImport ("VHTProxy")]
	public static extern bool GetFingersRotationRG(int finger, int phalanx, float[] fingerRot);

	[DllImport ("VHTProxy")]
	public static extern bool UpdateRightGlove();


	//LEFT CYBERGLOVE FUNCTIONS *******************************************************************************
	[DllImport ("VHTProxy")]
	public static extern bool InitLeftGlove();
	
	[DllImport ("VHTProxy")]
	public static extern bool GetPalmsPositionLG(float[] palmPos);
	
	[DllImport ("VHTProxy")]
	public static extern bool GetPalmsRotationLG(float[] palmRot);
	
	[DllImport ("VHTProxy")]
	public static extern bool GetFingersPositionLG(int finger, int phalanx, float[] fingerPos);
	
	[DllImport ("VHTProxy")]
	public static extern bool GetFingersRotationLG(int finger, int phalanx, float[] fingerRot);
	
	[DllImport ("VHTProxy")]
	public static extern bool UpdateLeftGlove();




	//RIGHT CYBER GRASP *******************************************************************************
	[DllImport ("VHTProxy_CyberGrasp")]
	public static extern bool InitRightGrasp();
	
	[DllImport ("VHTProxy_CyberGrasp")]
	public static extern bool GetPalmsPositionRT(float[] palmPos);
	
	[DllImport ("VHTProxy_CyberGrasp")]
	public static extern bool GetPalmsRotationRT(float[] palmRot);
	
	[DllImport ("VHTProxy_CyberGrasp")]
	public static extern bool GetFingersPositionRT(int finger, int phalanx, float[] fingerPos);
	
	[DllImport ("VHTProxy_CyberGrasp")]
	public static extern bool GetFingersRotationRT(int finger, int phalanx, float[] fingerRot);
	
	[DllImport ("VHTProxy_CyberGrasp")]
	public static extern bool UpdateRightGrasp();

	

	
	//LEFT CYBER GRASP *******************************************************************************
	[DllImport ("VHTProxy_CyberGrasp")]
	public static extern bool InitLeftGrasp();
	
	[DllImport ("VHTProxy_CyberGrasp")]
	public static extern bool GetPalmsPositionLT(float[] palmPos);
	
	[DllImport ("VHTProxy_CyberGrasp")]
	public static extern bool GetPalmsRotationLT(float[] palmRot);
	
	[DllImport ("VHTProxy_CyberGrasp")]
	public static extern bool GetFingersPositionLT(int finger, int phalanx, float[] fingerPos);
	
	[DllImport ("VHTProxy_CyberGrasp")]
	public static extern bool GetFingersRotationLT(int finger, int phalanx, float[] fingerRot);
	
	[DllImport ("VHTProxy_CyberGrasp")]
	public static extern bool UpdateLeftGrasp();



	//CYBERGRASP COMMON ********************************************************************************************
	[DllImport ("VHTProxy_CyberGrasp")]
	public static extern void Fini();

	[DllImport ("VHTProxy_CyberGrasp")]
	public static extern bool SetActuator(int hand, int finger, double amplitude);
	
	[DllImport ("VHTProxy_CyberGrasp")]
	public static extern bool StopActuator(int hand, int finger);



}

