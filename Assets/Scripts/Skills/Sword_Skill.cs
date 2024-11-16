using UnityEngine;
using UnityEngine.Serialization;

public enum SwordType
{
    Regular,
    Bounce,
    Pierce,
    Spin
}
public class Sword_Skill : Skill
{
    public SwordType swordType = SwordType.Regular; 
    
    [FormerlySerializedAs("amountOfBounce")]
    [Header("Bounce info")] 
    [SerializeField] private int bounceAmount;
    [SerializeField] private float bounceGravity;
    [SerializeField] private float bounceSpeed;
    
    [Header("Pierce info")]     
    [SerializeField] private int pierceAmount;
    [SerializeField] private float pierceGravity;
    
    [Header("Spin info")]     
    [SerializeField] private float hitCooldown = .35f;
    [SerializeField] private float maxTravelDistance = 7;
    [SerializeField] private float spinDuration = 2;
    [SerializeField] private float spinGravity = 1;
    
    [Header("Sword Skill info")]
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 launchForce;
    [SerializeField] private float swordGravity;
    [SerializeField] private float freezeTimeDuration;
    [SerializeField] private float returnSpeed;
    
    private Vector2 finalDirection;
    
    [Header("Aim dots")]
    [SerializeField] private float spaceBetweenDots;
    [SerializeField] private int numberOfAimDots;
    [SerializeField] private GameObject aimDotPrefab;
    [SerializeField] private Transform aimDotsParent;    
    
    private GameObject[] aimDots;

    protected override void Start()
    {
        base.Start();
        
        GenerateAimDots();

        SetupGravity();
    }
    protected override void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse1)) 
            finalDirection = new Vector2(AimDirection().normalized.x * launchForce.x, AimDirection().normalized.y * launchForce.y);

        if (Input.GetKey(KeyCode.Mouse1))
        {
            for (int i = 0; i < aimDots.Length; i++)
            {
                aimDots[i].transform.position = AimDotsPosition(i * spaceBetweenDots);
            }
            
        }
    }
    private void SetupGravity()
    {
        if (swordType == SwordType.Bounce)
            swordGravity = bounceGravity;
        else if (swordType == SwordType.Pierce)
            swordGravity = pierceGravity;
        else if (swordType == SwordType.Spin) 
            swordGravity = spinGravity;
    }
    public void CreateSword()
    {
        GameObject newSword = Instantiate(swordPrefab, player.transform.position, transform.rotation);
        Sword_Skill_Controller newSwordScript = newSword.GetComponent<Sword_Skill_Controller>();
        
        if (swordType == SwordType.Bounce)
            newSwordScript.SetupBounce(true, bounceAmount, bounceSpeed);
        else if (swordType == SwordType.Pierce)
            newSwordScript.SetupPierce(pierceAmount);
        else if (swordType == SwordType.Spin)
            newSwordScript.SetupSpin(true, maxTravelDistance, spinDuration, hitCooldown);
        
        newSwordScript.SetupSword(finalDirection, swordGravity, player, freezeTimeDuration, returnSpeed);
        
        player.AssignNewSword(newSword);
        
        AimDotsActive(false);
    }
    
    #region Aiming
    public Vector2 AimDirection()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);    
        Vector2 aimDirection = mousePosition - playerPosition;
        
        return aimDirection;
    }

    public void AimDotsActive(bool _isActive)
    {
        for (int i = 0; i < aimDots.Length; i++)
        {
            aimDots[i].SetActive(_isActive);
        }
    }
    
    private void GenerateAimDots()
    {
        aimDots = new GameObject[numberOfAimDots];
        for (int i = 0; i < numberOfAimDots; i++)
        {
            aimDots[i] = Instantiate(aimDotPrefab, player.transform.position, Quaternion.identity, aimDotsParent);
            aimDots[i].SetActive(false);
        }
    }

    private Vector2 AimDotsPosition(float _t)
    {
        Vector2 playerPosition = (Vector2)player.transform.position + new Vector2(
            AimDirection().normalized.x * launchForce.x,
            AimDirection().normalized.y * launchForce.y) * _t + Physics2D.gravity * (.5f * swordGravity * (_t * _t));
        
        return playerPosition;
    }
    #endregion
}
