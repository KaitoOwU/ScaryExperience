using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class Monster : MonoBehaviour
{
    public float monsterRadius;
    public AnimationClip clip;
    PlayableGraph playableGraph;
    AnimationPlayableOutput playableOutput;
    private void Awake()
    {
        playableGraph = PlayableGraph.Create();

        playableGraph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);

        playableOutput = AnimationPlayableOutput.Create(playableGraph, "Animation", GetComponent<Animator>());

    }
    public void PlayClip()
    {
        var clipPlayable = AnimationClipPlayable.Create(playableGraph, clip);


        // Connect the Playable to an output

        playableOutput.SetSourcePlayable(clipPlayable);

        // Plays the Graph.

        playableGraph.Play();
    }

}
