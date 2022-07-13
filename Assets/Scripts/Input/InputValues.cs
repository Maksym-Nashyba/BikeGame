namespace SBPScripts
{
    public readonly ref struct InputValues
    {
        public readonly float Steer;
        public readonly float Acceleration;
        public readonly bool BrakesHit;
        public readonly bool SprintHit;

        public InputValues(float steer, float acceleration, bool brakesHit, bool sprintHit)
        {
            Steer = steer;
            Acceleration = acceleration;
            BrakesHit = brakesHit;
            SprintHit = sprintHit;
        }
    }
}