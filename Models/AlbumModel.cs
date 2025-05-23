namespace Aetherium.Models
{
    public class AlbumModel
    {
        public int Id { get; set; }
        public CharacterModel Character { get; set; }
        public int CharacterId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<PhotoModel> Photos { get; set; }

    }
}
