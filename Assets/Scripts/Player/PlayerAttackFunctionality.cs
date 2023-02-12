using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerAttack))]
public abstract class PlayerAttackFunctionality : MonoBehaviour 
{

    protected Camera mainCam;
    protected CameraRecoil cameraRecoil;

    protected GameObject abilityMarkerPrefab;

    int reentryHash;

    private bool isWaiting;

    // locals
    [HideInInspector] public PlayerAttack attack;

    public void Awake()
    {
        this.attack              = this.GetComponent<PlayerAttack>();
        this.cameraRecoil        = FindObjectOfType<CameraRecoil>();
        this.mainCam             = Camera.main;
        this.abilityMarkerPrefab = Resources.Load<GameObject>("AbilityMarker");

        reentryHash = Animator.StringToHash("reentry");
    }

    public abstract void Fire();

    public abstract void FireAimed(RaycastHit hitInfo);

    private void OnEnable()
    {
        if (!attack.animator) return;
        attack.animator.SetTrigger(reentryHash);
    }

    protected void HitStop(float duration)
    {
        if (isWaiting) return;
        StartCoroutine(HitStopCoroutine(duration));
    }
    private IEnumerator HitStopCoroutine(float duration)
    {
        Time.timeScale = 0.0f;
        isWaiting = true;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1.0f;
        isWaiting = false;

        yield return null;
    }

}
