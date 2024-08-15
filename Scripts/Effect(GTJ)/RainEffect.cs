using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainEffect : MonoBehaviour
{
    private ParticleSystem SelfEffect;
    public GameObject RainHit;
    public GameObject Ground;
    public float GroundOffset = 0.5f;
    [Range(0,1)]
    public float SputteringPrabability = 0.5f;
    private void Awake()
    {
        SelfEffect = this.transform.GetComponent<ParticleSystem>();
    }
    private void OnParticleTrigger()
    {
          List<ParticleSystem.Particle> CollisonParticles = new List<ParticleSystem.Particle>();
          int NumCollision = SelfEffect.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, CollisonParticles);
          for(int i = 0; i < NumCollision; i++)
          {
               if (Random.Range(0f, 1f) > SputteringPrabability) continue;
               GameObject NewRainHit=GameObject.Instantiate(RainHit);
               NewRainHit.transform.position =new Vector3(CollisonParticles[i].position.x,Ground.transform.position.y+GroundOffset,0) ;
               Destroy(NewRainHit,1.5f);
          }
    }
}
