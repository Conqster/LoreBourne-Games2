using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;
using System;


namespace LoreBourne.AI
{
    enum Shape { cube, sphere};

    public class NPCPostCreation : MonoBehaviour
    {
        [Header("Post Properties")]
        [SerializeField] private GameObject ground;
        [SerializeField, Range(0,100)] private float postCreationAreaRadius;
        [Tooltip("Type in a string name of the ground is tag with!!!")]
        [SerializeField] private string groundTag;
        private Vector3 postCreationCenter;
        [SerializeField, Range(0,10)] private float debugRadius;
        [SerializeField, Range(1, 50)] private int numberOfPosts;
        [SerializeField] private List<Vector3> postNodes;
        private bool canCreatePosts = false;    /// used to create post when node have been stored in list
        private Vector3 hitPoint;          // point at which the post creation ray hit the ground

        [Space][Space]
        [Header("Modifier")]
        [SerializeField] private bool generatePostPrefab = false;
        [SerializeField] private bool drawNodePathTarget = false;
        [SerializeField] private bool drawSafeZoneRayTarget = false;
        [Tooltip("A reference to the prefab of post, if there is one!!!")]
        [SerializeField] private GameObject prefabPost;
        [SerializeField] private bool drawHeightPost = false;

        //[SerializeField] private bool generateCollider;

        [Space][Space]
        [Header("AI Properties")]
        [SerializeField, Range(1, 15)] private float npcHeight = 1.8f;
        [SerializeField, Range(1, 15)] private float rayLength;
        [SerializeField] private Transform target;

        [Space][Space]
        [Header("Testing Testing")]
        [SerializeField] private List<GameObject> gameObjectInRegion;       //used to store object in the range of creator
        private int currentNumberOfPosts;
        private float currentPostCreationAreaRadius;
        private bool postRay;
        private NavMeshPath path;
        [SerializeField] private bool postHeightDrew = false;



        private void Start()
        {
            //GetGround();
            currentNumberOfPosts = numberOfPosts;
        }



        private void Update()
        {
            CheckGroundForPosts();
            PostCreationProperties();
        }

        

        private void CheckGroundForPosts()
        {
            RaycastHit[] hits;

            Ray ray = new Ray(transform.position, -transform.up);
            hits = Physics.SphereCastAll(ray, 5f, Mathf.Infinity);

            foreach (RaycastHit hit in hits)
            {
                GameObject objectHit = hit.transform.gameObject;

                if(objectHit.tag == groundTag.ToString())
                {
                    ground = objectHit;
                    Vector3 myPoint = hit.transform.position;
                    hitPoint = myPoint;

                }


                if (!gameObjectInRegion.Contains(objectHit) && objectHit != gameObject)
                {
                    gameObjectInRegion.Add(objectHit);
                }
            }
        }

        private void PostCreationProperties()
        {
            if(hitPoint != null)
            {
                Vector3 creatorPos = transform.position;            //gets the position of the cretor gameObject

                postCreationCenter = new Vector3(creatorPos.x, hitPoint.y, creatorPos.z);

                //CreatePosts(postCreationCenter, numberOfPosts);
                if(numberOfPosts != currentNumberOfPosts || postNodes.Count == 0)
                {
                    bool itsEven = (numberOfPosts % 2 == 0) ? true : false;
                    
                    if(itsEven && postCreationAreaRadius != 0)
                    {
                        postNodes.Clear();
                        GeneratePostsPosition(postCreationCenter, numberOfPosts);

                        if (canCreatePosts)
                        {
                            if(generatePostPrefab)
                                CreatePost(postNodes);

                            canCreatePosts = false;
                        }
                    }
                    else
                    {
                        print("numberOfPost have to be even");
                    }
                }
            }
        }

        private void GeneratePostsPosition(Vector3 point, int numberOfNodes)
        {
            Vector3 centerOfCreation = point;
            float halfLengthOfWidth = postCreationAreaRadius * 0.5f;
            Vector3 edgeOfPostArea = centerOfCreation  - (new Vector3(halfLengthOfWidth, centerOfCreation.y, halfLengthOfWidth));
            Vector3 firstPost = edgeOfPostArea + new Vector3(debugRadius, centerOfCreation.y, debugRadius);

            int sumOfNodes = numberOfNodes;
            int numVerticalNode = sumOfNodes / 2;
            int numHorizontalNode = sumOfNodes / 2;
            float numOfPostInRow = numVerticalNode;
            float lengthAlongRow = postCreationAreaRadius - (debugRadius * 2);    // removing the padding
            float distanceBtwPost = lengthAlongRow / (numOfPostInRow - 1);

            if(postNodes.Count < sumOfNodes)
            {
                for (int v = 0; v < numVerticalNode; v++)
                {
                    for (int h = 0; h < numHorizontalNode; h++)
                    {
                        Vector3 currentPost = firstPost + new Vector3(v * distanceBtwPost, 0, h * distanceBtwPost);
                        postNodes.Add(currentPost);
                    }
                }
            }
            canCreatePosts = true;
            currentNumberOfPosts = numberOfPosts;
            currentPostCreationAreaRadius = postCreationAreaRadius;
        }

        private List<Vector3> PostForHeight(List<Vector3> postCollection)
        {
            List<Vector3> heightPost = new List<Vector3>();
            foreach(Vector3 post in postCollection)
            {
                Vector3 height = post;
                height.y += npcHeight;
                heightPost.Add(height);
            }
            return heightPost;
        }




        #region Playing with Coroutine On Post Height Creation
        private void DrawPostHeight()
        {
            List<Vector3> postToDraw = new List<Vector3>();
            postToDraw = PostForHeight(postNodes);
            drawHeightPost = false;
            StartCoroutine(StartDraw(DrawCompelete, postToDraw));
        }

        private void DrawCompelete()
        {
            postHeightDrew = true;
        }

        private IEnumerator StartDraw(Action onComplete, List<Vector3> postHeight)
        {
            postHeightDrew = false;
            if(postHeight.Count >= 0 && !postHeightDrew)
            {
                List<Vector3> postToDraw = new List<Vector3>();
                postToDraw = postHeight;
                int times = 0;
                foreach (Vector3 post in postToDraw)
                {
                    times++;
                    print("excuted times: " + times);
                    //postToDraw.Remove(post);
                    yield return DrawPost(post);
                }

            }

            yield return new WaitForEndOfFrame();
            onComplete.Invoke();
        }

        private IEnumerator DrawPost(Vector3 post)
        {
            Gizmos.color = Color.black;
            //Handles.DrawSolidDisc(post, Vector3.up, debugRadius);
            //Gizmos.DrawWireSphere(post, debugRadius);
            GameObject postObject = Instantiate(prefabPost, post, Quaternion.identity);
            List<Color> colour = new List<Color>()
            {

                Color.black,
                Color.blue,
                Color.clear,
                Color.cyan,
                Color.gray,
                Color.green,
                Color.magenta,
                Color.red,
                Color.white,
                Color.yellow
            };
            Material objMat = postObject.GetComponent<Renderer>().material;
            System.Random rnd = new System.Random();
            int chooseColour = rnd.Next(0, colour.Count);
            objMat.color = colour[chooseColour];
            float metallicIntensity = rnd.Next(0,10);
            float smoothIntensity = rnd.Next(0,10);
            //print(smoothIntensity/10);
            objMat.SetFloat("_Metallic", metallicIntensity / 10);
            //print(objMat.GetFloat("_Metallic"));
            objMat.SetFloat("_Smoothness", 1);
            postObject.transform.SetParent(transform);
            yield return new WaitForEndOfFrame();
        }


        #endregion

        private void CreatePost(List<Vector3> nodes)
        {

            foreach(Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            foreach(Vector3 node in nodes)
            {
                if(prefabPost != null)
                {
                    GameObject post = Instantiate(prefabPost, node, Quaternion.identity);
                    post.transform.SetParent(transform);
                }
                else
                {
                    print("If you what to create an object for the post, add a prefab to 'prefabPost' ");
                }
            }
        }

        private Dictionary<Vector3, bool> IsPostWalkable(List<Vector3> posts, bool value)
        {
            Dictionary<Vector3, bool> safePost = new Dictionary<Vector3, bool>();

            foreach (Vector3 node in posts)
            {
                if(numberOfPosts > 3)
                {
                    path = new NavMeshPath();
                    postRay = NavMesh.CalculatePath(node, target.position, NavMesh.AllAreas, path);
                    
                    NavMeshHit hit;
                    bool safePostRay = NavMesh.Raycast(node, target.position, out hit, NavMesh.AllAreas);

                    //// still testing 
                    safePost.Add(node, safePostRay);
                    if(drawNodePathTarget && value)
                    {
                        if(postRay)
                        {
                            for (int i = 0; i < path.corners.Length - 1; i++)
                                Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
                        }
                        else
                        {
                            Debug.DrawLine(node, target.position, postRay ? Color.red : Color.black);
                        }

                    }
                    else if(drawSafeZoneRayTarget && value)
                    {
                         Debug.DrawLine(node, target.position, safePostRay ? Color.blue : Color.red);
                    }

                    if(postRay && value)
                    {

                        Gizmos.color = safePostRay ? Color.blue : Color.red;
                        Gizmos.DrawWireSphere(node, debugRadius);
                    }
                    else if(!postRay && value)
                    {
                        Gizmos.color = Color.cyan;
                        Gizmos.DrawWireSphere(node, debugRadius);
                    }
                }
            }

            return safePost;

        }


        private void BestFightingPosition()
        {
            //Vector3 targetPosition;
            //GameObject obstaclesInRay;
        }

        private void OnDrawGizmos()
        {
            if(currentPostCreationAreaRadius == postCreationAreaRadius)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(transform.position, 
                    new Vector3(postCreationAreaRadius, 2f, postCreationAreaRadius));
            }
            else
            {
                //draw current
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(transform.position, 
                    new Vector3(currentPostCreationAreaRadius, 2f, currentPostCreationAreaRadius));

                // draw future
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireCube(transform.position,
                    new Vector3(postCreationAreaRadius, 2f, postCreationAreaRadius));
            }


            if (hitPoint != null)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawSphere(postCreationCenter, debugRadius);
            }


            if(postNodes != null)
            {
                IsPostWalkable(postNodes, true);
            }

            if(drawHeightPost)
            {
                DrawPostHeight();
            }

        }



        /// <summary>
        /// A Function that return a dictionary with post and true for safe position
        /// </summary>
        /// <returns></returns>
        public Dictionary<Vector3, bool> SafePost()
        {
            Dictionary<Vector3, bool> safePost;

            safePost = IsPostWalkable(postNodes, false);

            return safePost;
        }

    }

}
