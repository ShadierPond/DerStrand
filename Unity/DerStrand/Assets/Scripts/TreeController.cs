 using System;
 using System.Linq;
 using UnityEngine;
 using Random = UnityEngine.Random;

 public class TreeController : MonoBehaviour {
 
     // Player, Range
     [Header("Player Settings")]
     public int harvestTreeDistance;        // Set [Inspector] min. distance from Player to Tree for your scale?
     public bool rotatePlayer = true;    // Should we rotate the player to face the Tree? 
     private Transform myTransform;        // Player transform for cache
 
     // Terrains, Hit
     [Header("Terrain Settings")]
     private Terrain terrain;            // Derived from hit...GetComponent<Terrain>
     private RaycastHit hit;                // For hit. methods
     private string lastTerrain;            // To avoid reassignment/GetComponent on every Terrain click
 
     // Tree, GameManager
     [Header("Tree Settings")]
     // Prefab to spawn at terrain tree loc for TIIIIIIMBER!
     [SerializeField] public GameObject felledTreeMarker;
     [SerializeField] private Vector3 fellTreePositionOffset;    // Derived from hit.point
     private TreeManager rMgr;    // Resource manager script
     public float respawnTimer;            // Duration of terrain tree respawn timer
     
     [Header("Give Item")]
     [SerializeField] private Item[] items;
        [SerializeField] private int[] itemAmounts;
        [SerializeField] private int[] itemChances;
        
     public static TreeController Instance { get; private set; }

     private void Awake()
     {
         Instance = this;
     }

     private void Start () {
 
         if (harvestTreeDistance <= 0) {
             Debug.Log ("harvestTreeDistance unset in Inspector, using value: 6");
             harvestTreeDistance = 6;
         }
 
         if (respawnTimer <= 0) {
             Debug.Log ("respawnTimer unset in Inspector, using quick test value: 15");
             respawnTimer = 15;
         }
 
         myTransform = transform;
         lastTerrain = null;
         rMgr = GameManager.Instance.gameObject.GetComponent<TreeManager>();
 
     }
 
 
     public void ChopDownTree()
     {
         hit = Player.Instance._hit;
         // Did we click a Terrain?
         if(hit.collider.gameObject.GetComponent<Terrain>() == null)
             return;
 
         // Did we click on the same Terrain as last time (or very first time?)
         if(lastTerrain == null || lastTerrain != hit.collider.name) {
             terrain = hit.collider.gameObject.GetComponent<Terrain>();
             lastTerrain = terrain.name;
         }
 
         // Was it the terrain or a terrain tree, based on SampleHeight()
         var groundHeight = terrain.SampleHeight(hit.point);

         if (!(hit.point.y - .2f > groundHeight)) 
             return;
         // It's a terrain tree, check Proximity and Harvest
         if(CheckProximity()) 
             HarvestWood();
     }
 
 
     private bool CheckProximity() {
         var inRange = true;

         if (Vector3.Distance(myTransform.position, hit.point) > harvestTreeDistance) {
             Debug.Log ("Out of Range");
             inRange = false;
         }
         return inRange;
     }
     
     private bool CheckRecentUsage(string terrainName, int treeIndex) {
         var beenUsed = false;
 
         foreach (var tree in rMgr.managedTrees.Where(tree => tree.terrainName == terrainName && tree.treeINDEX == treeIndex))
         {
             Debug.Log ("Tree has been used recently");
             beenUsed = true;
         }
 
         return beenUsed;
     }
     
     private void HarvestWood() {
         var treeIndex = -1;
         var treeCount = terrain.terrainData.treeInstances.Length;
         float treeDist = harvestTreeDistance;
         var treePos = new Vector3 (0, 0, 0);

         for (var cnt=0; cnt < treeCount; cnt++) {
             var thisTreePos = Vector3.Scale(terrain.terrainData.GetTreeInstance(cnt).position, terrain.terrainData.size) + terrain.transform.position;
             var thisTreeDist = Vector3.Distance (thisTreePos, hit.point);

             if (!(thisTreeDist < treeDist)) 
                 continue;
             treeIndex = cnt;
             treeDist = thisTreeDist;
             treePos = thisTreePos;
         }
 
 
         if (treeIndex == -1) {
             Debug.Log ("Out of Range");
             return;
         }

         if (CheckRecentUsage(terrain.name, treeIndex)) 
             return;
         var marker = Instantiate(felledTreeMarker, treePos, Quaternion.identity);
         marker.transform.position = treePos + fellTreePositionOffset;

         // Add this terrain tree and cube to our Resource Manager for demo purposes
         rMgr.AddTerrainTree(terrain.name, treeIndex, Time.time+respawnTimer, marker.transform);
 
         if (rotatePlayer) {
             var lookRot = new Vector3 (hit.point.x, myTransform.position.y, hit.point.z);
             myTransform.LookAt (lookRot);
         }
         
            // Give Item
            for (int i = 0; i < items.Length; i++)
            {
                if (Random.Range(0, 100) <= itemChances[i])
                {
                    Player.Instance.inventory.AddItem(items[i], itemAmounts[i]);
                }
            }
     }
 }
