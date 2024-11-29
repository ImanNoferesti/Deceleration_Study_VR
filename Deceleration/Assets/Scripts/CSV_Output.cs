using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;
using System.Linq;


public class CSV_Output : MonoBehaviour
{
    string myFilePath;
    string path;
	string subID;
    int age;
    string gender;
    string date;
    int counter;
    int duplicate =1;
    string suffix = null;

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
        myFilePath = Application.dataPath + "/Data/CSV_Format/" + subID + "_Decel_" + suffix + ".csv";

		while(File.Exists(myFilePath))
        {
            myFilePath = Application.dataPath + "/Data/CSV_Format/" + subID + "_Decel_" + suffix + "_" + duplicate + ".csv";
            duplicate++;
        }  
        

        WriteToFile("Date" + "," + "Condition" + "," + "Participant ID" + "," + "Age" + "," + "Gender" + "," + "Trial" + "," + "Time" + "," + "Head_X_Pos" + "," + "Head_Y_Pos" + "," 
        + "Head_Z_Pos" + "," + "Head_X_Rot" + "," + "Head_Y_Rot" + "," + "Head_Z_Rot" + "," + "CarCreation" + "," + "CarDecelStart" + "," + "Response" + "," + "CarDestruction" + "," + "Car_X_Pos" + "," +
        "Car_Y_Pos" + "," + "Car_Z_Pos" + "," + "CarAcceleration" + "," + "CarVelocity" + "," +  "CarDistance" + "," + "VisualAngle" + "," + "RCVA" + "," + "Tau" + "," +
        "TauDot" + "," + "VelocityVector" + "," + "DistanceVector" + ","  + "VisualAngle2" + "," + "RCVA2" + "," + "Tau2\n");
	}

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnApplicationQuit()
    {
        counter = headSet.HeadXPos.Count;
        StringBuilder stringbuilder = new StringBuilder();
        for (int i = 0; i < counter; i++)
        {
            stringbuilder.Append(date + "," + setup.condition.ToString() + "," + subID + "," + age + "," + gender + "," + carInfo.trialNumber[i].ToString("F4") + "," + headSet.TotalTime[i].ToString("F4") 
                + "," + headSet.HeadXPos[i].ToString("F4") + "," + headSet.HeadYPos[i].ToString("F4") + "," + headSet.HeadZPos[i].ToString("F4") 
                + "," + headSet.HeadXRot[i].ToString("F4") + "," + headSet.HeadYRot[i].ToString("F4") + "," + headSet.HeadZRot[i].ToString("F4") 
                + "," + carInfo.carCreation[i].ToString("F4") + "," + carInfo.carDecelStart[i].ToString("F4") + "," + carInfo.response[i].ToString("F4")
                + "," + carInfo.carDestruction[i].ToString("F4") + "," + carInfo.carXPos[i].ToString("F4") + "," + carInfo.carYPos[i].ToString("F4") + "," + carInfo.carZPos[i].ToString("F4")
                + "," + carInfo.carAcceleration[i].ToString("F4") + "," + carInfo.carVelocity[i].ToString("F4")
                + "," + carInfo.carDistance[i].ToString("F4") + "," + carInfo.visualAngle[i].ToString("F4") + "," + carInfo.RCVA[i].ToString("F4") 
                + "," + carInfo.tau[i].ToString("F4") + "," + carInfo.tauDot1[i].ToString("F4") + "," + carInfo.velocityVector[i].ToString("F4") 
                + "," + carInfo.distanceVector[i].ToString("F4") + "," + carInfo.visualAngle2[i].ToString("F4")
                + "," + carInfo.RCVA2[i].ToString("F4") + "," + carInfo.tau2[i].ToString("F4") +"\n");
        }
        path = myFilePath;
        File.AppendAllText(path, stringbuilder.ToString());
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
