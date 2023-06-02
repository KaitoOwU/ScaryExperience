using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawn : MonoBehaviour
{
    [SerializeField] Transform _player;
    [SerializeField] GameObject _prefabMonster;
    public float _radius;
    [SerializeField] float _size;
    [SerializeField] int _monsterCount;
    [SerializeField] List<AnimationClip> _animations;
    List<GameObject> monsters = new List<GameObject>();
    void Start()
    {
        for (int i = 0; i < _monsterCount; i++)
        {
            
            GameObject temp = Instantiate(_prefabMonster);
            monsters.Add(temp);
            Spawn(temp);
            temp.SetActive(false);
        }
        
    }

    IEnumerator SpawningCooldown(float time, GameObject monster)
    {

        yield return new WaitForSeconds(time);

        monster.SetActive(false);
        Spawn(monster);
    }



    public void Spawn(GameObject monster)
    {
        monster.GetComponent<Monster>().clip = _animations[Random.Range(0, _animations.Count)];
        monster.transform.position = new Vector3(Random.Range(_player.position.x - _size / 2, _player.position.x + _size / 2), Random.Range(_player.position.y - _size / 2, _player.position.y + _size / 2), 0);
        CheckIfInCirecle(_player, monster.transform, _radius);
        foreach (GameObject temp in monsters)
        {
            CheckIfInCirecle(monster.transform, temp.transform, monster.GetComponent<Monster>().monsterRadius);
        }
        monster.SetActive(true);
        monster.GetComponent<Monster>().PlayClip();
        
        StartCoroutine(SpawningCooldown(monster.GetComponent<Monster>().clip.length, monster));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_player.position, _radius);

    }

    void CheckIfInCirecle(Transform center, Transform other, float radius)
    {

        Vector3 dir = (other.position - center.position).normalized;
        float distValue = Vector3.Distance(center.position, other.position);
        if(distValue < radius)
        {
            Vector3 pointOnSquare = dir * 500;
            pointOnSquare.x = Mathf.Clamp(pointOnSquare.x, center.position.x - _size / 2, center.position.x + _size / 2);
            pointOnSquare.y = Mathf.Clamp(pointOnSquare.y, center.position.y - _size / 2, center.position.y + _size / 2);

            float distTemp = Vector3.Distance(other.position, pointOnSquare);
            float deltaRadius = radius - distValue;
            other.position += dir * Random.Range(deltaRadius, distTemp);
        }
    }
}
