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
			ChangeGunName(go.transform, go.name);
            _objects.Add(info.PlayerID, go);

			MyPlayer = go.GetComponent<MyTPController>();
			go.GetComponent<MyTPSController>().enabled = true;
			MyPlayer.Id = info.PlayerID;
            Vector2 randomVec = new Vector2(Random.Range(-3f, 3f), Random.Range(-3f, 3f));
            MyPlayer.transform.position = new Vector3
				(info.PosInfo.Pos.X + randomVec.x,
				info.PosInfo.Pos.Y,
				info.PosInfo.Pos.Z + randomVec.y);
			SyncPos(MyPlayer.transform);
        }
		else
		{
            GameObject go = Managers.Resource.Instantiate("Player");
            go.name = info.Name;
            ChangeGunName(go.transform, go.name);
            _objects.Add(info.PlayerID, go);

            ThirdPersonController pc = go.GetComponent<ThirdPersonController>();
            pc.Id = info.PlayerID;
			Vector2 randomVec = new Vector2(Random.Range(-3f, 3f), Random.Range(-3f, 3f));
            pc.transform.position = new Vector3
				(info.PosInfo.Pos.X + randomVec.x,
                info.PosInfo.Pos.Y,
                info.PosInfo.Pos.Z + randomVec.y);
			SyncPos(MyPlayer.transform);
        }
	}

	private void SyncPos(Transform targerTrans)
	{
        C_Move movePacket = new C_Move()
        {
            PosInfo = new PositionInfo() { Pos = new PVector3() }
        };
        movePacket.PosInfo.Pos.X = targerTrans.position.x;
        movePacket.PosInfo.Pos.Y = targerTrans.position.y;
        movePacket.PosInfo.Pos.Z = targerTrans.position.z;
        movePacket.PosInfo.MoveDir = targerTrans.rotation.eulerAngles.y;
        movePacket.PosInfo.MoveSpeed = 0;
        Managers.Network.Send(movePacket);
    }

	public void Remove(int id)
	{
		GameObject go = FindById(id);
		if(go == null) return;

		_objects.Remove(id);
		Managers.Resource.Destroy(go);
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

	public GameObject Find(Func<GameObject, bool> condition)
	{
		foreach (GameObject obj in _objects.Values)
		{
			if (condition.Invoke(obj))
				return obj;
		}

		return null;
	}

	public void ChangeGunName(Transform parent, string addName)
	{
        for (int i = 0; i < (int)GunType.End; ++i)
        {
            foreach (Transform child in Util.FindChildWithTag(parent, Enum.GetName(typeof(GunType), i).ToUpper()).transform)
                child.name = addName + "_" + child.name;
        }
    }

	public void Clear()
	{
		foreach (GameObject obj in _objects.Values)
			Managers.Resource.Destroy(obj);
        _objects.Clear();
	}
}
