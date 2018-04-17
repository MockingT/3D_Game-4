# 3D_Game-4  
## 简单的鼠标打飞碟（Hit UFO）游戏  
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
