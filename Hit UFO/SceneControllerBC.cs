using UnityEngine;
using System.Collections;
using Com.Mygame;

namespace Com.Mygame
{
    public interface IUserInterface
    {
        void emitDisk();
    }

    public interface IQueryStatus
    {
        bool isCounting();
        bool isShooting();
        int getRound();
        int getPoint();
        int getEmitTime();
    }

    public interface IJudgeEvent
    {
        void nextRound();
        void setPoint(int point);
    }

    public class SceneController : System.Object, IQueryStatus, IUserInterface, IJudgeEvent
    {
        private static SceneController _instance;
        private SceneControllerBC _baseCode;
        private GameModel _gameModel;
        private Judge _judge;

        private int _round;
        private int _point;

        public static SceneController getInstance()
        {
            if (_instance == null)
            {
                _instance = new SceneController();
            }
            return _instance;
        }

        public void setGameModel(GameModel obj) { _gameModel = obj; }
        internal GameModel getGameModel() { return _gameModel; }

        public void setJudge(Judge obj) { _judge = obj; }
        internal Judge getJudge() { return _judge; }

        public void setSceneControllerBC(SceneControllerBC obj) { _baseCode = obj; }
        internal SceneControllerBC getSceneControllerBC() { return _baseCode; }

        // 操作接口  
        public void emitDisk() { _gameModel.prepareToEmitDisk(); }

        // 查询接口  
        public bool isCounting() { return _gameModel.isCounting(); }
        public bool isShooting() { return _gameModel.isShooting(); }
        public int getRound() { return _round; }
        public int getPoint() { return _point; }
        public int getEmitTime() { return (int)_gameModel.timeToEmit + 1; }

        // 得分接口  
        public void setPoint(int point) { _point = point; }
        public void nextRound() { _point = 0; _baseCode.loadRoundData(++_round); }
    }

    public class SceneControllerBC : MonoBehaviour
    {
        private Color color;
        private Vector3 emitPos;
        private Vector3 emitDir;
        private float speed;

        void Awake()
        {
            SceneController.getInstance().setSceneControllerBC(this);
        }

        public void loadRoundData(int round)
        {
            switch (round)
            {
                case 1:     // 第一关  
                    color = Color.green;
                    emitPos = new Vector3(-2.5f, 0.2f, -5f);
                    emitDir = new Vector3(24.5f, 40.0f, 67f);
                    speed = 4;
                    SceneController.getInstance().getGameModel().setting(1, color, emitPos, emitDir.normalized, speed, 1);
                    break;
                case 2:     // 第二关  
                    color = Color.red;
                    emitPos = new Vector3(2.5f, 0.2f, -5f);
                    emitDir = new Vector3(-24.5f, 35.0f, 67f);
                    speed = 4;
                    SceneController.getInstance().getGameModel().setting(1, color, emitPos, emitDir.normalized, speed, 2);
                    break;
            }
        }
    }
}