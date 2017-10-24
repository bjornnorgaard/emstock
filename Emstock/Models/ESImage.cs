using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class EsImage
    {
        public long Id { get; set; }
        [MaxLength(128)]
        public string ImageMimeType { get; set; }
        public byte[] Thumbnail { get; set; }
        public byte[] ImageData { get; set; }
    }
}
