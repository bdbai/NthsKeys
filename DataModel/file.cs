namespace NthsKeys.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("nthskeys.file")]
    public partial class file
    {
        [Column(TypeName = "uint")]
        public long Id { get; set; }

        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Path { get; set; }

        [Required]
        public virtual archive Archive { get; set; }
    }
}
