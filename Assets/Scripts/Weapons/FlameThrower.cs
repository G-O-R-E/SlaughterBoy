using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class FlameThrower : MonoBehaviour
{
    [Header("Weapon Parameter")]
    [SerializeField] List<GameObject> listEnemy;
    [SerializeField] GameObject prefabBullet;
    [SerializeField] public int baseRange;
    [SerializeField] public int baseDamage;

    private int id = 0;
    private float damage = 0;
    private bool areStatsSet = false;

    GameObject flame;

    void Start()
    {
        listEnemy = new List<GameObject>();
        flame = Instantiate(prefabBullet, transform.GetChild(0).position, transform.rotation);
    }


    void Update()
    {
        FlipWeaponY();

        if (!areStatsSet)
        {
            SetWeaponStats();
            areStatsSet = true;
        }

        if (listEnemy.Count != 0)
        {
            SortListEnemy();
            WeaponTracking();            
        }
        ShootEnemies();


        listEnemy.Clear();
    }

    private void ChangeWeaponPosition()
    {
        float radius = 1f;
        float angle = (60f * (float)-id) * Mathf.Deg2Rad;
        Vector3 vector = new Vector3((float)Mathf.Cos(angle) * radius, (float)Mathf.Sin(angle) * radius, 0);
        transform.position += vector;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<EnemyStatistic>())
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

        Vector3 vectorToTarget = listEnemy[0].GetComponent<MonoBehaviour>().transform.position - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * 200f);
    }

    private void ShootEnemies()
    {
        if (flame)
        {
            ParticleSystem ps = flame.GetComponent<ParticleSystem>();
            if (ps)
            {
                Quaternion rotation = new Quaternion();
                rotation.eulerAngles = new Vector3(0, 0, -90 + transform.eulerAngles.z);

                SetDirectionParticle(flame.GetComponent<ParticleSystem>(), transform.rotation);
                flame.GetComponent<Flame>().SetDamage(damage);

            }
        }       
       
    }

    private float Length2D(Vector2 _v1, Vector2 _v2)
    {
        Vector2 v3 = _v2 - _v1;
        return v3.magnitude;
    }

    private void SetDirectionParticle(ParticleSystem ps, Quaternion quaternion)
    {
        ps.transform.position = transform.position;
        ps.transform.rotation = quaternion;
    }

    private void SetWeaponStats()
    {
        Stats stats = GameObject.Find("Player").GetComponentInChildren<Stats>();

        int range = baseRange + stats.GetScope();
        if (range < 1) range = 1;
        GetComponent<CircleCollider2D>().radius = range;

        damage = baseDamage + stats.GetAttack() + stats.GetDistanceDamage();
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

    public float GetDamage()
    {
        return damage;
    }
}
