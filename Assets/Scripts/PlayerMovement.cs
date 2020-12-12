using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    Animation anim;

    public Animator SkinAnimator;
    public GameManager GM;
    CapsuleCollider selfCollider;
    Rigidbody rb;

    public GameObject Vfx;

    public Transform PlayerTransf;

    public float JumpSpeed = 12;

    public float SideSpeed;

    bool isRolling = false;
    bool falling = false;

    Vector3 ccCenterNorm = new Vector3(0, 1f, 0),
            ccCenterRoll = new Vector3(0, .2f, 0),
            StartPlayerPos;
    Vector3 rbVelocity;

    float ccHeightNorm = 2,
          ccHeightRoll = .4f;

    bool wannaJump = false;

    void Start()
    {
        selfCollider = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();

        SwipeController.SwipeEvent += CheckInput;

        if (Advertisement.isSupported)
        {
            Advertisement.Initialize("3935565", false);
        }
       
    }

    public void Pause()
    {
        rbVelocity = rb.velocity;
        rb.isKinematic = true;
        SkinAnimator.speed = 0;
    }

    public void UnPause()
    {
        rb.isKinematic = false;
        rb.velocity = rbVelocity;
        SkinAnimator.speed = 1;
    }    

    private void OnDestroy()
    {
        SwipeController.SwipeEvent -= CheckInput;
    }

    private void FixedUpdate()
    {
        Debug.DrawRay(transform.position, Vector3.down * 0.05f);
        rb.AddForce(new Vector3(0, Physics.gravity.y * 4, 0), ForceMode.Acceleration);

        if (wannaJump && isGrounded())
        {
            
            SkinAnimator.SetTrigger("jumping");
            rb.AddForce(new Vector3(0, JumpSpeed, 0), ForceMode.Impulse);
            wannaJump = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrounded())
            SkinAnimator.ResetTrigger("falling");
        else if (rb.velocity.y<-8)
            SkinAnimator.SetTrigger("falling");

        SkinAnimator.SetBool("falling", rb.velocity.y < -8);   

        Vector3 Pos = transform.position;
        Pos.z = 0;
        transform.position = Pos;
    }

    void CheckInput(SwipeController.SwipeType type)
    {
        if (isGrounded() && GM.CanPlay && !isRolling)
        {           
            if (type == SwipeController.SwipeType.UP)
                        wannaJump = true;
            else if (type == SwipeController.SwipeType.DOWN)
                        StartCoroutine(DoRoll());                
        }

    }

    bool isGrounded()
    {
        return Physics.Raycast(PlayerTransf.position, Vector3.down, 0.05f);
    }

    IEnumerator DoRoll()
    {
        isRolling = true;
        SkinAnimator.SetBool("rolling", true);
       
        selfCollider.center = ccCenterRoll;
        selfCollider.height = ccHeightRoll;

        yield return new WaitForSeconds(1);
                
        SkinAnimator.SetBool("rolling", false);

        selfCollider.center = ccCenterNorm;
        selfCollider.height = ccHeightNorm;

        yield return new WaitForSeconds(.3f);

        isRolling = false;
    }

    IEnumerator JumpFalling()
    {
        falling = true;
        SkinAnimator.SetBool("falling", true);
        Debug.Log("falling");

        yield return new WaitForSeconds(.5f);

        falling = false;
        SkinAnimator.SetBool("falling", false);
        Debug.Log("falling");
    }




    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Finish")
        {
            StartCoroutine(Finish());
            return;
        }

        if ((!collision.gameObject.CompareTag("Trap") &&
             !collision.gameObject.CompareTag("DeathPlane")) ||
            !GM.CanPlay)
            return;


        if (GM.CanPlay == false)
        {
            return;
        }

        if (collision.gameObject.CompareTag("Trap"))
        {
            var vfx = Instantiate(Vfx, collision.gameObject.transform.position, Quaternion.identity);
            Destroy(vfx, 1);
            Destroy(collision.gameObject);
        }


        StartCoroutine(GameOver());
    }

    IEnumerator Finish()
    {
        GM.CanPlay = false;
        yield return new WaitForSeconds(.1f);
        GM.ShowFinish();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Coin5"))
            return;
        GM.AddCoins(5);
        Destroy(other.gameObject);
    }



    IEnumerator GameOver()
    {
        GM.CanPlay = false;

        SkinAnimator.SetTrigger("death");

        yield return new WaitForSeconds(2);

        SkinAnimator.ResetTrigger("death");
        if (Advertisement.IsReady())
        {
            Advertisement.Show("video");
        }


        GM.ShowResult();
       
        
    }

}
