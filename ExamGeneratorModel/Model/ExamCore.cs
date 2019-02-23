using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExamGeneratorModel.Model
{
    public class ExamCore: Entity
    {
        [Key]
        public int Id { get; set; }
        [StringLength(200)]
        public string Name { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
        [Required]
        public bool IsDeleted { get; set; }
        [Required]
        public string Owner { get; set; }
    }
}
