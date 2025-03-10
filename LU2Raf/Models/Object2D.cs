namespace LU2Raf.Models
{
    public class Object2D
    {
        public Guid Id { get; set; }
        public int PrefabId { get; set; }
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public float ScaleX { get; set; }
        public float ScaleY { get; set; }
        public float RotationZ { get; set; }
        public int SortingLayer { get; set; }

        // Voeg hier de EnvironmentId toe
        public string EnvironmentId { get; set; }

        public Object2D() { }

        public Object2D(int prefabId, float positionX, float positionY, float scaleX, float scaleY, float rotationZ, int sortingLayer, string environmentId)
        {
            Id = Guid.NewGuid();
            PrefabId = prefabId;
            PositionX = positionX;
            PositionY = positionY;
            ScaleX = scaleX;
            ScaleY = scaleY;
            RotationZ = rotationZ;
            SortingLayer = sortingLayer;
            EnvironmentId = environmentId;
        }
    }
}
