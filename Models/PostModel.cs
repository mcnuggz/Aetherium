using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aetherium.Models
{
    public class PostModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int CharacterId { get; set; }
        [ForeignKey("CharacterId")]
        public CharacterModel Character { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsEdited { get; set; }
        public int EditedBy { get; set; }
        public string VisibleBy { get; set; } = "Public";
    }
}