using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities
{
    public class Photo
    {
        [Key]
        [Column(TypeName = "uniqueidentifier")]
        [Required]
        public Guid PhotoID { get; set; }

        [Column(TypeName = "uniqueidentifier")]
        [Required]
        public Guid UserID { get; set; }

        [Column(TypeName = "varchar(max)")]
        [Required]
        public string PhotoPath { get; set; }

        [Column(TypeName = "nvarchar(25)")]
        public string Title { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string Descripton { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        [Required]
        public string Categories { get; set; }

        [Column(TypeName = "float")]
        [Required]
        public float Size { get; set; }

        [Column(TypeName = "varchar(9)")]
        [Required]
        public string Resolution { get; set; }

        [Column(TypeName = "bit")]
        [Required]
        public bool Private { get; set; }

        [Column(TypeName = "datetimeoffset(0)")]
        [Required]
        public DateTimeOffset DateTimeModify { get; set; }
    }
}