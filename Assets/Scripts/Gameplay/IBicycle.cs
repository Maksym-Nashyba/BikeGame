namespace Gameplay
{
    public interface IBicycle
    {
        public float GetCurrentSpeed();

        public float GetAcceleration();

        public float GetTorqueY();

        public void SetInteractable(bool enabled);

        public bool IsAirborne();

        public float GetAirtimeSeconds();
    }
}
