using System.Text.Json.Serialization;

namespace LU2Raf.Models
{
    public class Object2D
    {
        public Guid Id { get; set; }

        public string PrefabId { get; set; }

        public float PositionX { get; set; }

        public float PositionY { get; set; }

        public float ScaleX { get; set; }

        public float ScaleY { get; set; }

        public float RotationZ { get; set; }

        public int SortingLayer { get; set; }

        public Object2D() { }

        public Object2D(string prefabId, float positionX, float positionY, float scaleX, float scaleY, float rotationZ, int sortingLayer)
        {
            Id = Guid.NewGuid();
            PrefabId = prefabId;
            PositionX = positionX;
            PositionY = positionY;
            ScaleX = scaleX;
            ScaleY = scaleY;
            RotationZ = rotationZ;
            SortingLayer = sortingLayer;
        }
    }
    public static class ObjectStore
    {
        public static List<Object2D> Objects { get; } = new List<Object2D>();
    }
}
