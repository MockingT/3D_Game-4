using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskFactory : MonoBehaviour
{ 
    public GameObject disk;
    private List<DiskData> used;
    private List<DiskData> free;
    private int count;
    public Camera cam;

    // Initialization
    void Awake()
    {
        used = new List<DiskData>();
        free = new List<DiskData>();
        count = 0;
    }

    // get the count of the used disks
    public int usedCount()
    {
        return used.Count; 
    }

    // if click the disk
    void FixedUpdate()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Vector3 mouse = Input.mousePosition;
            Camera ca = cam.GetComponent<Camera>();
            Ray ray = ca.ScreenPointToRay(mouse);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.tag.Contains("Disk")) // if hit the disk
                { 
                    DiskData theDisk = hit.collider.gameObject.GetComponent<DiskData>();
                    theDisk.reset();
                    FreeDisk(theDisk);
                    ScoreRecorder a = Singleton<ScoreRecorder>.Instance;
                    a.Record(theDisk); // get score
                }
            }
        }
    }

    // get a new disk and launch it
    public int getDisk(int ruler)
    {
        DiskData theDisk;
        if (free.Count > 0) // if there is free disk
        {
            int free_index = free.Count - 1; // get the free disk's index
            theDisk = free.ToArray()[free_index];
            free.RemoveAt(free_index);
            theDisk.setStart(ruler);
            used.Add(theDisk);
        }
        else // create a new disk
        {
            count++;
            GameObject newDisk = Instantiate(disk) as GameObject;
            newDisk.name = "Disk" + count.ToString();
            theDisk = newDisk.GetComponent<DiskData>();
            theDisk.innerDiskCount = count;
            theDisk.setStart(ruler);
            used.Add(theDisk);
        }
        return theDisk.indexInUsed = used.Count - 1;
    }

    //3 ways of freedisk
    public void FreeDisk(int index)
    {
        DiskData theDisk = used.ToArray()[index];
        if(theDisk != null)
        {
            used.Remove(theDisk);
            free.Add(theDisk);
        }
    }
    public void FreeDisk(DiskData _disk)
    {

        DiskData theDisk = null;
        foreach (DiskData _Disk in used)
        {
            if (_Disk.innerDiskCount == _disk.innerDiskCount)
            {
                theDisk = _Disk;
            }
        }
        if(theDisk != null)
        {
            theDisk.reset();
            free.Add(theDisk);
            used.Remove(theDisk);
        }
    }
    public void FreeAllDisks() // free the used disks
    {
        int i = 0;
        for (i = used.Count - 1; i >= 0; i--)
        {
            DiskData disk = used[i];
            used.Remove(disk);
            free.Add(disk);
        }
    }
}

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (T)FindObjectOfType(typeof(T));
                if (instance == null)
                {
                    Debug.LogError("An instance of " + typeof(T) + " is needed in the scene, but there is none.");
                }
            }
            return instance;
        }
    }
}
