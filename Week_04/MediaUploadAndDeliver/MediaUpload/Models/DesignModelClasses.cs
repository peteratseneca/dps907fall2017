using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// added...
using System.ComponentModel.DataAnnotations;

namespace MediaUpload.Models
{
    public class Book
    {
        public Book()
        {
            PublishedDate = DateTime.Now.AddYears(-1);
        }

        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Title { get; set; }

        [Required, StringLength(100)]
        public string Author { get; set; }

        [Required, StringLength(13)]
        public string ISBN13 { get; set; }

        public int Pages { get; set; }
        public DateTime PublishedDate { get; set; }

        [Required, StringLength(50)]
        public string Format { get; set; }

        public byte[] Photo { get; set; }
        public string ContentType { get; set; }
    }

}
