namespace NthsKeys.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("nthskeys.settings")]
    public partial class setting
    {
        [Column(TypeName = "uint")]
        public long Id { get; set; }

        [Key]
        [Required]
        [StringLength(10)]
        public string Key { get; set; }

        [Required]
        [StringLength(30)]
        public string Value { get; set; }
    }
}
