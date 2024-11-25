using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Blackhole_Skill_Controller : MonoBehaviour
{
    [SerializeField] private GameObject hotkeyPrefab; 
    [SerializeField] private List<KeyCode> keyCodeList; 
    
    private float maxSize;
    private float growSpeed;
    private float shrinkSpeed;
    
    private bool canGrow = true;
    private bool canShrink;
    private bool canCreatedHotkeys = true;
    private bool cloneAttackReleased;
    private bool playerCanDissapear = true;
    
    private int amountOfAttacks = 4;
    private float cloneAttackCooldown = .3f;
    private float cloneAttackTimer;
    private float blackholeTimer;
    
    private List<Transform> targets = new List<Transform>();
    private List<GameObject> createdHotkeys = new List<GameObject>();

    public bool playerCanExitState { get; private set; }
    
    public void SetupBlackhole(float _maxSize, float _growSpeed, float _shrinkSpeed, int _amountOfAttacks, float _cloneAttackCooldown, float _blackholeDuration)
    {
        maxSize = _maxSize;
        growSpeed = _growSpeed; 
        shrinkSpeed = _shrinkSpeed;
        amountOfAttacks = _amountOfAttacks;
        cloneAttackCooldown = _cloneAttackCooldown;
        blackholeTimer = _blackholeDuration;
        
        cloneAttackTimer = cloneAttackCooldown;
    }
    
    private void Update()
    {
        cloneAttackTimer -= Time.deltaTime;
        blackholeTimer -= Time.deltaTime;

        if (blackholeTimer < 0)
        {
            blackholeTimer = Mathf.Infinity;
            
            if (targets.Count > 0)
                ReleaseCloneAttack();
            else
                FinishBlackHole();
        }
        
        if (Input.GetKeyDown(KeyCode.B))
        {
            ReleaseCloneAttack();
        }

        CloneAttackLogic();
        
        if (canGrow && !canShrink)
        { 
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }   
        if (canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);

            if (transform.localScale.x <= 0) 
                Destroy(gameObject);
        }
    }
    private void ReleaseCloneAttack()
    {
        if (targets.Count <= 0)
        {
            return;
        }
        
        DestroyHotkeys();
        cloneAttackReleased = true;
        canCreatedHotkeys = false;

        if (playerCanDissapear)
        {
            playerCanDissapear = false;
            PlayerManager.instance.player.MakeTransparent(true);
        }

    }
    private void CloneAttackLogic()
    {
        if (cloneAttackTimer < 0 && cloneAttackReleased && amountOfAttacks > 0)
        {
            cloneAttackTimer = cloneAttackCooldown;

            int randomIndex = Random.Range(0, targets.Count);

            float xOffSet;

            if (Random.Range(0, 100) > 50)
                xOffSet = 2;
            else
                xOffSet = -2;

            SkillManager.instance.clone.CreateClone(targets[randomIndex], new Vector3(xOffSet, 0));
            amountOfAttacks--;

            if (amountOfAttacks <= 0)
            {
                Invoke("FinishBlackHole", 1f);
            }
        }
    }

    private void FinishBlackHole()
    {
        DestroyHotkeys();
        playerCanExitState = true;
        canShrink = true;
        cloneAttackReleased = false;
    }

    private void DestroyHotkeys()
    {
        if (createdHotkeys.Count <= 0)
            return;
        
        foreach (var hotkey in createdHotkeys) 
            Destroy(hotkey);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTime(true);

            CreateHotkey(collision);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTime(false);
        }
    }
    
    private void CreateHotkey(Collider2D collision)
    {
        if (keyCodeList.Count <= 0)
        {
            Debug.Log("No more keys");
            return;
        }

        if (!canCreatedHotkeys)
            return;

        GameObject newHotkey = Instantiate(hotkeyPrefab, collision.transform.position + new Vector3(0, 2), Quaternion.identity);
        createdHotkeys.Add(newHotkey);

        KeyCode choosenKey = keyCodeList[Random.Range(0, keyCodeList.Count)];
        keyCodeList.Remove(choosenKey);

        Blackhole_Hotkey_Controller newHotkeyScript = newHotkey.GetComponent<Blackhole_Hotkey_Controller>();

        newHotkeyScript.SetupHotkey(choosenKey, collision.transform, this);
    }
    
    public void AddEnemyToList(Transform _enemyTransform) => targets.Add(_enemyTransform);
}
