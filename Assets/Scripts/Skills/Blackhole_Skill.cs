using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackhole_Skill : Skill
{   
    [Header("Blackhole Info")]
    [SerializeField] private GameObject blackholePrefab;
    [SerializeField] private float blackholeDuration;
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;
    
    [SerializeField] private int amountOfAttacks;
    [SerializeField] private float cloneAttackCooldown;
    
    Blackhole_Skill_Controller currentBlackhole;
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();
        
        GameObject newBlackhole = Instantiate(blackholePrefab, player.transform.position, Quaternion.identity);
        
        Blackhole_Skill_Controller newBlackholeScript = newBlackhole.GetComponent<Blackhole_Skill_Controller>();
        
        newBlackholeScript.SetupBlackhole(maxSize, growSpeed, shrinkSpeed, amountOfAttacks, cloneAttackCooldown, blackholeDuration);       
        
        currentBlackhole = newBlackholeScript; // Assign the current blackhole
    }   
    
    public bool BlackholeSkillFinished()
    {
        if (!currentBlackhole)
            return false;
        
        if (currentBlackhole.playerCanExitState)
        {
            currentBlackhole = null;
            return true;
        }

        return false;
    }
}
