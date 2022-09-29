 using System;
 using System.Linq;
 using UnityEngine;
 using Random = UnityEngine.Random;

 public class TreeController : MonoBehaviour {
 
     // Player, Range
     [Header("Player Settings")]
     public int harvestTreeDistance;        // Set [Inspector] min. distance from Player to Tree for your scale?
     public bool rotatePlayer = true;    // Should we rotate the player to face the Tree? 

     // Terrains, Hit
     [Header("Terrain Settings")]
     private Terrain terrain;            // Derived from hit...GetComponent<Terrain>
     private RaycastHit hit;                // For hit. methods
     private string lastTerrain;            // To avoid reassignment/GetComponent on every Terrain click
 
     // Tree, GameManager
     [Header("Tree Settings")]
     // harvested Tree marker
     [SerializeField] public GameObject felledTreeMarker;
     [SerializeField] private Vector3 fellTreePositionOffset;    // Derived from hit.point
     private TreeManager rMgr;    // Resource manager script
     public float respawnTimer;            // Duration of terrain tree respawn timer
     
     [Header("Give Item")]
     [SerializeField] private Item[] items;
        [SerializeField] private int[] itemAmounts;
        [SerializeField] private int[] itemChances;
        
     // Public access to the Class
     public static TreeController Instance { get; private set; }

     private void Awake()
     {
         Instance = this;
     }

     private void Start () {
         // If the harvest distance is not set, set it to 6
         if (harvestTreeDistance <= 0) {
             //Debug.Log ("harvestTreeDistance unset in Inspector, using value: 6");
             harvestTreeDistance = 6;
         }
         // If the respawn timer is not set, set it to 15
         if (respawnTimer <= 0) {
             //Debug.Log ("respawnTimer unset in Inspector, using quick test value: 15");
             respawnTimer = 15;
         }
         // Set the last terrain to null
         lastTerrain = null;
         // Get the TreeManager script
         rMgr = GameManager.Instance.gameObject.GetComponent<TreeManager>();
 
     }
 
 
     public void ChopDownTree()
     {
         // Get Raycast hit from Player Camera
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
     
     // Check if the Player is close enough to the Tree
     private bool CheckProximity() {
         // If the Player is not close enough to the Tree, return false
         var inRange = !(Vector3.Distance(gameObject.transform.position, hit.point) > harvestTreeDistance);
         return inRange;
     }
     
     // Check if Tree has been harvested recently
     private bool CheckRecentUsage(string terrainName, int treeIndex) {
         var beenUsed = false;
 
         foreach (var tree in rMgr.managedTrees.Where(tree => tree.terrainName == terrainName && tree.treeINDEX == treeIndex))
         {
             //Debug.Log ("Tree has been used recently");
             beenUsed = true;
         }
 
         return beenUsed;
     }
     
     // Harvest the Tree
     private void HarvestWood() {
         var treeIndex = -1;
         var treeCount = terrain.terrainData.treeInstances.Length;
         float treeDist = harvestTreeDistance;
         var treePos = new Vector3 (0, 0, 0);
        
         // For each tree in the terrain
         for (var cnt=0; cnt < treeCount; cnt++) {
             // Get the tree position
             var thisTreePos = Vector3.Scale(terrain.terrainData.GetTreeInstance(cnt).position, terrain.terrainData.size) + terrain.transform.position;
             // Get the distance from the tree to the hit point
             var thisTreeDist = Vector3.Distance (thisTreePos, hit.point);
            // If this tree is closer than the last tree
             if (!(thisTreeDist < treeDist)) 
                 continue;
             // Set the Tree Index
             treeIndex = cnt;
             // Set the Tree Distance
             treeDist = thisTreeDist;
             // Set the Tree Position
             treePos = thisTreePos;
         }
         
         // if the tree index is still -1 (no tree) , return
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
             var lookRot = new Vector3 (hit.point.x, gameObject.transform.position.y, hit.point.z);
             gameObject.transform.LookAt (lookRot);
         }
         
            // Give Item to Player
            for (int i = 0; i < items.Length; i++)
            {
                if (Random.Range(0, 100) <= itemChances[i])
                {
                    Player.Instance.inventory.AddItem(items[i], itemAmounts[i]);
                }
            }
     }
 }
