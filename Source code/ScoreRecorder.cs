using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreRecorder : MonoBehaviour
{
    private int score;
    void Awake()
    {
        Reset();
    }
    public void Reset()
    {
        score = 0;
    }
    // sum up the score
    public void Record(DiskData disk)
    {
        score += disk.shotScore;
    }
    // show the score text
    void OnGUI()
    {
        GUI.TextArea(new Rect(20, 20, 100, 30), "Score : " + score.ToString());
    }
}