
using System;
using System.Collections.Generic;
using Core;
using Core.Core;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay
{
    public class EarthManager : Singleton<EarthManager>
    {
        public Dictionary<Vector2Int,Earth> EarthMap = new Dictionary<Vector2Int, Earth>();

        public int lowest = -3, leftest = -1, rightest=1, highest=0;

        public int LowSee => lowest - 10;
        public int LeftSee => leftest - 10;
        public int RightSee => rightest + 10;
        public int HighSee => 10;

        private int LowGen => LowSee - 5;
        private int LeftGen => LeftSee - 5;
        private int RightGen => RightSee + 5;

        private int NewLowGen => newLowest - 15;
        private int NewLeftGen => newLeftest - 15;
        private int NewRightGen => newRightest + 15;

        public Action<float,float,float> OnRefreshBound;


        private int newLeftest, newRightest, newLowest, newHighest;

        [SerializeField] private GameObject earthPrefab;
        
        public void UpdateBound(Vector2Int p)
        {
            newLeftest = Mathf.Min(leftest, p.x);
            newRightest = Mathf.Max(rightest, p.x);
            newLowest = Mathf.Min(lowest, p.y);
            newHighest = Mathf.Max(highest, p.y);
        }

        public void GenerateBound()
        {
            for (int x = NewLeftGen; x < LeftGen; x++)
            {
                for (int y = 0; y >= NewLowGen; y --)
                {
                    EarthRandom(x,y);
                }
            }
            for (int x = RightGen+1; x <= NewRightGen; x++)
            {
                for (int y = 0; y >= NewLowGen; y --)
                {
                    EarthRandom(x,y);
                }
            }
            for (int x = LeftGen; x < RightGen; x++)
            {
                for (int y = LowGen-1; y >= NewLowGen; y --)
                {
                    EarthRandom(x,y);
                }
            }

            highest = Mathf.Max(highest,newHighest);
            lowest = Mathf.Min(lowest, newLowest);
            leftest = Mathf.Min(leftest,newLeftest);
            rightest = Mathf.Max(rightest,newRightest);

            //OnRefreshBound(LowSee, LeftSee, RightSee);
        }

        public void Fill(float ratio)
        {
            foreach (var e in EarthMap)
            {
                e.Value.abundant = e.Value.wetness * ratio;
            }
        }

        public void Alleviate(float ratio)
        {
            foreach (var e in EarthMap)
            {
                e.Value.abundant = Mathf.Min(e.Value.wetness * ratio,e.Value.abundant);
            }
        }

        void AddEarth(Vector2Int pos, float wet)
        {
            if (pos.y > 0) return;
            if (EarthMap.ContainsKey(pos))
            {
                var e = EarthMap[pos];
                e.wetness += wet;
                e.abundant += wet;
                e.ResetWetness();
                return;
            }
            var i = Instantiate(earthPrefab);
            i.transform.position = new Vector3(pos.x, pos.y, 0);
            Earth earth = i.GetComponent<Earth>();
            earth.wetness = wet;
            earth.abundant = wet;
            EarthMap.Add(pos, earth);
        }

        void SetPeak(Vector2Int pos, float peak, int radius)
        {
            int sqrR = radius * radius;
            for (int x = pos.x - radius; x <= pos.x + radius; x ++)
            {
                for (int y = pos.y - radius; y <= pos.y + radius; y ++)
                {
                    Vector2Int p = new Vector2Int(x, y);
                    float sqrM = (p - pos).sqrMagnitude;
                    //Debug.Log(p-pos);
                    if (sqrM > sqrR) continue;
                    AddEarth(p, peak * (radius - Mathf.Sqrt(sqrM))/radius);
                }
            }
        }

        bool RandomSuccess(int y, float r)
        {
            if (Random.value < (1-Mathf.Exp(y*0.02f))*r)
            {
                //Debug.Log("Success");
                return true;
            }
            return false;
        }

        void EarthRandom(int x, int y)
        {
            Vector2Int pos = new Vector2Int(x, y);
            if (y <= -10)
            {
                if (RandomSuccess(y,0.1f))
                //if (true)
                {
                    AddEarth(pos,0);
                    EarthMap[pos].isRock = true;
                    return;
                }
            }
            AddEarth(pos,0);
            EarthMap[pos].isRock = false;
            if (RandomSuccess(y,0.05f)) SetPeak(pos,40,5);
        }
            
        private void Start()
        {
            
            for (int x = LeftGen; x <= RightGen; x++)
            {
                for (int y = 0; y >= LowGen; y--)
                {
                    EarthRandom(x,y);
                }
            }
            SetPeak(Vector2Int.down*3, 40, 5);
            EventHandler.OnDrought += Alleviate;
            EventHandler.OnRain += Fill;
        }

        public bool IsRock(Vector2Int pos)
        {
            if (EarthMap.ContainsKey(pos)) return EarthMap[pos].isRock;
            return true;
        }
    }
}
