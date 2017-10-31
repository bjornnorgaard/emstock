using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Image
    {
        public long Id { get; set; }
        [MaxLength(128)]
        [DisplayName("Image Mime Type")]
        public string ImageMimeType { get; set; }
        public byte[] Thumbnail { get; set; }
        [DisplayName("Image Data")]
        public byte[] ImageData { get; set; }
    }
}
