using Aetherium.Enums;

namespace Aetherium.Models
{
    public class CharacterRelationshipModel
    {
        public int Id { get; set; }
        public int CharacterAId { get; set; }
        public CharacterModel CharacterA { get; set; }
        public int CharacterBId { get; set; }
        public CharacterModel CharacterB { get; set; }
        public RelationshipTypeEnum RelationshipType { get; set; }
        public bool Confirmed { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
