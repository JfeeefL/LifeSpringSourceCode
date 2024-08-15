using System;
using System.Collections.Generic;
using DG.Tweening;
using Gameplay;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Core
{
    [RequireComponent(typeof(Animator))]
    public class Cell: MonoBehaviour
    {
        [HideInInspector] public CellType type;
        //public List<Gene> Genes;
        [HideInInspector] public Vector2Int pos;
        [HideInInspector] public bool isMoving;
        [HideInInspector] public string animationName;
        private Animator _animator;
        public bool isVisit; 
        
        int _health = 5;

        private void Awake()
        {
            _health = 5;
        }

        public string WordStem = "<color=#f17a17><b>干细胞</b></color>\n高耗水但可分裂的细胞\n";
        public string WordStore = "<color=#f17a17><b>储水细胞</b></color>\n低耗水且可储水的细胞\n";
        public string WordCap = "<color=#f17a17><b>保卫细胞</b></color>\n保护细胞免受脱水\n";

        public string thisWord => type switch
        {
            CellType.Stem => WordStem,
            CellType.Store => WordStore,
            CellType.Cap => WordCap,
            _ => "Undefined\n"
        };
        private void Start()
        {
            //Genes = CellManager.Instance.GlobalGenes;
            _animator = GetComponent<Animator>();
            _animator.Play(animationName);

            
            GetComponent<HitPoint>().GetHitWords = () => thisWord + EarthManager.Instance.EarthMap[pos].GetEarthInfo();
        }

        private int _dotweenCount;

        public void Damage()
        {
            _health--;
            _animator.SetInteger("Health",_health);
            if (_health == 0)
            {
                CellManager.Instance.CellMap.Remove(pos);
                transform.DOScaleY(0, 0.2f).OnComplete(() => { Destroy(gameObject); });
            }
        }

        public void Recover()
        {
            _health = 5;
            _animator.SetInteger("Health",_health);
        }
        
        
        public void PerformTask(Gene gene)
        {
            if (gene.Task == TaskType.Idle) return;
            if (gene.Task == TaskType.ToCap)
            {
                type = CellType.Cap;
                isMoving = false;
                
                _animator.SetInteger("CellType",1);
                // 加动画之后，修改这里，使得动画播放完之后才能把IsMoving设置为false

                return;
            }

            if (gene.Task == TaskType.ToStore)
            {
                type = CellType.Store;
                isMoving = false;
                
                _animator.SetInteger("CellType",2);
                // 加动画之后，修改这里，使得动画播放完之后才能把IsMoving设置为false
                
                return;
            }
            //Debug.Log("Reproducable at" + pos );
            var dir = gene.Direction;
            isMoving = true;
            int step = 0;
            List<Cell> pushes = new List<Cell>();
            Vector2Int nowPos = pos + dir;
            while (step < 2)
            {
                if (EarthManager.Instance.IsRock(nowPos))
                {
                    //Debug.Log("Blocked by rock");
                    step = 2;
                    break;
                }

                if (CellManager.Instance.IsEmpty(nowPos)) break;
                pushes.Add(CellManager.Instance.CellMap[nowPos]);
                nowPos += dir;
                step++;
            }

            if (step == 2)
            {
                //Debug.Log("Blocked");
                isMoving = false;
                return;
            }
            
            //Debug.Log("Pushable");
            
            _dotweenCount = 1 + pushes.Count;
            pushes.Reverse();
            Cell newCell = Instantiate(CellManager.Instance.cellPrefab).GetComponent<Cell>();
            newCell.animationName = "StemIdle";
            newCell.transform.position = new Vector3(pos.x, pos.y, 0);
            newCell.pos = pos+dir;
            newCell.transform.DOMove(new Vector3(newCell.pos.x, newCell.pos.y, 0),0.3f).OnComplete(() =>
            {
                _dotweenCount--;
                if (_dotweenCount == 0)
                {
                    isMoving = false;
                }
            });
            foreach (var c in pushes)
            {
                CellManager.Instance.CellMap.Remove(c.pos);
                c.pos += dir;
                CellManager.Instance.CellMap.Add(c.pos, c);
                c.transform.DOMove(new Vector3(c.pos.x, c.pos.y, 0), 0.3f).OnComplete(() =>
                {
                    _dotweenCount--;
                    if (_dotweenCount == 0)
                    {
                        isMoving = false;
                    }
                });
            }
            CellManager.Instance.CellMap.Add(newCell.pos,newCell);
        }
    }

    public enum CellType
    {
        Stem, Store, Cap
    }
}