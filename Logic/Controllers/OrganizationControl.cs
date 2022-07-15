using System;
using System.Linq;
using Logic.Models;
using Logic.Services;
using System.Collections.Generic;

namespace Logic.Controllers
{
    public static class OrganizationControl
    {
        public static List<Organization> Organizations;

        static OrganizationControl()
        {
            Organizations = new List<Organization>();
        }

        public static bool Add(string name, string city, string iNN, string eDM, string email, string address, string website, string contractDate, string contractNumber, Guid? id = null)
        {
            Organization organization = new Organization()
            {
                Id = id ?? Guid.NewGuid(),

                Name = name,
                City = city,
                INN = iNN,
                EDM = eDM,
                Email = email,
                Address = address,
                Website = website,
                ContractDate = contractDate,
                ContractNumber = contractNumber
            };

            if (Save(organization))
            {
                Organizations.Add(organization);
                return true;
            }

            return false;
        }

        private static bool Save(Organization organization)
        {
            using (var context = new MyDBContext())
            {
                if (context.Organizations.Any(prototype => prototype.Name == organization.Name && prototype.ContractNumber == organization.ContractNumber && prototype.INN == organization.INN))
                {
                    return false;
                }

                context.Organizations.Add(organization);
                context.SaveChanges();
                return true;
            }
        }

        public static List<Organization> Search(string info)
        {
            Update();

            if (Organizations.Any(prototype => prototype.Summary.Contains(info)))
            {
                Organizations = Organizations.Where(prototype => prototype.Summary.Contains(info)).ToList();
                return Organizations;
            }

            return new List<Organization>();
        }

        public static void Update()
        {
            using (var context = new MyDBContext())
            {
                Organizations = context.Organizations.ToList();
            }
        }

        public static void Remove(Organization organization)
        {
            Organizations.Remove(organization);

            using (var context = new MyDBContext())
            {
                if (context.Organizations.Any(prototype => prototype.Id == organization.Id))
                {
                    context.Organizations.RemoveRange(context.Organizations.Where(prototype => prototype.Id == organization.Id));
                }

                context.SaveChanges();
            }
        }
    }
}                                                                                                                                       
