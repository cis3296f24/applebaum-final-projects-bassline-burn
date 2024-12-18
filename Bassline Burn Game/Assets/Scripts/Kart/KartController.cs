using System;
using Fusion;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Collections;


public class KartController : KartComponent
{
	public new CapsuleCollider2D collider;

	public Transform model;

	public float maxSpeedNormal;
	public float acceleration;
	//public float driftFactor;

	[Tooltip("X-Axis: steering\nY-Axis: velocity\nCoordinate space is normalized")]


	public float maxSteerStrength = 35;
	public float speedToDrift;

	public Rigidbody2D rb;
	public int lapCount { get; private set; } = 1;

	public bool HasFinishedRace => Kart.LapController.EndRaceTick != 0;
	public bool HasStartedRace => Kart.LapController.StartRaceTick != 0;
	private float RealSpeed => transform.InverseTransformDirection(rb.velocity).z;

	[Networked] public float MaxSpeed { get; set; }

	[Networked] public RoomPlayer RoomUser { get; set; }

	[Networked] public float AppliedSpeed { get; set; }

	[Networked] private KartInput.NetworkInputData Inputs { get; set; }


	[Networked] private float SteerAmount { get; set; }
	[Networked] private int AcceleratePressedTick { get; set; }
	[Networked] private bool IsAccelerateThisFrame { get; set; }

	private Radio radio;
	public GameObject radioControlPad;
	public GameObject finishScreen;
	private bool inRadioMenu;
	public float rotationAngle = 0;
	public int currentStation = 0;

	public float moveInput;
    public float turnInput;
	public float menuTimerMax;
	public float menuTimerCurr;
	public GameObject[] radioUI;
	[Networked] private Vector2 Position { get; set; }
	[Networked] private float Rotation { get; set; }
	public bool CanDrive = false;

	private HashSet<int> checkpointsPassed = new HashSet<int>();
	private int totalCheckpoints = 6;
	private int highestCheckpointPassed = 0;
	private bool finished { get; set; } = false;

	[Networked] public bool IsRaceFinished { get; private set; } = false;


	public float velocityVsUp = 0;
	public float someMaxSpeed = 0;
	public float base_acceleration = 5f;
	public float boostMultiplier;

	public float base_boostAcceleration;
	public float turnFactor = 3.5f;
    public float driftFactor = 0.95f;
	public float currentBoostTime = 0;
	public float maxBoostTime = 3f;
	public float boostVal = 15f;

	public float baseSpeed = 10f;

	CarSurfaceHandler carSurfaceHandler;

	private void Awake()
	{
		collider = GetComponent<CapsuleCollider2D>();
		carSurfaceHandler = GetComponent<CarSurfaceHandler>();
		
	}

	public override void Spawned()
	{
		base.Spawned();
		radio = GetComponent<Radio>();
		if (Object.HasInputAuthority)
        {
            
        }


	}

	private void Update(){
		if (Inputs.IsDownThisFrame(KartInput.NetworkInputData.ButtonRadio))
		{
			Debug.Log("test");
			currentStation = (currentStation + 1) % radioUI.Length;
			radio.NavigateStations(true);
			
		}

		
	}

	
    public override void FixedUpdateNetwork()
	{
		base.FixedUpdateNetwork();

		if (IsRaceFinished)
		{
			rb.velocity = Vector2.zero; // Ensure the car remains stopped
			return; // Prevent further updates
		}

		if (GetInput(out KartInput.NetworkInputData input))
		{
			Inputs = input;
		}

		if (Inputs.IsDown(KartInput.NetworkInputData.ButtonRadio))
		{
			//Debug.Log("test");
			if(currentStation == 0){
				currentStation = 1;
			}
			else{
				currentStation = 0;
			}
			Debug.Log($"currentStation {currentStation}");
			//currentStation = (currentStation + 1) % radioUI.Length;
			radio.NavigateStations(true);
			
		}

		if (Inputs.Boost != 0 && currentBoostTime > 0){
			
			boostMultiplier = 3f;
            someMaxSpeed = boostVal;
            currentBoostTime = Mathf.Clamp(currentBoostTime - Time.deltaTime * 2f, 0, maxBoostTime);
			Debug.Log($"currentBoostTime {currentBoostTime}");
			
			
		}
		else{
			boostMultiplier = 1f;
			someMaxSpeed = baseSpeed;
			if(currentBoostTime < maxBoostTime){
				currentBoostTime += Time.deltaTime;
			}
			ChangeStats(radio.currentStation);

			//Debug.Log($"boostMultiplier {boostMultiplier}");
			
			
		}

		if (GameManager.Instance.raceStart)
		{
			Move(Inputs);
			Steer(Inputs);

			if (Object.HasStateAuthority)
			{
				Position = rb.position;
				Rotation = rb.rotation;
			}
			else
			{
				rb.position = Vector2.Lerp(rb.position, Position, Time.deltaTime * 10);
				rb.rotation = Mathf.LerpAngle(rb.rotation, Rotation, Time.deltaTime * 10);
			}
		}

		HandleStartRace();
		KillOrthagonalVelocity();
	}


    private void HandleStartRace()
    {
        if (!HasStartedRace && Track.Current != null && Track.Current.StartRaceTimer.Expired(Runner))
        {
            var components = GetComponentsInChildren<KartComponent>();
            foreach (var component in components) component.OnRaceStart();
        }
    }

    public override void OnRaceStart()
    {
        base.OnRaceStart();
		CanDrive = true;
		
        if (Object.HasInputAuthority)
        {
            AudioManager.PlayMusic(Track.Current.music);
        }
    }

    private void Move(KartInput.NetworkInputData input)
	{
		moveInput = input.Accelerate;

		float velocityVsUp = Vector2.Dot(transform.up, rb.velocity);

		if (Object.HasStateAuthority)
		{
			if (velocityVsUp > someMaxSpeed && moveInput > 0)
				moveInput = 0;
			if (velocityVsUp < -someMaxSpeed * 0.5f && moveInput < 0)
				moveInput = 0;


			
			if (moveInput != 0)
			{
				rb.drag = Mathf.Lerp(rb.drag,0,Time.fixedDeltaTime*10);
				switch(GetSurface()){
					case Surface.SurfaceTypes.Offroad: 
						rb.drag = Mathf.Lerp(rb.drag,9.0f,Time.fixedDeltaTime*3);
						break;
					case Surface.SurfaceTypes.Hazard:
						rb.drag = 0;
						moveInput = Mathf.Clamp(moveInput, 0, 1.0f);
						break;
       		 	}
			
			}
			else
			{
				// Apply drag when there's no input
				//ApplyDrag();
				//rb.drag = Mathf.Lerp(rb.drag,0,Time.fixedDeltaTime*10);
				rb.drag = Mathf.Lerp(rb.drag,3.0f,Time.fixedDeltaTime*3);
			}

			

			
			Vector2 engineForce = transform.up * moveInput * acceleration;
			rb.AddForce(engineForce, ForceMode2D.Force);
			Position = rb.position;

		}
	}


	private void ApplyDrag()
	{
		float dragCoefficient = 0.3f; // Adjust this value for stronger or weaker drag
		Vector2 velocity = rb.velocity;

		// Apply a force opposite to the velocity to simulate drag
		Vector2 dragForce = -velocity.normalized * velocity.sqrMagnitude * dragCoefficient;

		// Add drag force to the Rigidbody2D
		rb.AddForce(dragForce, ForceMode2D.Force);
	}

	private void Steer(KartInput.NetworkInputData input)
	{
		turnInput = input.Steer;

		if (finished){
			return;
		}

		if (Object.HasStateAuthority)
		{
			// float steerAmount = turnInput * maxSteerStrength;
			// rotationAngle -= steerAmount * speedToDrift;
			// rb.MoveRotation(rotationAngle);
			float minSpeedForTurn = rb.velocity.magnitude/8;
        	minSpeedForTurn = Mathf.Clamp01(minSpeedForTurn);
        	rotationAngle -= turnInput * turnFactor * minSpeedForTurn;
        	rb.MoveRotation(rotationAngle);
			Rotation = rb.rotation; // Sync rotation
		}
	}



	void KillOrthagonalVelocity(){
        Vector2 forwardVelocity = transform.up * Vector2.Dot(rb.velocity,transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(rb.velocity,transform.right);

        rb.velocity = forwardVelocity + rightVelocity * driftFactor;
        
    }

	public void ResetState()
	{
		rb.velocity = Vector3.zero;
		AppliedSpeed = 0;
		transform.up = Vector3.up;
		model.transform.up = Vector3.up;
	}
	public void OnTriggerEnter2D(Collider2D other) {
		if(other.gameObject.name == "Finish"){
			if(HasInputAuthority)
			{
				if (CheckpointsComplete())
				{
					if (lapCount >= 3) // Adjust maxLaps based on your TrackDefinition
					{
						lapCount++;
						CompleteRace();
					}
					else
					{
						lapCount++;
						ResetCheckpoints(); // Reset for the next lap
					}
				}
			}else{
				// finished = true;
			}
		}
	}

	public bool CheckpointsComplete()
{
    return checkpointsPassed.Count == totalCheckpoints;
}


	public void CompleteRace()
	{
		if (!IsRaceFinished)
		{
			IsRaceFinished = true; // Mark race as finished for this player
			rb.velocity = Vector2.zero; // Stop the car
			rb.angularVelocity = 0; // Stop any rotation
		}
	}



	public void SetTotalCheckpoints(int count){
		totalCheckpoints = count;
	}

	public void OnCheckpointCrossed(int checkpointID)
	{
		// Ensure this is for the local player's kart
		if (!Object.HasInputAuthority) return;

		if (!checkpointsPassed.Contains(checkpointID))
		{
			checkpointsPassed.Add(checkpointID);
			if (checkpointID > highestCheckpointPassed)
			{
				highestCheckpointPassed = checkpointID;
			}

			if (CheckpointsComplete())
			{
				Debug.Log($" now finish the lap!");
			}
		}
	}
	public float GetVelocityMagnitude(){
        return rb.velocity.magnitude;
    }

	float GetLateralVelocity(){
        return UnityEngine.Vector2.Dot(transform.right, rb.velocity);
    }
	public bool IsTireSchreeching(out float lateralVelocity, out bool isBoosting, out bool isBraking){
        lateralVelocity = GetLateralVelocity();
        isBoosting = false;
        isBraking = false;

        if(moveInput < 0 && velocityVsUp >0){
            isBraking = true;
            return true;
        }


        if (Inputs.Boost != 0 && currentBoostTime > 0){
            isBoosting = true;
            return true;
        }


        if(Mathf.Abs(GetLateralVelocity())>4.0f){
            return true;
        }

        return false;
    }

	public void ResetCheckpoints(){
		checkpointsPassed.Clear();
		highestCheckpointPassed = 0;
		Debug.Log($"Checkpoints reset for lap {lapCount + 1}.");
	}
	public Surface.SurfaceTypes GetSurface(){
		//Debug.Log($"Driving On {carSurfaceHandler.GetCurrentSurface()}");
        return carSurfaceHandler.GetCurrentSurface();
    }

	private void StopCar(){
    // Stop the Rigidbody2D
    rb.velocity = Vector2.zero;
    rb.angularVelocity = 0;

    // Prevent any input from affecting the car
    moveInput = 0;
    turnInput = 0;

    Debug.Log("Race finished. Car stopped.");
	}

	public void ChangeStats(int currentStation){
        if(currentStation == 1){
			maxBoostTime = 3f;
            base_acceleration = 6f;
			boostVal = 15f;
            baseSpeed = 10f;
            driftFactor = 0.95f;
            
        }else if(currentStation == 0){
			maxBoostTime = 9f;
			boostVal = 20f;
            base_acceleration = 5f;
            baseSpeed = 6f;
            driftFactor = 0.5f;
        }
    }

	public IEnumerator RespawnWithDelay(float waitTime)
	{
		Debug.Log("Respawning in 1 second...");

		// Optional: Disable car controls and play a respawn animation or effect
		CanDrive = false;
		rb.velocity = Vector2.zero;

		yield return new WaitForSeconds(waitTime); // Wait for 1 second

		if (highestCheckpointPassed >= 0 && highestCheckpointPassed < Track.Current.checkpoints.Length)
		{
			Transform respawnPoint = Track.Current.checkpoints[highestCheckpointPassed].transform;

			// Reset position and velocity
			rb.position = respawnPoint.position;
			Rotation = rb.rotation = respawnPoint.eulerAngles.z; 
			rb.velocity = Vector2.zero;
			rb.angularVelocity = 0;

	

			Debug.Log($"Respawned at checkpoint {highestCheckpointPassed}");
		}
		else
		{
			Debug.LogWarning("No valid checkpoint to respawn at.");
		}

		// Re-enable controls after respawning
		CanDrive = true;
	}


}

