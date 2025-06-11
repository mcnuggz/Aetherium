using Aetherium.Enums;

namespace Aetherium.Models
{
    public class ReactionModel
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public int CharacterId { get; set; }
        public ReactionTypeEnum Type { get; set; }

        public PostModel Post { get; set; } = null!;
        public CharacterModel Character { get; set; } = null!;
    }
}
