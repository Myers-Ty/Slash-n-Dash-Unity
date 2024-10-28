using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Edgar.Unity.Examples.Gungeon
{
    #region codeBlock:2d_gungeon_roomManager

    public class GungeonRoomManager : MonoBehaviour
    {
        #region hide

        /// <summary>
        /// Whether the room was cleared from all the enemies.
        /// </summary>
        public bool Cleared;

        /// <summary>
        /// Whether the room was visited by the player.
        /// </summary>
        public bool Visited;

        /// <summary>
        /// Doors of neighboring corridors.
        /// </summary>
        public List<GameObject> Doors = new List<GameObject>();

        #endregion

        /// <summary>
        /// Enemies that can spawn inside the room.
        /// </summary>
        public GameObject[] Enemies;

        public GameObject[] Bosses;

        /// <summary>
        /// Whether enemies were spawned.
        /// </summary>
        public bool EnemiesSpawned;

        /// <summary>
        /// Collider of the floor tilemap layer.
        /// </summary>
        public Collider2D FloorCollider;

        #region hide

        /// <summary>
        /// Room instance of the corresponding room.
        /// </summary>
        private RoomInstanceGrid2D roomInstance;

        /// <summary>
        /// Room info.
        /// </summary>
        private GungeonRoom room;

        // My variables
        private bool JustEnteredRoom;

        public void Start()
        {
            roomInstance = GetComponent<RoomInfoGrid2D>()?.RoomInstance;
            room = roomInstance?.Room as GungeonRoom;
        }

        public void OnRoomEnter(GameObject player)
        {
            GungeonGameManager.Instance.OnRoomEnter(roomInstance);

            if (!Visited && roomInstance != null)
            {
                Visited = true;
                UnlockDoors();
            }

            if (ShouldSpawnEnemies())
            {
                // Close all neighboring doors
                CloseDoors();

                // Spawn enemies
                if ($"{(roomInstance?.Room as GungeonRoom)?.Type}".Equals("Boss"))
                {
                    SpawnBoss();
                }
                else
                {
                    SpawnEnemies();
                }
            }

            JustEnteredRoom = true; 

        }

        private void FixedUpdate()
        {
            WaitBeforeOpeningDoors();
        }

        public void OnRoomLeave(GameObject player)
        {
            GungeonGameManager.Instance.OnRoomLeave(roomInstance);
            JustEnteredRoom = false;
        }

        #endregion

        private void SpawnBoss()
        {
            EnemiesSpawned = true;

            var Boss = new List<GameObject>();
            var totalEnemiesCount = 1;

            while (Boss.Count < totalEnemiesCount)
            {
                // Find random position inside floor collider bounds
                var position = RandomPointInBounds(FloorCollider.bounds, 1f);

                // Check if the point is actually inside the collider as there may be holes in the floor, etc.
                if (!IsPointWithinCollider(FloorCollider, position))
                {
                    continue;
                }

                // We want to make sure that there is no other collider in the radius of 1
                if (Physics2D.OverlapCircleAll(position, 0.5f).Any(x => !x.isTrigger))
                {
                    continue;
                }

                // Default Boss 1, change to boss 2 if stage 2
                var enemyPrefab = Bosses[0];
                if (GungeonGameManager.Instance.Stage == 2)
                {
                    enemyPrefab = Bosses[1];
                }

                // Create an instance of the enemy and set position and parent
                var enemy = Instantiate(enemyPrefab);
                enemy.transform.position = position;
                enemy.transform.parent = roomInstance.RoomTemplateInstance.transform;
                Boss.Add(enemy);
            }
        }

        private void SpawnEnemies()
        {
            EnemiesSpawned = true;

            var enemies = new List<GameObject>();
            // More enemies for second floor
            var totalEnemiesCount = GungeonGameManager.Instance.Random.Next(3, 5);
            if (GungeonGameManager.Instance.Stage == 2)
            {
                totalEnemiesCount = GungeonGameManager.Instance.Random.Next(5, 8);
            }

            while (enemies.Count < totalEnemiesCount)
            {
                // Find random position inside floor collider bounds
                var position = RandomPointInBounds(FloorCollider.bounds, 1f);

                // Check if the point is actually inside the collider as there may be holes in the floor, etc.
                if (!IsPointWithinCollider(FloorCollider, position))
                {
                    continue;
                }

                // We want to make sure that there is no other collider in the radius of 1
                if (Physics2D.OverlapCircleAll(position, 0.5f).Any(x => !x.isTrigger))
                {
                    continue;
                }

                // Pick random enemy prefab
                var enemyPrefab = Enemies[Random.Range(0, Enemies.Length)];

                // Create an instance of the enemy and set position and parent
                var enemy = Instantiate(enemyPrefab);
                enemy.transform.position = position;
                enemy.transform.parent = roomInstance.RoomTemplateInstance.transform;
                enemies.Add(enemy);
            }
        }

        private static bool IsPointWithinCollider(Collider2D collider, Vector2 point)
        {
            return collider.OverlapPoint(point);
        }

        private static Vector3 RandomPointInBounds(Bounds bounds, float margin = 0)
        {
            return new Vector3(
                Random.Range(bounds.min.x + margin, bounds.max.x - margin),
                Random.Range(bounds.min.y + margin, bounds.max.y - margin),
                Random.Range(bounds.min.z + margin, bounds.max.z - margin)
            );
        }

        #region hide

        /// <summary>
        /// Wait some time before before opening doors.
        /// </summary>
        private void WaitBeforeOpeningDoors()
        {

            if (transform.childCount == 1 && JustEnteredRoom)
            {
                OpenDoors();
            }
        }

        /// <summary>
        /// Close doors before we spawn enemies.
        /// </summary>
        private void CloseDoors()
        {
            foreach (var door in Doors)
            {
                if (door.GetComponent<GungeonDoor>().State == GungeonDoor.DoorState.EnemyLocked)
                {
                    door.SetActive(true);
                }
            }
        }

        /// <summary>
        /// Open doors that were closed because of enemies.
        /// </summary>
        private void OpenDoors()
        {
            foreach (var door in Doors)
            {
                if (door.GetComponent<GungeonDoor>().State == GungeonDoor.DoorState.EnemyLocked)
                {
                    door.SetActive(false);
                }
            }
        }

        /// <summary>
        /// Unlock doors that were locked because there is a shop or reward room on the other end.
        /// </summary>
        private void UnlockDoors()
        {
            if (room.Type == GungeonRoomType.Reward || room.Type == GungeonRoomType.Shop)
            {
                foreach (var door in Doors)
                {
                    if (door.GetComponent<GungeonDoor>().State == GungeonDoor.DoorState.Locked)
                    {
                        door.GetComponent<GungeonDoor>().State = GungeonDoor.DoorState.Unlocked;
                    }
                }
            }
        }

        /// <summary>
        /// Check if we should spawn enemies based on the current state of the room and the type of the room.
        /// </summary>
        /// <returns></returns>
        private bool ShouldSpawnEnemies()
        {
            return Cleared == false && EnemiesSpawned == false && (room.Type == GungeonRoomType.Normal || room.Type == GungeonRoomType.Hub || room.Type == GungeonRoomType.Boss);
        }

        #endregion
    }

    #endregion
}