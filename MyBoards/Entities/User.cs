﻿namespace MyBoards.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }

        public Address Adress { get; set; }
        public List<WorkItem> WorkItems { get; set; } = new List<WorkItem>();
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
