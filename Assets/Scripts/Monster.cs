using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class Monster : MonoBehaviour
{
    public bool isAroundCircle;
    public float playerRadius;
    public Transform player;
    FlameManager flameManager;
    public AnimationClip clip;
    PlayableGraph playableGraph;
    AnimationPlayableOutput playableOutput;
    public MonsterSpawn monsterSpawn;
    private void Awake()
    {
        

        playableGraph = PlayableGraph.Create();

        playableGraph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);

        playableOutput = AnimationPlayableOutput.Create(playableGraph, "Animation", GetComponent<Animator>());

    }

    private void Start()
    {
        flameManager = player.GetComponent<FlameManager>();
    }
    public void PlayClip()
    {
        var clipPlayable = AnimationClipPlayable.Create(playableGraph, clip);


        // Connect the Playable to an output

        playableOutput.SetSourcePlayable(clipPlayable);

        // Plays the Graph.

        playableGraph.Play();
    }

    private void Update()
    {

        if (!isAroundCircle)
        {
            if (Vector3.Distance(player.position, transform.position) <= playerRadius)
            {
                gameObject.SetActive(false);
            }
        }
        else
        {
            if(flameManager.Value == 0)
            {
                gameObject.SetActive(false);
            }
            /*if (Vector3.Distance(player.position, transform.position) < playerRadius)
            {
                gameObject.SetActive(false);
            }*/
        }

        Vector3 dir = (transform.position - player.position).normalized;
        
        transform.localPosition = dir * (monsterSpawn._radius - 0.5f);
        
    }
}
