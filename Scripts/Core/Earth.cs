using System;
using Gameplay;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Core
{
    public class Earth: MonoBehaviour
    {
        public bool isRock;
        public float wetness;
        public float abundant;

        [SerializeField] private Sprite[] EarthSprite, RockSprite;
        [SerializeField] private Sprite WaterSprite;

        public string GetEarthInfo()
        {
            return "<color=#f17a17><b>土壤含水量</b></color>为"+(int)abundant + "\n";
        }
        private void Start()
        {
            var wetDraw = GetComponent<SpriteRenderer>();
            wetDraw.sprite = WaterSprite;
            wetDraw.color = new Color(1, 1, 1, 0);
            if(!isRock)
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = EarthSprite[Random.Range(0, EarthSprite.Length)];
            else transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = RockSprite[Random.Range(0, RockSprite.Length)];
            ResetWetness();
            
            GetComponent<HitPoint>().GetHitWords = () =>
            {
                Vector2Int v = Vector2Int.RoundToInt(transform.position);
                return (CellManager.Instance.CellMap.ContainsKey(v)?
                    CellManager.Instance.CellMap[Vector2Int.RoundToInt(transform.position)].thisWord:"") +
                       GetEarthInfo();
            };
        }

        public void ResetWetness()
        {
            if(isRock) return;
            var wetDraw = GetComponent<SpriteRenderer>();
            var color = wetDraw.color;
            color = new Color(color.r, color.g, color.b, Mathf.Min(1, abundant / 100));
            wetDraw.color = color;
        }

        public float Absorb(float maxAbsorb)
        {
            float result = Mathf.Min(abundant, maxAbsorb);
            abundant -= result;
            ResetWetness();
            return result;
        }
        
    }
}