namespace BigSchool.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Course")]
    public partial class Course
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Course()
        {
            Attendances = new HashSet<Attendance>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(128)]
        public string LecturerId { get; set; }

        [Required]
        [StringLength(255)]
        public string Place { get; set; }

        [Column(TypeName = "date")]
        public DateTime DateTime { get; set; }

        public int CategoryId { get; set; }
        public List<Category> ListCategory = new List<Category>();
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Attendance> Attendances { get; set; }
        public string Name;
        public virtual Category Category { get; set; }
    }
}
