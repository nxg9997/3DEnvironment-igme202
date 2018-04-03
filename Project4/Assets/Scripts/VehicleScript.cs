using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleScript : MonoBehaviour {

	public bool IsFollower = false;
	public bool IsFlowFollower = false;
	public GameObject[] path;
	public int pathCtrl = 0;

	public Vector3 position;
	public Vector3 direction;
	public Vector3 velocity;
	public Vector3 acceleration;

	public float mass;
	public float maxSpeed;
	public float maxForce;
	public float radius;

	public float maxAvoidDistance;

	public Material fVectorColor;
	public Material rVectorColor;
	public Material futurePosVectorColor;

	public bool debugEnabled = false;

	public GameObject terrain;
	public GameObject manager;

	public float startY;

	public Vector3 resistanceCenter;
	public float resistanceRadius;

	// Use this for initialization
	protected virtual void Start () {
		//float height = terrain.GetComponent<Terrain> ().SampleHeight (transform.position);

		startY = transform.position.y;
		transform.position = new Vector3 (transform.position.x, startY, transform.position.z);
	}

	// Update is called once per frame
	void Update () {
		if (IsFollower){
			if (NextPathObject()){
				pathCtrl++;
			}
		}

		//Debug.Log ("current follow num: " + pathCtrl);
		CalcSteeringForces ();
		UpdatePosition ();
		SetTrasform ();

		float terrainheight;
	}

	protected Vector3 Seek (Vector3 targetPos) {
		Vector3 desiredVelocity = targetPos - position;
		desiredVelocity.Normalize ();
		desiredVelocity = desiredVelocity * maxSpeed;

		return (desiredVelocity - velocity);
	}

	protected Vector3 Flee (Vector3 targetPos) {
		Vector3 desiredVelocity = position - targetPos;
		desiredVelocity.Normalize ();
		desiredVelocity = desiredVelocity * maxSpeed;

		return (desiredVelocity - velocity);
	}

	protected Vector3 Pursue (Vector3 targetPos){
		return Seek (targetPos * 2f);
	}

	protected Vector3 Evade (Vector3 targetPos){
		return Flee (targetPos * 2f);
	}

	void CalcSteeringForces(){
		Vector3 ultimate = Vector3.zero;
		//ultimate += Wander ();
		//ultimate += JAvoid ();
		if (IsFollower){
			ultimate += FollowPath ();
		}
		if (IsFlowFollower){
			ultimate += FollowFlowField ();
		}
		ultimate += ResistanceField ();
		ultimate = Vector3.ClampMagnitude (ultimate, maxForce);
		//ultimate += NoOOB ();
		acceleration += ultimate;
		acceleration.y = 0;
	}

	void UpdatePosition () {
		position = gameObject.transform.position;
		velocity += acceleration;
		velocity = Vector3.ClampMagnitude (velocity, maxSpeed);
		position += velocity;
		direction = velocity.normalized;
		acceleration = Vector3.zero;
	}

	void SetTrasform () {
		gameObject.transform.forward = direction;
		position = new Vector3 (position.x, startY, position.z);
		transform.position = position;
	}

	void ApplyForce (Vector3 force) {}


	/// <summary>
	/// Method used for avoiding obstacles
	/// </summary>
	/// <returns>A vector that steers away from an obstacle</returns>
	protected Vector3 JAvoid(){
		GameObject[] gArr = GameObject.FindGameObjectsWithTag ("avoid");
		List<GameObject> currentObstacles = new List<GameObject>();

		for (int i = 0; i < gArr.Length; i++)
		{
			if(LocationCheck(gArr[i].transform.position, gArr[i].transform.forward.sqrMagnitude))
			{
				currentObstacles.Add(gArr[i]);
			}
		}

		foreach(GameObject obj in currentObstacles)
		{
			Vector3 distToObstacle = obj.transform.position - transform.position;
			float dot = Vector3.Dot(transform.right, distToObstacle);

			if (dot >= 0)
			{
				return (-1 * transform.right) * maxSpeed;
			}
			else if(dot < 0)
			{
				return transform.right * maxSpeed;
			}
		}

		return Vector3.zero;
	}

	private bool LocationCheck(Vector3 obsPosition, float obsRadius)
	{
		Vector3 futureLocation = transform.forward * maxAvoidDistance;

		Vector3 distToObstacle = obsPosition - transform.position;

		if(distToObstacle.sqrMagnitude < futureLocation.sqrMagnitude)
		{
			if(Vector3.Dot(distToObstacle, transform.right) < obsRadius + radius)
			{
				if(Vector3.Dot(distToObstacle, transform.forward) > 0)
				{
					return true;
				}
			}
		}
		return false;
	}



	public Vector3 Wander ()
	{
		Vector3 cCenter = (transform.position + transform.forward.normalized);

		float rAng = Random.Range (0f, 360f);
		//rAng = Mathf.PerlinNoise (cCenter.x, Time.deltaTime);

		Vector3 target = new Vector3 (0, 0, 1f);
		target = Quaternion.AngleAxis (rAng, Vector3.up) * target;
		Debug.Log (target);
		target.Normalize ();
		Vector3 ctarget = cCenter + target;
		ctarget.y = 0;
		//Debug.Log (ctarget);

		return Seek(ctarget);
	}

	/// <summary>
	/// Prevents vehicle from leaving the map
	/// </summary>
	/// <returns>A vector that steers towards the center of the map</returns>
	public Vector3 NoOOB (){
		if (transform.position.z >= 5f || transform.position.z <= -5f || transform.position.x >= 5f || transform.position.x <= -5f){
			return Seek (Vector3.zero);
		}
		return Vector3.zero;
	}

	public Vector3 Separation (GameObject obj){
		return Flee (obj.transform.position);
	}

	/// <summary>
	/// Method for following a set path of gameobjects
	/// </summary>
	/// <returns>A force that steers the object towards the next follow node</returns>
	public Vector3 FollowPath(){
		if (pathCtrl == 0){
			return Seek (path [pathCtrl].transform.position);
		}
		else if (pathCtrl == path.Length - 1){
			int currSeek = pathCtrl;
			return Seek (path [currSeek].transform.position);
		}
		else {
			return Seek (path [pathCtrl].transform.position);
		}
	}

	/// <summary>
	/// Determines if the gameobject can move to the next path node
	/// </summary>
	/// <returns><c>true</c>, if close to the current path node, <c>false</c> otherwise.</returns>
	bool NextPathObject(){
		if ((path[pathCtrl].transform.position - transform.position).sqrMagnitude < 20f){
			if (pathCtrl == path.Length - 1){
				pathCtrl = -1;
			}
			return true;
		}
		return false;
	}

	/// <summary>
	/// Follows the flow field.
	/// </summary>
	/// <returns>The Vector at the object's current location in the flow field</returns>
	Vector3 FollowFlowField(){
		Vector3 currFFVec;

		int currX = (int)transform.position.x / 5;
		int currZ = (int)transform.position.z / 5;

		currFFVec = manager.GetComponent<FlowFieldScript> ().flowField[currX, currZ];

		return currFFVec;
	}

	Vector3 ResistanceField(){
		Vector3 result = Vector3.zero;

		if ((transform.position - resistanceCenter).sqrMagnitude < (resistanceRadius * resistanceRadius)){
			result = velocity * -0.8f;
		}

		return result;
	}
}