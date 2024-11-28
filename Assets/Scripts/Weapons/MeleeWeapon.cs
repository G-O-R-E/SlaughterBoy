using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using Unity.VisualScripting;

public class MeleeWeapon : MonoBehaviour
{
    [Header("Weapon Parameter")]
    [SerializeField] List<GameObject> listEnemy;
    [SerializeField] public int baseRange;
    [SerializeField] public int baseDamage;
    [SerializeField] public float baseWeaponCooldown;

    private float weaponCooldown = 1.5f;
    private float weaponMaxCooldown = 1.5f;
    private int id = 0;
    private int damage = 0;
    private bool areStatsSet = false;
    private bool weaponCanAttack = true;

    public bool isAttacking = false;
    public bool isMovingForward = true;
    private float travelSpeed = 40f;
    private Vector3 basePosition;
    private Vector3 weaponOffset;
    private Vector3 target;

    [Header("Sound Parameter")]
    [SerializeField] AudioMixerGroup group;
    [SerializeField] AudioClip[] clip;
    private AudioSource source;

    void Start()
    {
        source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
        source.outputAudioMixerGroup = group;
        source.loop = false;
    }


    void Update()
    {
        basePosition = transform.parent.parent.position + weaponOffset;
        FlipWeaponY();

        if (!areStatsSet)
        {
            SetStatsWeapon();
            areStatsSet = true;
        }

        if (listEnemy.Count != 0 && !isAttacking)
        {
            SortListEnemy();
            WeaponTracking();
        }

        HitEnemies();

        if (!weaponCanAttack)
        {
            weaponCooldown -= Time.deltaTime;
            if (weaponCooldown <= 0)
            {
                weaponCooldown = weaponMaxCooldown;
                weaponCanAttack = true;
                isMovingForward = true;
            }
        }

        listEnemy.Clear();
    }

    private void ChangeWeaponPosition()
    {
        float radius = 1f;
        float angle = (60f * (float)-id) * Mathf.Deg2Rad;
        weaponOffset = new Vector3((float)Mathf.Cos(angle) * radius, (float)Mathf.Sin(angle) * radius, 0);
        transform.position += weaponOffset;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            collision.gameObject.TryGetComponent<EnemyStatistic>(out EnemyStatistic component);
            if (component != null && component.hasSpawned)
            {
                listEnemy.Add(collision.gameObject);
            }
        }
    }

    private int SortEnemiesByLength(GameObject _enemy1, GameObject _enemy2)
    {
        float length1 = Length2D(transform.position, _enemy1.GetComponent<MonoBehaviour>().transform.position);
        float length2 = Length2D(transform.position, _enemy2.GetComponent<MonoBehaviour>().transform.position);

        if (length1 < length2)
        {
            return -1;
        }
        else if (length1 > length2)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    private void SortListEnemy()
    {
        listEnemy.Sort(SortEnemiesByLength);
    }

    private void WeaponTracking()
    {
        // Weapon rotation to aim at nearest enemy

        Vector3 vectorToTarget = listEnemy[0].gameObject.transform.position - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * 200f);
    }

    private void HitEnemies()
    {
        if (weaponCanAttack && listEnemy.Count != 0 && !isAttacking)
        {
            Vector3 vectorToTarget = listEnemy[0].gameObject.transform.position - transform.position;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = q;

            target = basePosition + vectorToTarget.normalized * transform.GetComponent<CircleCollider2D>().radius;
            isAttacking = true;
        }

        if (isAttacking)
        {
            if (isMovingForward)
            {
                transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * travelSpeed);
                if (transform.position == target)
                {
                    //sound
                    int rndClipFire = Random.Range(0, clip.Length);
                    source.clip = clip[rndClipFire];
                    source.Play();

                    isMovingForward = false;
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, basePosition, Time.deltaTime * travelSpeed);
                if (transform.position == basePosition)
                {
                    //sound
                    int rndClipFire = Random.Range(0, clip.Length);
                    source.clip = clip[rndClipFire];
                    source.Play();

                    isAttacking = false;
                    weaponCanAttack = false;
                }
            }
        }
    }

    private float Length2D(Vector2 _v1, Vector2 _v2)
    {
        Vector2 v3 = _v2 - _v1;
        return v3.magnitude;
    }

    private void SetStatsWeapon()
    {
        Stats stats = GameObject.Find("Player").GetComponentInChildren<Stats>();

        int range = baseRange + stats.GetScope();
        if (range < 1) range = 1;
        GetComponent<CircleCollider2D>().radius = range;

        damage = baseDamage + stats.GetAttack() + stats.GetCloseDamage();
        weaponMaxCooldown = baseWeaponCooldown - baseWeaponCooldown * (stats.GetAttackSpeed() / 100);
        weaponCooldown = weaponMaxCooldown;
    }

    private void FlipWeaponY()
    {
        if (transform.rotation.eulerAngles.z > 90 && transform.rotation.eulerAngles.z < 270)
        {
            GetComponent<SpriteRenderer>().flipY = true;
        }
        else
        {
            GetComponent<SpriteRenderer>().flipY = false;
        }
    }

    public void InitializeWeapon(int _id)
    {
        id = _id;
        ChangeWeaponPosition();
    }

    public int GetDamage()
    {
        return damage;
    }
}