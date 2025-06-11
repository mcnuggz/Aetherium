namespace Aetherium.Models
{
    public class CommentModel
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public int CharacterId { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }

        public PostModel Post { get; set; } = null!;
        public CharacterModel Character { get; set; } = null!;
    }
}
