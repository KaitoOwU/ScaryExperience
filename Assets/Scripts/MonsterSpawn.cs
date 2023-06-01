using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawn : MonoBehaviour
{
    [SerializeField] Transform _player;
    [SerializeField] GameObject _prefabMonster;
    [SerializeField] float _radius;
    [SerializeField] float _size;
    [SerializeField] int _monsterCount;
    [SerializeField] List<AnimationClip> _animations;
    List<GameObject> monsters = new List<GameObject>();
    void Start()
    {
        for (int i = 0; i < _monsterCount; i++)
        {
            
            GameObject temp = Instantiate(_prefabMonster);
            
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
        monster.SetActive(true);
        monster.GetComponent<Monster>().PlayClip();
        
        StartCoroutine(SpawningCooldown(monster.GetComponent<Monster>().clip.length, monster));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_player.position, _radius);

    }
}
