using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RobotStatusViewer : MonoBehaviour
{
    public GameObject[] robots;
    public Image[] images;
    public Text[] statusTexts;

    void Start()
    {
        Invoke("FindRobots",1f);
    }

    public void FindRobots()
    {
        robots = GameObject.FindGameObjectsWithTag("Robot");
        if(robots == null)
        {
            Invoke("FindRobots", 1f);
            return;
        }

        GameObject robotStatusGroup =  GameObject.Find("RobotStatusGroup");
        for(int i=0; i<robots.Length; i++)
        {
            robotStatusGroup.transform.GetChild(2*i ).gameObject.SetActive(true);
            robotStatusGroup.transform.GetChild(2 * i + 1).gameObject.SetActive(true);
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if(robots != null)
        {
            for(int i=0; i<robots.Length; i++)
            {
                Robot robot = robots[i].GetComponent<Robot>();
                statusTexts[i].text = robot.name +  "       " + robot.robotStatus.ToString();
            }
        }
    }
}
