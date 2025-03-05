using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Buffers;

namespace LU2Raf.Models
{
    public class Environment2D
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public Guid OwnerUserId { get; set; }

        [Required]
        public int MinLength { get; set; }

        [Required]
        public int MaxLength { get; set; }

        public Environment2D() { }

        public Environment2D(string name, Guid ownerUserId, int minLength, int maxLength)
        {
            Id = Guid.NewGuid();
            Name = name;
            OwnerUserId = ownerUserId;
            MinLength = minLength;
            MaxLength = maxLength;
        }
    }
    public static class EnvironmentStore
    {
        public static List<Environment2D> Environments { get; } = new List<Environment2D>();
    }
}
