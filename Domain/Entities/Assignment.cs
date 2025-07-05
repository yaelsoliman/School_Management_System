﻿namespace Domain.Entities
{
    public class Assignment : BaseEntity
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }

        public Guid? CourseId { get; set; }
        public virtual Course? Course { get; set; }

        public Guid? CreatedByUserId { get; set; }

    }
}
