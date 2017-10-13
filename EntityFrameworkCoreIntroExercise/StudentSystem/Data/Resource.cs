namespace StudentSystem.Data
{
    using System.ComponentModel.DataAnnotations;
    using StudentSystem.Data.Enums;

    public class Resource
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        
        public ResourceType ResourceType { get; set; }

        [Required]
        public string Url { get; set; }

        public int CourseId { get; set; }

        public Course Course { get; set; }

    }
}
