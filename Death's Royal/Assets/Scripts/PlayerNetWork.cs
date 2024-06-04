using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetWork : NetworkBehaviour
{
    [SerializeField] private Transform spawnObjectPrefabs;
    [SerializeField] private Transform spawnedObjectTransform;

    private NetworkVariable<MyCustomData> randomNumber = new NetworkVariable<MyCustomData>(
        new MyCustomData { iHP = 1, isBool = false },
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);

    public struct MyCustomData : INetworkSerializable {
        public int iHP;
        public bool isBool;
        public FixedString128Bytes message;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref iHP);
            serializer.SerializeValue(ref isBool);
            serializer.SerializeValue(ref message);
        }
    }

    public override void OnNetworkSpawn()
    {
        randomNumber.OnValueChanged += (MyCustomData prevValue, MyCustomData newValue) => {
            Debug.Log(OwnerClientId + "; " + newValue.iHP + "; " + newValue.isBool + "; " + newValue.message);
        };
    }

    // Update is called once per frame
    void Update()
    {
        if(!IsOwner) return;

        if (Input.GetKeyDown(KeyCode.T))
        {
            spawnedObjectTransform = Instantiate(spawnObjectPrefabs);
            spawnedObjectTransform.GetComponent<NetworkObject>().Spawn(true);
            //randomNumber.Value = new MyCustomData { iHP = Random.Range(1, 30), isBool = true, message = "Hello World!" };
            //TestServerRPC(new ServerRpcParams());
            //TestClientRPC(new ClientRpcParams { Send = new ClientRpcSendParams { TargetClientIds = new List<ulong> { 1 } } });
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            spawnedObjectTransform.GetComponent<NetworkObject>().Despawn(true);
        }

        Vector3 vDir = new Vector3(0, 0, 0);

        if(Input.GetKey(KeyCode.W)) vDir.z += 1f;
        if(Input.GetKey(KeyCode.S)) vDir.z -= 1f;
        if(Input.GetKey(KeyCode.A)) vDir.x -= 1f;
        if(Input.GetKey(KeyCode.D)) vDir.x += 1f;

        float moveSpeed = 3f;
        transform.position += vDir * moveSpeed * Time.deltaTime;
    }

    [ServerRpc]
    private void TestServerRPC(ServerRpcParams serverRpcParams)
    {
        Debug.Log("TestServerRPC " + OwnerClientId + "; " + serverRpcParams.Receive.SenderClientId);
    }

    [ClientRpc]
    private void TestClientRPC(ClientRpcParams clientRpcParams)
    {
        Debug.Log("TestClientRPC");
    }
}
