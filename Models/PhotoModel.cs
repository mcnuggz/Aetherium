namespace Aetherium.Models
{
    public class PhotoModel
    {
        public int Id { get; set; }
        public int CharacterId { get; set; }
        public CharacterModel Character { get; set; }
        public string Url { get; set; }
        public string Caption { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
        public int? AlbumId { get; set; }
        public AlbumModel Album { get; set; }
    }
}