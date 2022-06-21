using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Database;
using Google.MiniJSON;
using Models;
using Network;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    private DatabaseReference _database;

    [SerializeField] private GameObject remotePlayerPrefab;

    [SerializeField] private Rigidbody2D _playerMovement;
    private DatabaseReference _localPlayerRef;

    private readonly Dictionary<string, NetworkedPlayer> _connectedPlayersData = new();
    private readonly Dictionary<string, RemotePlayer> _remotePlayers = new();

    private void Awake()
    {
        _database = FirebaseDatabase.DefaultInstance.RootReference;
        _localPlayerRef = _database.Child("players").Push();


        _database.Child("players").ChildChanged += OnChildChanged;
    }

    private void OnChildChanged(object sender, ChildChangedEventArgs e)
    {
        var player = e.Snapshot.Key;

        var data = JsonUtility.FromJson<NetworkedPlayer>(e.Snapshot.GetRawJsonValue());
        if (player == _localPlayerRef.Key) return;

        _connectedPlayersData[player] = data;

        if (_remotePlayers.ContainsKey(player)) return;

        var remotePlayerGameObject = Instantiate(remotePlayerPrefab);
        _remotePlayers[player] = remotePlayerGameObject.GetComponent<RemotePlayer>();
    }

    private void FixedUpdate()
    {
        foreach (var (key, snapshot) in _connectedPlayersData)
        {
            if (!_remotePlayers.ContainsKey(key)) continue;

            var remotePlayerGameObject = _remotePlayers[key];
            remotePlayerGameObject.UpdatePosition(snapshot);
        }
    }

    private async void LateUpdate()
    {
        await _localPlayerRef.SetRawJsonValueAsync(JsonUtility.ToJson(new NetworkedPlayer(_playerMovement.position, false)));
    }
}