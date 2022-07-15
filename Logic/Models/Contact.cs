using System;

namespace Logic.Models
{
    public class Contact
    {
        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; }

        public string Name { get; set; }
        public string Post { get; set; }
        public string Email { get; set; }
        public string NumberPhone { get; set; }
    }
}
