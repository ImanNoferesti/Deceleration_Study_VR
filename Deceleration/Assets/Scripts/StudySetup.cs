using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StudySetup : MonoBehaviour
{

    public enum ConditionType
    {
        Air,
        Ground,
        Road
    }

    public enum Gender
    {
        Male,
        Female,
        Other
    }
    public string subID;
    public int age;
    //public string gender;
    public Gender gender;
    public ConditionType condition;

    void Awake() 
    {
        DontDestroyOnLoad(this.gameObject);
    }


    private void Start()
    {
        SceneManagement();
    }

    public void SceneManagement()
    {
        switch(condition)
        {
            case ConditionType.Air:
                SceneManager.LoadScene(1);
                break;

            case ConditionType.Ground:
                SceneManager.LoadScene(2);
                break;
            
            case ConditionType.Road:
                SceneManager.LoadScene(3);
                break;
        }

    }


}
