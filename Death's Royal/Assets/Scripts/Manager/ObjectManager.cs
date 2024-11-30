using Google.Protobuf.Protocol;
using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectManager
{
	public MyTPController MyPlayer { get; set; }
	Dictionary<int, GameObject> _objects = new Dictionary<int, GameObject>();

	public void Add(Google.Protobuf.Protocol.PlayerInfo info, bool myPlayer = false)
	{
		if (myPlayer)
		{
			GameObject go = Managers.Resource.Instantiate("MyPlayer");
			go.name = "MyPlayer";
			_objects.Add(info.PlayerID, go);

			MyPlayer = go.GetComponent<MyTPController>();
			go.GetComponent<MyTPSController>().enabled = true;
			MyPlayer.Id = info.PlayerID;
			MyPlayer.transform.position = new Vector3
				(info.PosInfo.Pos.X, info.PosInfo.Pos.Y, info.PosInfo.Pos.Z);
        }
		else
		{
            GameObject go = Managers.Resource.Instantiate("Player");
            go.name = info.Name;
            _objects.Add(info.PlayerID, go);

            ThirdPersonController pc = go.GetComponent<ThirdPersonController>();
            pc.Id = info.PlayerID;
			Vector2 randomVec = new Vector2(Random.Range(-4.0f, 4.0f), Random.Range(-4.0f, 4.0f));
            pc.transform.position = new Vector3
				(info.PosInfo.Pos.X + randomVec.x,
                info.PosInfo.Pos.Y + randomVec.y,
                info.PosInfo.Pos.Z);
        }
	}

	public void Add(int id,GameObject go)
	{
		_objects.Add(id, go);
	}

	public void Remove(int id)
	{
		_objects.Remove(id);
	}

	public void RemoveMyPlayer()
	{
		if (MyPlayer == null)
			return;

		Remove(MyPlayer.Id);
		MyPlayer = null;
	}

	public GameObject FindById(int id)
	{
		GameObject go = null;
		_objects.TryGetValue(id, out go);
		return go;
	}

	public GameObject Find(Vector3Int cellPos)
	{
		foreach (GameObject obj in _objects.Values)
		{
			//CreatureController cc = obj.GetComponent<CreatureController>();
			//if (cc == null)
			//	continue;
		}

		return null;
	}

	public GameObject Find(Func<GameObject, bool> condition)
	{
		foreach (GameObject obj in _objects.Values)
		{
			if (condition.Invoke(obj))
				return obj;
		}

		return null;
	}

	public void Clear()
	{
		_objects.Clear();
	}
}
