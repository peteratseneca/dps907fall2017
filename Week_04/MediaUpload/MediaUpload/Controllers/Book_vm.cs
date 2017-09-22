using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// added...
using System.ComponentModel.DataAnnotations;

namespace MediaUpload.Controllers
{
    // Attention 05 - Book resource model classes

    public class BookAdd
    {
        // Attention 06 - Notice the "book add" does not include media item properties
        public BookAdd()
        {
            PublishedDate = DateTime.Now.AddYears(-1);
        }

        [Required, StringLength(100)]
        public string Title { get; set; }

        [Required, StringLength(100)]
        public string Author { get; set; }

        [Required, StringLength(13)]
        public string ISBN13 { get; set; }

        [Range(1, UInt16.MaxValue)]
        public int Pages { get; set; }
        public DateTime PublishedDate { get; set; }

        [Required, StringLength(50)]
        public string Format { get; set; }
    }

    public class BookBase : BookAdd
    {
        public int Id { get; set; }
    }

    // Display only, no data annotations
    public class BookWithMediaInfo : BookBase
    {
        // Attention 07 - Notice the name of this property - PhotoLength
        // In the source object - Book - we do NOT have a matching property
        // However, its value does get configured correctly by AutoMapper
        // when mapping a Book object to a BookWithMediaInfo object
        
        // Why?
        // The Photo *property* is an object that has a "Length" property
        // Therefore, we can use AutoMapper's magic to set the value
        public int PhotoLength { get; set; }
        public string ContentType { get; set; }
    }

    public class BookWithMedia : BookWithMediaInfo
    {
        // Attention 08 - Deliver the media item bytes with this class

        public byte[] Photo { get; set; }
    }
}
