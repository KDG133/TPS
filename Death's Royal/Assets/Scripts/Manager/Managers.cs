using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_instance;
    static Managers Instance { get { Init(); return s_instance; } }

    ObjectManager _obj = new ObjectManager();
    NetworkManager _network = new NetworkManager();
    SpawnManager _spawn = new SpawnManager();
    EffectManager _effect = new EffectManager();
    SoundManager _sound = new SoundManager();
    UpgradeManager _upgrade = new UpgradeManager();
    PoolManager _pool = new PoolManager();
    ResourceManager _resource = new ResourceManager();
    SceneManagerEx _scene = new SceneManagerEx();

    public static ObjectManager Object { get { return Instance._obj; } }
    public static NetworkManager Network { get { return Instance._network; } }
    public static SpawnManager Spawn { get { return Instance._spawn; } }
    public static EffectManager Effect { get { return Instance._effect; } }
    public static SoundManager Sound { get { return Instance._sound; } }
    public static UpgradeManager Upgrade { get { return Instance._upgrade; } }
    public static PoolManager Pool { get { return Instance._pool; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    public static SceneManagerEx Scene { get { return Instance._scene; } }

    void Start()
    {
        Init();
	}

    void Update()
    {
        _network.Update();
        _upgrade.Update();
    }

    static void Init()
    {
        if (s_instance == null)
        {
			GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Managers>();

            s_instance._network.Init();
            s_instance._sound.Init();
            s_instance._spawn.Init();
            s_instance._effect.Init();
            s_instance._upgrade.Init();
            s_instance._pool.Init();
        }		
	}

    public static void Clear()
    {
        //Sound.Clear();
        //Scene.Clear();
        //UI.Clear();
        Pool.Clear();
    }
}
