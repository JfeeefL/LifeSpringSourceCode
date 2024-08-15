using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core
{
    [Serializable]
    public class Gene
    {
        [ShowInInspector]
        public BlockType[,] TypeCondition = new BlockType[3,3];
        public Gradient[,] GradientCondition = new Gradient[3, 3];
        public TaskType Task;
        public Vector2Int Direction;

    }

    public enum Gradient
    {
        Any, Lower, Higher
    }
    public enum BlockType
    {
        Any, 
        Stem,
        Store,
        Cap,
        Earth,
        Rock
    }

    public enum TaskType
    {
        Reproduce,
        ToCap, 
        ToStore, 
        Idle, 
    }
}