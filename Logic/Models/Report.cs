using System;

namespace Logic.Models
{
    public class Report : IComparable<Report>
    {
        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; }

        public string DateReport { get; set; }
        public string Information { get; set; }

        public override string ToString() => DateReport;
        public int CompareTo(Report other) => DateReport.CompareTo(other.DateReport);
    }
}
