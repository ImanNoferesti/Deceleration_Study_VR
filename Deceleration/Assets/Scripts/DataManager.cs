using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System;
using System.Text;


public class DataManager : MonoBehaviour {

	string myFilePath;
	string subID;
    int age;
    string gender;
    string date;
    int counter;
    int duplicate=1;

    HeadSet headSet;
    Deceleration_V1 carInfo;
    StudySetup setup;
    [SerializeField] GameObject head;
    [SerializeField]GameObject car;
	StreamWriter fileWriter;


    void Awake()
    {
        headSet = head.GetComponent<HeadSet>();
        carInfo = car.GetComponent<Deceleration_V1>();
        setup = GameObject.Find("Study Setup").GetComponent<StudySetup>();
    }

	private void Start()
	{
        
        subID = setup.subID;
        age = setup.age;
        gender = setup.gender.ToString();
        date = DateTime.Now.ToString();

        string suffix = null;
        switch(setup.condition)
        {
            case StudySetup.ConditionType.Air:
                suffix = "A";
                break;
            case StudySetup.ConditionType.Ground:
                suffix = "G";
                break;
            case StudySetup.ConditionType.Road:
                suffix = "R";
                break;
        }

        //internal storage/android/data/yourApp/files/testFile.txt
        myFilePath = Application.dataPath + "/Data/Text_Format/" + subID + "_Decel_" + suffix + ".txt";

		while(File.Exists(myFilePath))
        {
            myFilePath = Application.dataPath + "/Data/Text_Format/" + subID + "_Decel_" + suffix + "_" + duplicate + ".txt";
            duplicate++;
        }              
                

        WriteToFile("Deceleration Project\n" + "\nDate: "
        + date + "\nCondition: " + setup.condition.ToString() +
        "\nParticipant ID: " + subID + "\nAge: " + age 
        + "\nGender: " + gender + "\n\n\nTime\tHead_X_Pos\tHead_Y_Pos"
        + "\tHead_Z_Pos\tHead_X_Rot\tHead_Y_Rot\tHead_Z_Rot\t"
        + "CarCreation\tCarDecelStart\tResponse\tCarDestruction\tCar_X_Pos\tCar_Y_Pos\tCar_Z_Pos\tCarAcceleration\tCarVelocity"
        + "\tCarDistance\tVisualAngle\tRCVA\tTau\tTauDot\tVelocityVector\tDistanceVector"
        +"\tVisualAngle2\tRCVA2\tTau2\n");
	}

    void Update()
    {

        counter = headSet.HeadXPos.Count;
        
    }

	
    
    private void OnApplicationQuit()
    {

        StringBuilder stringBuilder = new StringBuilder();
        for(int i=0; i < counter; i++)
        {
            stringBuilder.Append(headSet.TotalTime[i].ToString("000.0000") + "\t"
            + headSet.HeadXPos[i].ToString("000.0000") + "\t"
            + headSet.HeadYPos[i].ToString("000.0000") + "\t"
            + headSet.HeadZPos[i].ToString("000.0000") + "\t"
            + headSet.HeadXRot[i].ToString("000.0000") + "\t"
            + headSet.HeadYRot[i].ToString("000.0000") + "\t"
            + headSet.HeadZRot[i].ToString("000.0000") + "\t"
            + carInfo.carCreation[i].ToString("0") + "\t"
            + carInfo.carDecelStart[i].ToString("0") + "\t"
            + carInfo.response[i].ToString("0") + "\t"
            + carInfo.carDestruction[i].ToString("0") + "\t"
            + carInfo.carXPos[i].ToString("000.0000") + "\t"
            + carInfo.carYPos[i].ToString("000.0000") + "\t"
            + carInfo.carZPos[i].ToString("000.0000") + "\t"
            + carInfo.carAcceleration[i].ToString("000.0000") + "\t"
            + carInfo.carVelocity[i].ToString("000.0000") + "\t"
            + carInfo.carDistance[i].ToString("000.0000") + "\t"
            + carInfo.visualAngle[i].ToString("000.0000") + "\t"
            + carInfo.RCVA[i].ToString("000.0000") + "\t"
            + carInfo.tau[i].ToString("000.0000") + "\t"
            + carInfo.tauDot1[i].ToString("000.0000") + "\t"
            + carInfo.velocityVector[i].ToString("000.0000") + "\t"
            + carInfo.distanceVector[i].ToString("000.0000") + "\t"
            + carInfo.visualAngle2[i].ToString("000.0000") + "\t"
            + carInfo.RCVA2[i].ToString("000.0000") + "\t"
            + carInfo.tau2[i].ToString("000.0000") + "\n");
        }
        
        File.AppendAllText(myFilePath, stringBuilder.ToString());


    }
    
    
    
    
    
    public void WriteToFile(string message)
    {
        try
        {
            fileWriter = new StreamWriter(myFilePath, true);
            fileWriter.Write(message);
            fileWriter.Close();
        }
        catch(System.Exception e)
        {
            Debug.LogError("Cannot write into the file");
        }
    }


		
}
