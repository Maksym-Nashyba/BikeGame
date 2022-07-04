namespace SBPScripts
{
    public readonly ref struct InputValues
    {
        public readonly float Steer;
        public readonly float Acceleration;

        public InputValues(float steer, float acceleration)
        {
            Steer = steer;
            Acceleration = acceleration;
        }
    }
}