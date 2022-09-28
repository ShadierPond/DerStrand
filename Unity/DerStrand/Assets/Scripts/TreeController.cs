 using System.Linq;
 using UnityEngine;
 public class TreeController : MonoBehaviour {
 
     // Player, Range
     public int harvestTreeDistance;        // Set [Inspector] min. distance from Player to Tree for your scale?
     public bool rotatePlayer = true;    // Should we rotate the player to face the Tree? 
     private Transform myTransform;        // Player transform for cache
 
     // Terrains, Hit
     private Terrain terrain;            // Derived from hit...GetComponent<Terrain>
     private RaycastHit hit;                // For hit. methods
     private string lastTerrain;            // To avoid reassignment/GetComponent on every Terrain click
 
     // Tree, GameManager
     public GameObject felledTree;        // Prefab to spawn at terrain tree loc for TIIIIIIMBER!
     private TreeManager rMgr;    // Resource manager script
     public float respawnTimer;            // Duration of terrain tree respawn timer
 
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
 
 
     private void Update ()
     {
         if (!Input.GetMouseButtonUp(0)) 
             return;
         var ray = Camera.main.ScreenPointToRay (Input.mousePosition);
         if (!Physics.Raycast(ray, out hit, 30f)) 
             return;
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
         var clickDist = Vector3.Distance (myTransform.position, hit.point);
 
         if (clickDist > harvestTreeDistance) {
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
                 
         // Notice we are looping through every terrain tree, 
         // which is a caution against a 15,000 tree terrain
 
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

         var marker = GameObject.CreatePrimitive (PrimitiveType.Cube);
         marker.transform.position = treePos;
 
         // Example of spawning a placed tree at this location, just for demo purposes
         // it will slide through terrain and disappear in 4 seconds
         var fellTree = Instantiate(felledTree,treePos,Quaternion.identity) as GameObject;
         fellTree.gameObject.AddComponent<Rigidbody>();
 
         Destroy(fellTree,4);
 
         // Add this terrain tree and cube to our Resource Manager for demo purposes
         rMgr.AddTerrainTree(terrain.name, treeIndex, Time.time+respawnTimer, marker.transform);
 
         if (rotatePlayer) {
             var lookRot = new Vector3 (hit.point.x, myTransform.position.y, hit.point.z);
             myTransform.LookAt (lookRot);
         }

         foreach (var item in Player.Instance.inventory.database.items)
         {
             if(item.name == "Wood")
             {
                 Player.Instance.inventory.AddItem(item, 1);
             }
         }
     }
 }
