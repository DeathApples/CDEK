using System;

namespace Logic.Models
{
    public class Organization
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string City { get; set; }
        public string INN { get; set; }
        public string EDM { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Website { get; set; }
        public string ContractDate { get; set; }
        public string ContractNumber { get; set; }

        public string Summary { get => $"{Name} {City} {INN} {EDM} {Email} {Address} {Website} {ContractDate} {ContractNumber}".ToLower(); }
    }
}
