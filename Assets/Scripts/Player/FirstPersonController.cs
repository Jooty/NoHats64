using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
public class FirstPersonController : MonoBehaviour
{

    [SerializeField] private bool isWalking;
    [SerializeField] [Range(0f, 1f)] private float runStepLengthen;
    [SerializeField] private float groundForce;
    [SerializeField] private float gravityMultiplier;
    [SerializeField] private MouseLook mouseLook;
    [SerializeField] private bool useFOVKick;
    [SerializeField] private FOVKick FOVKick = new FOVKick();
    [SerializeField] private bool useHeadBob;
    [SerializeField] private CurveControlledBob headBob = new CurveControlledBob();
    [SerializeField] private LerpControlledBob jumpBob = new LerpControlledBob();
    [SerializeField] private float stepInterval;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private Camera playerCam;
    [SerializeField] private CameraRecoil cameraRecoil;

    private bool jump;
    private Vector2 input;
    private Vector3 moveDir = Vector3.zero;
    private CollisionFlags collisionFlags;
    private bool previouslyGrounded;
    private Vector3 originalCamPos;
    private float stepCycle;
    private float nextStep;
    private bool jumping;

    // locals
    private Character player;
    private CharacterController controller;
    private AudioSource aud;

    private void Awake()
    {
        this.player = this.GetComponent<Player>();
        this.controller = this.GetComponent<CharacterController>();
        this.aud = this.GetComponent<AudioSource>();

        // restore default values
        originalCamPos = playerCam.transform.lossyScale;
        stepCycle = 0f;
        nextStep = stepCycle / 2;
        jumping = false;
    }

    private void Start()
    {
        FOVKick.Setup(playerCam);
        headBob.Setup(playerCam, stepInterval);
        mouseLook.Init(transform, playerCam.transform);
    }

    private void Update()
    {
        RotateView();

        // the jump state needs to read here to make sure it is not missed
        if (!jump)
            jump = Input.GetButtonDown("Jump");

        if (!previouslyGrounded && controller.isGrounded)
        {
            StartCoroutine(jumpBob.DoBobCycle());
            PlayLandingSound();
            moveDir.y = 0f;
            jumping = false;
        }

        if (previouslyGrounded && !controller.isGrounded && !jumping)
            moveDir.y = 0f;

        previouslyGrounded = controller.isGrounded;
    }


    private void PlayLandingSound()
    {
        player.characterBodyManager.GetBodyEffects().FootstepEvent();
        nextStep = stepCycle + .5f;
    }

    private void FixedUpdate()
    {
        GetInput(out var speed);

        Vector3 desiredMove = transform.forward * input.y + transform.right * input.x;

        // get a normal for the surface that is being touched to move along it
        Physics.SphereCast(transform.position, controller.radius, Vector3.down, out RaycastHit hitInfo,
                           controller.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
        desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

        moveDir.x = desiredMove.x * speed;
        moveDir.z = desiredMove.z * speed;

        if (controller.isGrounded)
        {
            moveDir.y = -groundForce;

            // jumping
            if (jump)
            {
                moveDir.y = player.characterStats.jumpForce;
                PlayJumpSound();
                jump = false;
                jumping = true;

                // do effects
                var desiredRot = new Vector3(15.0f, 0.0f, 0.0f);
                cameraRecoil.DoRecoil(desiredRot);
            }
        }
        else
        {
            moveDir += Physics.gravity * gravityMultiplier * Time.fixedDeltaTime;
        }
        collisionFlags = controller.Move(moveDir * Time.fixedDeltaTime);

        ProgressStepCycle(speed);
        UpdateCameraPosition(speed);

        mouseLook.UpdateCursorLock();
    }

    private void PlayJumpSound()
    {
        aud.clip = jumpSound;
        aud.Play();
    }

    private void ProgressStepCycle(float speed)
    {
        if (controller.velocity.sqrMagnitude > 0 && (input.x != 0 || input.y != 0))
        {
            stepCycle += (controller.velocity.magnitude + (speed * (isWalking ? 1f : runStepLengthen))) *
                         Time.fixedDeltaTime;
        }

        if (!(stepCycle > nextStep))
        {
            return;
        }

        nextStep = stepCycle + stepInterval;

        PlayFootStepAudio();
    }

    private void PlayFootStepAudio()
    {
        if (!controller.isGrounded)
        {
            return;
        }

        player.characterBodyManager.GetBodyEffects().FootstepEvent();
    }


    private void UpdateCameraPosition(float speed)
    {
        Vector3 newCameraPosition;
        if (!useHeadBob)
        {
            return;
        }
        if (controller.velocity.magnitude > 0 && controller.isGrounded)
        {
            playerCam.transform.localPosition =
                headBob.DoHeadBob(controller.velocity.magnitude + (speed * (isWalking ? 1f : runStepLengthen)));
            newCameraPosition = playerCam.transform.localPosition;
            newCameraPosition.y = playerCam.transform.localPosition.y - jumpBob.Offset();
        }
        else
        {
            newCameraPosition = playerCam.transform.localPosition;
            newCameraPosition.y = originalCamPos.y - jumpBob.Offset();
        }
        playerCam.transform.localPosition = newCameraPosition;
    }

    private void GetInput(out float speed)
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        bool waswalking = isWalking;

        isWalking = !Input.GetKey(KeyCode.LeftShift);
        speed = isWalking ? player.characterStats.moveSpeed : player.characterStats.GetSprintSpeed();
        input = new Vector2(horizontal, vertical);

        if (input.sqrMagnitude > 1)
        {
            input.Normalize();
        }

        // handle speed change to give an fov kick
        // only if the player is going to a run, is running and the fovkick is to be used
        if (isWalking != waswalking && useFOVKick && controller.velocity.sqrMagnitude > 0)
        {
            StopAllCoroutines();
            StartCoroutine(!isWalking ? FOVKick.FOVKickUp() : FOVKick.FOVKickDown());
        }
    }

    private void RotateView()
    {
        mouseLook.LookRotation(transform, playerCam.transform);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        //dont move the rigidbody if the character is on top of it
        if (collisionFlags == CollisionFlags.Below)
        {
            return;
        }

        if (body == null || body.isKinematic)
        {
            return;
        }
        body.AddForceAtPosition(controller.velocity * 0.1f, hit.point, ForceMode.Impulse);
    }

}