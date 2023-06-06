using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MonsterSpawn : MonoBehaviour
{
    [SerializeField] Transform _player;
    [SerializeField] GameObject _prefabMonster;
    public float _radius;
    [SerializeField] float height;
    [SerializeField] float width;
    [SerializeField] Material litMaterial;

    [Header("Monster Eyes")]
    [SerializeField] int _monsterEyesCount;
    [SerializeField] List<AnimationClip> _animationsEyes;
    List<GameObject> monstersEyes = new List<GameObject>();

    [Header("Monster Cricle")]
    [SerializeField] int _monsterCircleCount;
    [SerializeField] List<AnimationClip> _animationsCircle;
    List<GameObject> monstersCircle = new List<GameObject>();


    AudioManager _audioManager;

    [HideInInspector] public bool playerIsDead = false;

    private void Awake()
    {
        _audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    public void StartSpawn()
    {
        
        for (int i = 0; i < _monsterEyesCount; i++)
        {
            
            GameObject temp = Instantiate(_prefabMonster);
            monstersEyes.Add(temp);
            temp.GetComponent<Monster>().playerRadius = _radius;
            temp.GetComponent<Monster>().player = _player;
            temp.GetComponent<Monster>().isAroundCircle = false;
            temp.GetComponent<Monster>().monsterSpawn = this;
            Spawn(temp);
            temp.SetActive(false);
        }

        for (int i = 0; i < _monsterCircleCount; i++)
        {

            GameObject temp = Instantiate(_prefabMonster, _player);
            monstersCircle.Add(temp);
            temp.GetComponent<Monster>().playerRadius = _radius;
            temp.GetComponent<Monster>().player = _player;
            temp.GetComponent<Monster>().isAroundCircle = true;
            temp.GetComponent<SpriteRenderer>().material = litMaterial;
            temp.GetComponent<Monster>().monsterSpawn = this;
            Spawn(temp);
            temp.SetActive(false);
        }
        _audioManager.PlayGlbGlbSound();
    }

    IEnumerator SpawningCooldown(float time, GameObject monster)
    {

        yield return new WaitForSeconds(time);
        
        monster.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        Spawn(monster);
    }



    public void Spawn(GameObject monster)
    {
        if (!playerIsDead)
        {
            if (!monster.GetComponent<Monster>().isAroundCircle)
            {
                monster.GetComponent<Monster>().clip = _animationsEyes[Random.Range(0, _animationsEyes.Count)];
                monster.transform.position = new Vector3(Random.Range(_player.position.x - width / 2, _player.position.x + width / 2), Random.Range(_player.position.y - height / 2, _player.position.y + height / 2), 0);

                CheckIfInCirecle(_player, monster.transform, _radius);
            }
            else
            {
                monster.GetComponent<Monster>().clip = _animationsCircle[Random.Range(0, _animationsCircle.Count)];
                SpawnAroundCircle(monster);
            }
            monster.SetActive(true);

            monster.GetComponent<Monster>().PlayClip();
            StartCoroutine(SpawningCooldown(monster.GetComponent<Monster>().clip.length, monster));
        }
        

    }

    public void RefreshCircle()
    {
        foreach (GameObject monster in monstersCircle)
        {
            monster.SetActive(false);
            SpawningCooldown(0.5f, monster);
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_player.position, _radius);
        Vector3 A = new Vector3(_player.position.x - width, _player.position.y + height);
        Vector3 B = new Vector3(_player.position.x + width, _player.position.y + height);
        Vector3 C = new Vector3(_player.position.x + width, _player.position.y - height);
        Vector3 D = new Vector3(_player.position.x - width, _player.position.y - height);
        Gizmos.DrawLine(A, B);
        Gizmos.DrawLine(B, C);
        Gizmos.DrawLine(C, D);
        Gizmos.DrawLine(D, A);

    }

    void CheckIfInCirecle(Transform center, Transform other, float radius)
    {
        Vector3 dir = (other.position - center.position).normalized;
        float distValue = Vector3.Distance(center.position, other.position);
        if(distValue < radius)
        {
            Vector3 pointOnSquare = dir * 500;
            pointOnSquare.x = Mathf.Clamp(pointOnSquare.x, center.position.x - width / 2, center.position.x + width / 2);
            pointOnSquare.y = Mathf.Clamp(pointOnSquare.y, center.position.y - height / 2, center.position.y + height / 2);

            float distTemp = Vector3.Distance(other.position, pointOnSquare);
            float deltaRadius = radius - distValue;
            other.position += dir * Random.Range(deltaRadius, distTemp);
        }
    }

    void SpawnAroundCircle(GameObject monster)
    {

        monster.transform.localPosition = Vector3.zero;
        float angleDeg = Random.Range(-180, 180);
        float angleRad = Mathf.Deg2Rad * angleDeg;
        Vector3 pointOnCircle = new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad)) * (_radius - 0.5f);
        monster.transform.position += pointOnCircle;

        //Rotation
        Vector3 objToPlayer = monster.transform.position - _player.position;
        float rotationAngle = Mathf.Atan2(objToPlayer.y, objToPlayer.x);
        rotationAngle *= Mathf.Rad2Deg;
        monster.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90 + rotationAngle));

    }

    

}
