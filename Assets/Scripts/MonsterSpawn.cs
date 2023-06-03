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
    [SerializeField] int _monsterCount;
    [SerializeField] List<AnimationClip> _animations;
    [SerializeField] List<GameObject> monsters = new List<GameObject>();
    void Start()
    {
        for (int i = 0; i < _monsterCount; i++)
        {
            
            GameObject temp = Instantiate(_prefabMonster);
            monsters.Add(temp);
            temp.GetComponent<Monster>().playerRadius = _radius;
            temp.GetComponent<Monster>().player = _player;
            Spawn(temp);
            temp.SetActive(false);
        }
        
    }

    IEnumerator SpawningCooldown(float time, GameObject monster)
    {

        yield return new WaitForSeconds(time);
        float alpha = monster.GetComponent<SpriteRenderer>().color.a;
        DOTween.To(() => alpha, x => alpha = x, 0, 0.1f).SetEase(Ease.OutExpo);
        monster.SetActive(false);
        
        Spawn(monster);
    }



    public void Spawn(GameObject monster)
    {
        monster.GetComponent<Monster>().clip = _animations[Random.Range(0, _animations.Count)];
        monster.transform.position = new Vector3(Random.Range(_player.position.x - width / 2, _player.position.x + width / 2), Random.Range(_player.position.y - height / 2, _player.position.y + height / 2), 0);
        
        /*foreach (GameObject temp in monsters)
        {
            if (temp != monster)
            {
                CheckIfInCirecle(monster.transform, temp.transform, 2);
            }

        }*/
        CheckIfInCirecle(_player, monster.transform, _radius);
        monster.SetActive(true);
        float alpha = monster.GetComponent<SpriteRenderer>().color.a;
        DOTween.To(() => alpha, x => alpha = x, 255, 0.1f).SetEase(Ease.OutExpo);
        monster.GetComponent<Monster>().PlayClip();
        
        StartCoroutine(SpawningCooldown(monster.GetComponent<Monster>().clip.length, monster));
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
}
