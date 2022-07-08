namespace SBPScripts
{
    public readonly ref struct InputValues
    {
        public readonly float Steer;
        public readonly float Acceleration;
        public readonly bool BrakesHit;

        public InputValues(float steer, float acceleration, bool brakesHit)
        {
            Steer = steer;
            Acceleration = acceleration;
            BrakesHit = brakesHit;
        }
    }
}