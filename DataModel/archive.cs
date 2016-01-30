namespace NthsKeys.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("nthskeys.archive")]
    public partial class archive
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "uint")]
        public long Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Path { get; set; }

        [StringLength(20)]
        public string Password { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime CreateTime { get; set; }
    }
}
