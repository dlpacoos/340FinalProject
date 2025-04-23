using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class RollerAgent : Agent
{
    Rigidbody rBody;
    void Start () {
        rBody = GetComponent<Rigidbody>();
    }

    public Transform Target;
    public override void OnEpisodeBegin()
    {
        // If the Agent fell, zero its momentum
        if (this.transform.localPosition.y < 0)
        {
            this.rBody.angularVelocity = Vector3.zero;
            this.rBody.linearVelocity = Vector3.zero;
            this.transform.localPosition = new Vector3( 0, 0.5f, 0);
        }

        // Move the target to a new spot
        Target.localPosition = new Vector3(Random.value * 8 - 4,
                                            0.5f,
                                            Random.value * 8 - 4);
    }

    public override void CollectObservations(VectorSensor sensor)
    { 
        // Target and Agent positions
        sensor.AddObservation(Target.localPosition);
        sensor.AddObservation(this.transform.localPosition);

        // Agent velocity
        sensor.AddObservation(rBody.linearVelocity.x);
        sensor.AddObservation(rBody.linearVelocity.z);
    }

    public float constantSpeed = 1f; // Your desired constant speed

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // Read input direction
        Vector3 inputDirection = Vector3.zero;
        inputDirection.x = actionBuffers.ContinuousActions[0];
        inputDirection.z = actionBuffers.ContinuousActions[1];

        // Normalize to ensure diagonal movement isn't faster
        if (inputDirection.magnitude > 1f)
            inputDirection.Normalize();

        // Apply constant speed
        Vector3 velocity = inputDirection * constantSpeed;
        rBody.linearVelocity = new Vector3(velocity.x, rBody.linearVelocity.y, velocity.z); // keep y velocity for gravity

        // Rewards and environment logic
        float distanceToTarget = Vector3.Distance(this.transform.localPosition, Target.localPosition);

        if (distanceToTarget < 1.42f)
        {
            SetReward(1.0f);
            EndEpisode();
        }
        else if (this.transform.localPosition.y < 0)
        {
            SetReward(-1.0f);
            EndEpisode();
        }
    }

    void OnCollisionEnter(Collision collision)
{
    if (collision.gameObject.CompareTag("Bullet"))
    {
        SetReward(-1.0f); // Penalty for getting hit 
    }
}

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        continuousActionsOut[1] = Input.GetAxis("Vertical");
    }
}
