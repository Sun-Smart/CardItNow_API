using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.Dynamic;

namespace carditnow.Models
{
    public class geographymaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "int")]
        public int? geoid { get; set; }
        [Column(TypeName = "string")]
        public string geoname { get; set; }
        [Column(TypeName = "string")]
        public string geocode { get; set; }
        [Column(TypeName = "decimal")]
        public decimal? chargepercent { get; set; }
        [Column(TypeName = "decimal")]
        public decimal? vat { get; set; }
        [Column(TypeName = "string")]
        public string useraccess { get; set; }
        [Column(TypeName = "string")]
        public string status { get; set; }
        [Column(TypeName = "int")]
        public int? createdby { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime? createddate{ get; set; }
        [Column(TypeName = "int")]
        public int? updatedby { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime? updateddate{ get; set; }
    }
    public class geographymasterView
    {
public geographymaster data{ get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<citymaster> citymasters { get; set; }
        [NotMapped]
        public string Deleted_citymaster_IDs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<geoaccess> geoaccesses { get; set; }
        [NotMapped]
        public string Deleted_geoaccess_IDs { get; set; }

    }
}

