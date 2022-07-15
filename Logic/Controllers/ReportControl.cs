using System;
using System.Linq;
using Logic.Models;
using Logic.Services;
using System.Collections.Generic;

namespace Logic.Controllers
{
    public static class ReportControl
    {
        public static List<Report> Reports;

        static ReportControl()
        {
            Reports = new List<Report>();
        }

        public static bool Add(Guid organizationId, string information, string reportDate)
        {
            Report report = new Report()
            {
                Id = Guid.NewGuid(),

                DateReport = reportDate,
                Information = information,
                OrganizationId = organizationId
            };

            if (Save(report))
            {
                Reports.Add(report);
                return true;
            }

            return false;
        }

        private static bool Save(Report report)
        {
            using (var context = new MyDBContext())
            {
                if (context.Reports.Any(prototype => prototype.OrganizationId == report.OrganizationId && prototype.DateReport == report.DateReport))
                {
                    return false;
                }

                context.Reports.Add(report);
                context.SaveChanges();
                return true;
            }
        }

        public static List<Report> Search(Guid organizationId)
        {
            using (var context = new MyDBContext())
            {
                if (context.Reports.Any(prototype => prototype.OrganizationId == organizationId))
                {
                    Reports = context.Reports.Where(prototype => prototype.OrganizationId == organizationId).ToList();
                    return Reports;
                }
            }

            return new List<Report>();
        }

        public static void Remove(Report report)
        {
            Reports.Remove(report);

            using (var context = new MyDBContext())
            {
                if (context.Reports.Any(prototype => prototype.Id == report.Id))
                {
                    context.Reports.RemoveRange(context.Reports.Where(prototype => prototype.Id == report.Id));
                }

                context.SaveChanges();
            }
        }

        public static void RemoveAll(Organization organization)
        {
            using (var context = new MyDBContext())
            {
                if (context.Reports.Any(prototype => prototype.OrganizationId == organization.Id))
                {
                    context.Contacts.RemoveRange(context.Contacts.Where(prototype => prototype.OrganizationId == organization.Id));
                    Reports.RemoveAll(prototype => prototype.OrganizationId == organization.Id);
                }

                context.SaveChanges();
            }
        }
    }
}
