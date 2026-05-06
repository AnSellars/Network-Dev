using UnityEngine;
using Photon.Pun; 
using HashTable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using System.Text;

public class PlayerController : MonoBehaviourPunCallbacks
{
    private Transform _transform;
    public PhotonView _pv;
    private Rigidbody _rb;

    public float speed;
    public float jumpPower;
    private bool isGrounded = true; // This will help us manage jumping and prevent double jumps.

    public float bulletPower;
    public int hp;

    GameSceneManager _gm;

    [SerializeField]
    private Image hp_image;

    [SerializeField]

    private TextMeshProUGUI name_Text;

    void Start()
    {
        _transform = this.transform;
        _pv = this.gameObject.GetComponent<PhotonView>();
        _rb = this.gameObject.GetComponent<Rigidbody>();
        _gm = GameObject.Find("GameSceneManager").GetComponent<GameSceneManager>();

        // This check is slightly different in the video.
        // A better way to handle this is shown in the Update method.
        hp = 100;
        if (!_pv.IsMine)
        {
            // This prevents the NullReferenceException on all other clients.
            Destroy(this);
        }

    }

    void Update()
    {
        if (_pv.IsMine)
        {
            Control();
            //if (_transform.position.y < -5)
            //{
            //    Dead();
            //}
        }
    }

    void Control()
    {
        // --- Horizontal Movement ---
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            // Move the character left
            _transform.position += Vector3.left * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            // Move the character right
            _transform.position += Vector3.right * speed * Time.deltaTime;
        }

        // --- NEW: Vertical Movement ---
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            // Move the character up
            _transform.position += Vector3.forward * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            // Move the character down
            _transform.position += Vector3.back * speed * Time.deltaTime;
        }

        // --- JUMP: Changed from Z key to Space bar ---
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            //print("Jumping! Current velocity: " + _rb.linearVelocity);
            //// Apply an upward force for the jump. We only change the y-velocity.
            //// Note: Using _rb.velocity is correct here instead of _rb.linearVelocity. They often do the same thing in 2D.
            //_rb.linearVelocity = new Vector3(_rb.linearVelocity.x, _rb.linearVelocity.y, jumpPower);
            _rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            print("Jumping! Current velocity after jump: " + _rb.linearVelocity);
            isGrounded = false; // We are now in the air, so we set isGrounded to false.

        }

        // Example Attack Key
        if (Input.GetKeyDown(KeyCode.X))

        {
            Vector3 offset = new Vector3(0.1f, 0, 0);
            GameObject bulletObj = PhotonNetwork.Instantiate("PhotonBullet", _transform.position + offset, Quaternion.identity);
            Rigidbody brb = bulletObj.GetComponent<Rigidbody>();
            brb.AddForce(new Vector2(bulletPower, 0));
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            // We can add logic here if we want to allow double jumps or reset jump count, etc.
            // For now, we just print a message to confirm that the player has landed on the ground.
            isGrounded = true;
        }
    }


    //private void OnCollisionEnter2D(Collision2D other)
    //{
    //    // Only run this logic on the computer of the player who was hit
    //    if (_pv != null && _pv.IsMine)
    //    {
    //        // Check if the object we collided with is a bullet
    //        if (other.gameObject.CompareTag("Bullet"))
    //        {
    //            // Get the PhotonView directly from the bullet GameObject that hit us
    //            PhotonView bulletPV = other.gameObject.GetComponent<PhotonView>();

    //            // Make sure the bullet actually has a PhotonView before proceeding
    //            if (bulletPV != null)
    //            {
    //                // We only take damage if the bullet is NOT ours
    //                if (!bulletPV.IsMine)
    //                {
    //                    HashTable table = new HashTable();
    //                    hp -= 10;
    //                    table.Add("hp", hp);
    //                    PhotonNetwork.LocalPlayer.SetCustomProperties(table);

    //                    Debug.Log("Hit by an enemy bullet! HP is now " + hp);

    //                    // --- THIS IS THE CORRECTED LINE ---
    //                    // We use the bulletPV we found to get the attacker's name.
    //                    string attackerName = bulletPV.Owner.NickName;
    //                    string myName = _pv.Owner.NickName; // _pv is our own PhotonView
    //                    _gm.CallRpcSendMessageToAll($"{attackerName} hit {myName}");

    //                    if (hp <= 0)
    //                    {
    //                        PhotonNetwork.Destroy(this.gameObject);
    //                        Dead();
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}

    //public void Dead()
    //{
    //    PhotonNetwork.Destroy(this.gameObject);
    //    _gm.CallRpcLocalPlayerDead();

    //}

    //public void UpdateHpBar()
    //{
    //    float percent = (float)hp / 100;
    //    hp_image.transform.localScale = new Vector3(percent, hp_image.transform.localScale.y, hp_image.transform.localScale.z);
    //}

    //public override void OnPlayerPropertiesUpdate(Player targetPlayer, HashTable changedProps)
    //{
    //    if (targetPlayer == _pv.Owner)
    //    {

    //        hp = (int)changedProps["hp"];
    //        print(targetPlayer.NickName + ":" + hp.ToString());
    //        UpdateHpBar();
    //    }
    //}

}
