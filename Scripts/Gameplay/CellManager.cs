using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Gameplay
{
    public class CellManager : Singleton<CellManager>
    {
        [SerializeField]
        public List<Gene> GlobalGenes;

        public GameObject cellPrefab;

        public float totalWaterGained = 30;
        
        
        public static Vector2Int[] Directions4 =
        {
            new Vector2Int(-1, 0),
            new Vector2Int(0, -1),
            new Vector2Int(0, 1),
            new Vector2Int(1, 0),
        };
        
        public static Vector2Int[] Directions =
        {
            new Vector2Int(-1,-1), 
            new Vector2Int(-1,0),  
            new Vector2Int(-1,1),
            new Vector2Int(0,-1),
            new Vector2Int(0,1),
            new Vector2Int(1,-1), 
            new Vector2Int(1,0),  
            new Vector2Int(1,1),
        };
        
        bool Exposed(Vector2Int pos)
        {
            foreach (var dir in CellManager.Directions4)
            {
                if (!CellMap.ContainsKey(pos + dir))
                {
                    return true;
                }
            }
            return false;
        }

        public Dictionary<Vector2Int, Cell> CellMap = new Dictionary<Vector2Int, Cell>();

        public int CellCount => CellMap.Count;

        BlockType Cell2Block(CellType cell)
        {
            return (BlockType)(cell+1);
        }

        public bool MatchPos(Vector2Int pos, Gene gene)
        {
            foreach (var dir in Directions)
            {
                Vector2Int checkPos = pos + dir;
                Vector2Int id = dir + Vector2Int.one;
                BlockType thisType = gene.TypeCondition[id.x,id.y];
                if (thisType == BlockType.Any)
                    continue;
                if (CellMap.ContainsKey(checkPos))
                {
                    if(Cell2Block(CellMap[checkPos].type) != gene.TypeCondition[id.x,id.y])
                        return false;
                }
                else
                {
                    if (thisType != BlockType.Earth && thisType != BlockType.Rock) return false;
                    bool conditionIsRock = thisType == BlockType.Rock;
                    if (EarthManager.Instance.IsRock(checkPos) == conditionIsRock)
                        continue;
                    return false;
                }
            }
            return true;
        }

        public bool IsEmpty(Vector2Int pos)
        {
            return !CellMap.ContainsKey(pos);
        }

        public bool progressDone;

        [Button]
        public void StartProgressRound()
        {
            if(!progressDone) return;
            ScrollViewList.Instance.SyncGene();
            StartCoroutine(ProgressRound());
            EventHandler.CallOnChangeDays(1);//加的推进一天
        }

        void AddCell(Vector2Int pos, CellType type)
        {
            Cell thisCell = Instantiate(cellPrefab).GetComponent<Cell>();
            thisCell.transform.position = new Vector3(pos.x, pos.y, 0);
            thisCell.pos = pos;
            thisCell.type = type;
            thisCell.animationName = type switch
            {
                CellType.Stem => "StemIdle",
                CellType.Store => "StoreIdle",
                CellType.Cap => "CapIdle",
                _ => null
            };
            CellMap.Add(pos, thisCell);
        }
        private void Start()
        {
            totalWaterGained = 20;
            progressDone = true;
            AddCell(Vector2Int.zero, CellType.Cap);
            AddCell(Vector2Int.down, CellType.Store);
            AddCell(Vector2Int.down*2, CellType.Stem);
            AddCell(Vector2Int.down*3, CellType.Cap);
            AddCell(Vector2Int.down+Vector2Int.right, CellType.Cap);
            AddCell(Vector2Int.down+Vector2Int.left, CellType.Cap);
            AddCell(Vector2Int.down*2+Vector2Int.right, CellType.Cap);
            AddCell(Vector2Int.down*2+Vector2Int.left, CellType.Cap);
        }
        bool WithStore(Vector2Int pos)
        {
            foreach (var dir in Directions)
            {
                if (CellMap.ContainsKey(dir+pos))
                {
                    //if (CellMap[dir + pos].type == CellType.Stem) quantity = 1; 
                    if (CellMap[dir + pos].type == CellType.Store) return true;
                }
            }

            return false;
        }
        float Absorb(Vector2Int pos)
        {
            float result = 0;
            var map = EarthManager.Instance.EarthMap;
            foreach (var dir in Directions)
            {
                if (map.ContainsKey(dir+pos))
                {
                    result += map[dir+pos].Absorb(1);
                }
            }
            return result;
        }
        public float totalWaterConsume = 0, totalWaterAbsorbed = 0;

        private void DFS(Vector2Int pos)
        {
            CellMap[pos].isVisit = true;
            foreach (var dir in Directions)
            {
                if (CellMap.ContainsKey(pos + dir))
                {
                    var cell = CellMap[pos + dir];
                    if(cell.isVisit) continue;
                    DFS(cell.pos);
                }
            }
        }

        IEnumerator ProgressRound()
        {
            progressDone = false;
            
            List<Cell> stems = new List<Cell>();
            totalWaterConsume = 0;
            totalWaterAbsorbed = 0;

            foreach (var cell in CellMap) cell.Value.isVisit = false;
            
            DFS(Vector2Int.zero);
            
            foreach (var cell in CellMap)
            {
                switch (cell.Value.type)
                {
                    case CellType.Stem:
                        stems.Add(cell.Value);
                        totalWaterConsume -= 8;
                        
                        break;
                    case CellType.Cap:
                        totalWaterConsume -= 0.5f;
                        if(cell.Value.isVisit && WithStore(cell.Key)){
                            totalWaterAbsorbed += Absorb(cell.Key);
                        }
                        break;
                    case CellType.Store:
                        totalWaterConsume -= 0.5f;
                        break;
                }
            }

            stems.Sort((Cell at, Cell bt) =>
            {
                var a = at.pos;
                var b = at.pos;
                var t = a.y - b.y; 
                return t == 0? a.x-b.x : t;
            });

            foreach (var c in stems)
            {
                foreach (var gene in GlobalGenes)
                {
                    if (MatchPos(c.pos, gene))
                    {
                        c.PerformTask(gene);
                        break;
                    }
                }
                if (c.isMoving) {
                    yield return new WaitUntil(() => c.isMoving == false);
                }
            }

            progressDone = true;
            EarthManager earth = EarthManager.Instance;
            var cellList = CellMap.ToList();

            int stemCount = 0;
            
            foreach (var cell in cellList)
            {
                earth.UpdateBound(cell.Key);
                if (cell.Value.type == CellType.Stem) stemCount ++;
                if(cell.Value.type == CellType.Cap) continue;
                if (Exposed(cell.Key))
                {
                    cell.Value.Damage();
                }
                else
                {
                    cell.Value.Recover();
                }
            }
            earth.GenerateBound();
            
            
            totalWaterGained += totalWaterAbsorbed + totalWaterConsume;
        }
    }
}