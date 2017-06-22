using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace MessagesLibrary
{
    public class MessagesContext : DbContext
    {
        public MessagesContext() : base()
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<MessagesContext>());
            Configuration.LazyLoadingEnabled = false;
        }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Message> Messages { get; set; }
    }

    public class Message
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string Subject { get; set; }

        [MaxLength(2000)]
        public string Body { get; set; }

        public DateTimeOffset DateSent { get; set; } = DateTimeOffset.Now;

        public virtual ICollection<Contact> Recipients { get; set; } = new HashSet<Contact>();

        public bool IsEmpty()
        {
            return string.IsNullOrEmpty(Subject) && string.IsNullOrEmpty(Body) && (Recipients.Count == 0);
        }
    }

    public class Contact
    {
        [Key]
        [EmailAddress]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public virtual ICollection<Message> Inbox { get; set; } = new HashSet<Message>();

    }
}
