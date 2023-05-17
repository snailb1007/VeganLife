namespace VeganLife.Helpers
{
    public static class CalculateHelper
    {
        public static float CalculateBMI(float mass, float height)
        {
            return mass / (height*height);
        }
    }
}
