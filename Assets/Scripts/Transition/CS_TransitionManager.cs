using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CS_TransitionManager : MonoBehaviour {
	
	public GameObject transition;
	private Animator myAnimator;
	private string nextScene;
	
	void Start () {
		myAnimator = transition.GetComponent<Animator> ();
	}
	
	void Update () {
	}
	
	public void StartAnimationOut(string t_nextScene)
	{
//		Debug.Log ("StartAnimationOut:" + t_nextScene);
		if(t_nextScene == null)
			Debug.LogError("next scene not set!");
		nextScene = t_nextScene;

		myAnimator.SetTrigger ("fadeOut");
	}
	
	public IEnumerator GoToNextScene()
	{
		//Stop BGM
		//GameObject.Find (CS_Global.NAME_MESSAGEBOX).SendMessage ("BGMStop");
		
		AsyncOperation async = SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Single);
		yield return async;
//		Debug.Log("Loading complete");
		myAnimator.SetTrigger ("fadeIn");
	}
	
}
