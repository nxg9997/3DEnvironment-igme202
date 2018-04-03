using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockerScript : MonoBehaviour {

	public float bounds = 50;

	public Vector3 position;
	public Vector3 direction;
	public Vector3 velocity;
	public Vector3 acceleration;

	public float mass;
	public float maxSpeed;
	public float maxForce;
	public float radius;

	public float maxAvoidDistance;

	public GameObject terrain;

	private GameObject[] flock;

	public GameObject manager;

	public float startY;
	public bool CanFollowFF = true;

	// Use this for initialization
	void Start () {
		flock = GameObject.FindGameObjectsWithTag ("flocker");
		terrain = GameObject.Find("Terrain");
		startY = transform.position.y;
		//manager = GameObject.Find ("manager");
	}
	
	// Update is called once per frame
	void Update () {
		CalcSteeringForces ();
		UpdatePosition ();
		SetTransform ();
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

	void UpdatePosition () {
		position = gameObject.transform.position;
		velocity += acceleration;
		velocity = Vector3.ClampMagnitude (velocity, maxSpeed);
		position += velocity;
		direction = velocity.normalized;
		acceleration = Vector3.zero;
	}

	void SetTransform () {
		gameObject.transform.forward = direction;
		position = new Vector3 (position.x, startY, position.z);
		transform.position = position;
	}

	void CalcSteeringForces(){
		Vector3 ultimate = Vector3.zero;

		//ultimate += NoOOB();
		ultimate += Separation () * 2f;
		ultimate += Align ();
		ultimate += Cohesion ();
		ultimate += FollowFlowField () * 0.05f;

		ultimate = Vector3.ClampMagnitude (ultimate, maxForce);
		acceleration += ultimate;
		//AvoidTerrain(0);
	}

	/// <summary>
	/// Prevents flock from moving out of bounds
	/// </summary>
	/// <returns>The OO.</returns>
	Vector3 NoOOB(){
		if (position.x > bounds || position.x < -bounds || position.z > bounds || position.z < -bounds){
			return Seek (/*new Vector3 (0, 30, 0)*/manager.GetComponent<ManagerScript>().currOOBTarget);
		}

		return Vector3.zero;
	}

	/// <summary>
	/// Keeps individual flockers away from each other
	/// </summary>
	Vector3 Separation(){
		Vector3 result = Vector3.zero;

		for (int i = 0; i < flock.Length; i++){
			if (Mathf.Abs((transform.position - flock[i].transform.position).sqrMagnitude) < 25f){
				result += Flee (flock [i].transform.position);
			}
		}

		return result;
	}

	/// <summary>
	/// Points flockers in the average direction of the rest of the flock
	/// </summary>
	Vector3 Align(){
		Debug.Log ("in align");
		/*Vector3 avgDir = Vector3.zero;

		for (int i = 0; i < flock.Length; i++){
			avgDir += flock [i].GetComponent<FlockerScript> ().direction;
		}

		avgDir.Normalize ();*/




		return Seek(transform.position + manager.GetComponent<ManagerScript>().avgDir);
	}

	/// <summary>
	/// Keeps flock together
	/// </summary>
	Vector3 Cohesion(){
		/*float avgX = 0;
		float avgY = 0;
		float avgZ = 0;

		for (int i = 0; i < flock.Length; i++){
			avgX += flock [i].transform.position.x;
			avgY += flock [i].transform.position.y;
			avgZ += flock [i].transform.position.z;
		}

		avgX /= flock.Length;
		avgY /= flock.Length;
		avgZ /= flock.Length;

		Vector3 flockCenter = new Vector3 (avgX, avgY, avgZ);*/



		return Seek(manager.GetComponent<ManagerScript>().flockCenter);
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

	protected Vector3 Avoid(){
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

	/// <summary>
	/// Avoids the terrain, this version of the method doesn't work
	/// </summary>
	/// <returns>The terrain.</returns>
	Vector3 AvoidTerrain (){
		/*RaycastHit rch;

		if (Physics.Raycast(transform.position, transform.forward, out rch)){
			return Flee (rch.point);
		}*/
		float tHeight = terrain.GetComponent<Terrain>().SampleHeight (transform.position);
		Vector3 fleeVector = new Vector3 (transform.position.x, tHeight, transform.position.y);

		if (Mathf.Abs((transform.position - fleeVector).sqrMagnitude) < maxAvoidDistance * maxAvoidDistance){
			return Flee (fleeVector);
		}



		return Vector3.zero;
	}

	/// <summary>
	/// Avoids the terrain.
	/// </summary>
	/// <returns>The terrain.</returns>
	/// <param name="NaN">Used to designate this version of AvoidTerrain</param>
	Vector3 AvoidTerrain (int NaN){
		Debug.Log ("in AT");
		Vector3 result = Vector3.zero;
		float tHeight = terrain.GetComponent<Terrain>().SampleHeight (transform.position);

		Vector3 fleeVec = new Vector3(transform.position.x + transform.forward.x * 2f, tHeight, transform.position.z + transform.forward.z * 2f);
		if (Mathf.Abs((transform.position - fleeVec).sqrMagnitude) < maxAvoidDistance * maxAvoidDistance){
			Vector3 fleeResult = Flee (fleeVec);
			result = new Vector3 (0, Mathf.Abs (fleeResult.y), 0);
		}


		return result * 2f;
	}

	/// <summary>
	/// Follows the flow field.
	/// </summary>
	/// <returns>The Vector at the object's current location in the flow field</returns>
	Vector3 FollowFlowField(){
		Vector3 currFFVec = Vector3.zero;


		int currX = (int)transform.position.x / 5;
		int currZ = (int)transform.position.z / 5;

		if (currX > 100 || currZ > 100 || currX < 0 || currZ < 0 || !CanFollowFF){
			/*for (int i = 0; i < 100; i++){
				for (int j = 0; j < 100; j++){
					manager.GetComponent<FlowFieldScript> ().flowField [i, j] *= -1f;
				}
			}

			if (currX > 100){ currX = 100;}
			if (currX < 0){ currX = 0;}
			if (currZ > 100){ currZ = 100;}
			if (currZ < 0){ currZ = 0;}*/
			currFFVec = Seek (new Vector3 (150, startY, 250));
			CanFollowFF = false;
		}
		else if (CanFollowFF){
			currFFVec = manager.GetComponent<FlowFieldScript> ().flowField[currX, currZ];
		}


		return currFFVec;
	}

	void EnterFFF(){
		if ((manager.GetComponent<ManagerScript>().flockCenter - new Vector3(250, startY, 250)).sqrMagnitude < 2500f){
			CanFollowFF = true;
		}
	}
}
