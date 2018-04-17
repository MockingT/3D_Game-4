using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundController : MonoBehaviour, Rounds
{

    private DiskFactory disk_factory;
    private ScoreRecorder score_recorder;
    private bool startShot; // control the shooting state
    private int roundCount; // count the round

    [Header("The number of disks in each round")] // user defines it
    public int n; // n disks in a round
    private int cur_n; // the current number of delievered disks

    private int currentTimeCount;
    private int timeCountPerSec;

    void Awake()
    {
        disk_factory = Singleton<DiskFactory>.Instance;
        score_recorder = Singleton<ScoreRecorder>.Instance;
        // in case that user inputs a negative number 
        if (n < 1)
        {
            n = 10;
        }
        roundCount = 0; // start from 0
        timeCountPerSec = (int)(1f / Time.deltaTime);
    }

    void Update()
    {
        currentTimeCount++;
        if (startShot)
        {
            if (currentTimeCount % timeCountPerSec == 0 && cur_n < n)
            {
                cur_n++;
                if (cur_n == n)
                {
                    stopShotDisk();
                    return;
                }
                else
                    disk_factory.getDisk(roundCount);
            }
        }

    }

    public void nextRound() 
    {
        startShotDisk(); // can shoot now
        disk_factory.FreeAllDisks(); // reset the disks
        currentTimeCount = 0;
        cur_n = 0;
        roundCount++; // raise the round
    }

    public void getScore(DiskData disk)
    {
        score_recorder.Record(disk);
    }

    public void restartAll()
    {
        stopShotDisk();
        roundCount = 0;
        score_recorder.Reset();
    }

    // start & stop the shooting
    public void startShotDisk()
    {
        this.startShot = true;
    }
    public void stopShotDisk()
    {
        cur_n = 0;
        this.startShot = false;
    }

    void OnGUI()
    {
        if (cur_n == 0 && disk_factory.usedCount() == 0)
        {
            if (GUI.Button(new Rect(150, 20, 120, 30), "Start/Next Round"))
            {
                this.nextRound();
            }
            if (GUI.Button(new Rect(290, 20, 80, 30), "Replay"))
            {
                this.restartAll();
            }

        }
    }
}

public interface Rounds
{
    void nextRound();
    void getScore(DiskData disk);
    void startShotDisk();
    void stopShotDisk();
    void restartAll();
}
