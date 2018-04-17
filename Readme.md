# 3D_Game-4  
## 简单的鼠标打飞碟（Hit UFO）游戏（视频见：http://v.youku.com/v_show/id_XMzU0NTcxODE2OA==.html?spm=a2hzp.8244740.0.0）  
### 游戏内容要求：  
- 游戏有 n 个 round，每个 round 都包括10 次 trial；  
- 每个 trial 的飞碟的色彩、大小、发射位置、速度、角度、同时出现的个数都可能不同。它们由该 round 的 ruler 控制；  
- 每个 trial 的飞碟有随机性，总体难度随 round 上升；  
- 鼠标点中得分，得分规则按色彩、大小、速度不同计算，规则可自由设定。  
### 游戏的要求：  
- 使用带缓存的工厂模式管理不同飞碟的生产与回收，该工厂必须是场景单实例的！具体实现见参考资源 Singleton 模板类  
- 近可能使用前面 MVC 结构实现人机交互与游戏模型分离  
### 游戏类图：  
![avatar](https://github.com/MockingT/3D_Game-4/blob/master/picture/3d1.png)  
### 文件框架：  
![avatar](https://github.com/MockingT/3D_Game-4/blob/master/picture/3d2.png)  
其中仅做了飞盘这一个预设，其余的SoreRecorder.cs（实现分数记录）, RoundController.cs（控制游戏的开始结束以及进入下一个Round）, DiskFactory.cs（如课件中要求，添加一个disk工厂，管理每一round中disk的出现落地被点击等动作）, DiskData.cs（Disk的基本属性和设置，包括大小出发位置颜色，速度等）
均添加到主摄像机上即可。  
### Camera设置以及游戏规则：  
![avatar](https://github.com/MockingT/3D_Game-4/blob/master/picture/3d5.png)  
RoundController文件添加到主摄像机之后，用户可以选择每一轮出现的飞碟数量，如图我输入的飞碟数为10，并且每一Round飞碟数不变，当用户成功点击飞碟时，在第一轮会得到10分，第二轮每击中一个得20分，第三轮则是30分，以此类推。  
### 效果展示：  
![avatar](https://github.com/MockingT/3D_Game-4/blob/master/picture/3d3.png)  
![avatar](https://github.com/MockingT/3D_Game-4/blob/master/picture/3d4.png)  

### 代码部分：  
- DiskData.cs  

      using System.Collections;
      using System.Collections.Generic;
      using UnityEngine;

      public class DiskData : MonoBehaviour, OnReachEndCallback
      {
          private float time;
          public float g = -9.8f; // gravity speed
          private Vector3 pointA; // start from point a
          private Vector3 pointB; // end at point b
          private Vector3 _speed; // moving speed
          private Vector3 Gravity; 
          private Vector3 currentAngle; // the angle
          private float dTime = 0;
          private float shotSpeed; // launching speed
          public int indexInUsed { get; set; }
          public int shotScore { get; set; }
          public int innerDiskCount { get; set; }
          private int timeCount;
          private int currentTimeCount;
          public bool isEnabled { get; set; } // whether it can move
          public bool reachedEnd // whether reached
          {
              get
              {
                  if (currentTimeCount >= timeCount)
                      return true;
                  return false;
              }
          }

          // set the random start and end point
          public static Vector3 getRandomStartPoint()
          {
              Vector3 random = new Vector3(Random.Range(-1.5f, 1.5f), 1.5f, -12f);
              return random;
          }
          public static Vector3 getRandomEndPoint()
          {
              Vector3 random = new Vector3(Random.Range(-5f, 5f), Random.Range(3f, 8f), 5f);
              return random;
          }
          // set random color
          public static Color getRandomColor()
          {
              float r = Random.Range(0f, 1f);
              float g = Random.Range(0f, 1f);
              float b = Random.Range(0f, 1f);
              Color tcolor = new Color(r, g, b);
              return tcolor;
          }

          public void ReachEndCallback(DiskData disk)
          {
              Singleton<DiskFactory>.Instance.FreeDisk(disk);
          }

          // set the shape and the color
          public void setShapeColor(int ruler)
          {
              Renderer render = this.transform.GetComponent<Renderer>();
              render.material.shader = Shader.Find("Transparent/Diffuse");
              render.material.color = getRandomColor();
              this.transform.localScale = new Vector3(2 - 0.1f * ruler, 2 - 0.1f * ruler, 2 - 0.1f * ruler);
          }

          // Initialize: set the start location and the shape, color
          public void setStart(int a)
          {
              setShapeColor(a);
              timeCount = (int)(2f / Time.deltaTime);
              currentTimeCount = 0;
              this.isEnabled = true;
              this.shotScore = 10 * a; // in round1, 10points per hit, round2, 20points per hit...
              shotSpeed = 20f + 10f * a;
              pointA = getRandomStartPoint();
              pointB = getRandomEndPoint();
              time = Vector3.Distance(pointA, pointB) / shotSpeed;
              transform.position = pointA;
              _speed = new Vector3((pointB.x - pointA.x) / time,
                  (pointB.y - pointA.y) / time - 0.5f * g * time, (pointB.z - pointA.z) / time);
              Gravity = Vector3.zero;
          }

          public void reset()
          {
              isEnabled = false;
              this.transform.position = pointA;
              _speed = Vector3.zero;
              Gravity = Vector3.zero;
              currentAngle = Vector3.zero;
              this.transform.eulerAngles = currentAngle;
              currentTimeCount = 0;
              dTime = 0;
          }

          void FixedUpdate()
          {
              if (isEnabled && this.transform.position != pointB) // keep moving
              {
                  currentTimeCount++;
                  Gravity.y = g * (dTime += Time.fixedDeltaTime);
                  transform.position += (_speed + Gravity) * Time.fixedDeltaTime;
                  currentAngle.x = -Mathf.Atan((_speed.y + Gravity.y) / _speed.z) * Mathf.Rad2Deg;
                  transform.eulerAngles = currentAngle;
              }
              if (this.reachedEnd) // if it has reached the end and not get clicked
              {
                  reset();
                  ReachEndCallback(this);
              }
          }
      }

      public interface OnReachEndCallback
      {
          void ReachEndCallback(DiskData disk);
      }  
      
- DiskFactory.cs  

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
      
- RoundController.cs  

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
           
- ScoreRecorder.cs  

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
