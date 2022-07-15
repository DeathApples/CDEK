using System;
using System.Linq;
using Logic.Models;
using Logic.Services;
using System.Collections.Generic;

namespace Logic.Controllers
{
    public static class ContactControl
    {
        public static List<Contact> Contacts;

        static ContactControl()
        {
            Contacts = new List<Contact>();
        }

        public static bool Add(Guid organizationId, string name, string post, string email, string numberPhone)
        {
            Contact contact = new Contact()
            {
                Id = Guid.NewGuid(),

                Name = name,
                Post = post,
                Email = email,
                NumberPhone = numberPhone,
                OrganizationId = organizationId
            };

            if (Save(contact))
            {
                Contacts.Add(contact);
                return true;
            }

            return false;
        }

        private static bool Save(Contact contact)
        {
            using (var context = new MyDBContext())
            {
                if (context.Contacts.Any(prototype => prototype.Name == contact.Name && prototype.Post == contact.Post && prototype.Email == contact.Email))
                {
                    return false;
                }

                context.Contacts.Add(contact);
                context.SaveChanges();
                return true;
            }
        }

        public static List<Contact> Search(Guid organizationId)
        {
            using (var context = new MyDBContext())
            {
                if (context.Contacts.Any(prototype => prototype.OrganizationId == organizationId))
                {
                    Contacts = context.Contacts.Where(prototype => prototype.OrganizationId == organizationId).ToList();
                    return Contacts;
                }
            }

            return new List<Contact>();
        }

        public static void Remove(Contact contact)
        {
            Contacts.Remove(contact);

            using (var context = new MyDBContext())
            {
                if (context.Contacts.Any(prototype => prototype.Id == contact.Id))
                {
                    context.Contacts.RemoveRange(context.Contacts.Where(prototype => prototype.Id == contact.Id));
                }

                context.SaveChanges();
            }
        }

        public static void RemoveAll(Organization organization)
        {
            using (var context = new MyDBContext())
            {
                if (context.Contacts.Any(prototype => prototype.OrganizationId == organization.Id))
                {
                    context.Contacts.RemoveRange(context.Contacts.Where(prototype => prototype.OrganizationId == organization.Id));
                    Contacts.RemoveAll(prototype => prototype.OrganizationId == organization.Id);
                }

                context.SaveChanges();
            }
        }
    }
}
