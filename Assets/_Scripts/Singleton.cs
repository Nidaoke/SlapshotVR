using UnityEngine;
using System.Collections;
using System.Linq;

public class Singleton : MonoBehaviour
{
	private static Singleton _instance = null;
	public bool Reset = false;

	////////////////////////////////////////////////

	// properties & events

	public static Singleton instance { get { return _instance; } }

	////////////////////////////////////////////////

	public abstract class Manager : MonoBehaviour
	{
		public abstract void InitInstance();
	}

	public class Manager<T> : Manager where T : class
	{
		public static T inst;
		public override void InitInstance()
		{
			inst = this as T;
		}
	}

	////////////////////////////////////////////////

	// GameObject implements managed references, so the null test fails if the object is destroyed
	void Awake()
	{
		if (instance != null)
		{
			Debug.LogWarning("Singleton already exists, deleting copy");
			Destroy(this.gameObject);
		}
		_instance = this;
		DontDestroyOnLoad(this);
		InitSingletons();
	}

	void InitSingletons()
	{
		foreach (Manager manager in FindObjectsOfType<GameObject>().SelectMany(go => go.GetComponents(typeof(Manager))))
		{
			manager.InitInstance();
		}
	}

	void OnLevelWasLoaded(int level)
	{
		InitSingletons();
	}

	void Update()
	{
		if (_instance == null || Reset)
		{
			gameObject.SetActive(false);
			gameObject.SetActive(true);
			Reset = false;
		}
	}
}
