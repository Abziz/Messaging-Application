using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;

namespace MessagesLibrary
{
    public class MessageService
    {
        public ObservableCollection<Contact> Contacts { get; set; }

        public ObservableCollection<Message> Messages { get; set; }
         
        public Message NewMessage { get; set; }

        public ObservableCollection<Contact> Recipients { get; set; }

        /* methods */
        public MessageService()
        {
            using (var db = new MessagesContext())
            {
                Contacts = new ObservableCollection<Contact>(db.Contacts);
                Messages = new ObservableCollection<Message>(db.Messages.Include("Recipients"));
            }
            Recipients = new ObservableCollection<Contact>();
        }

        public void AddContact(Contact contact)
        {
            if (String.IsNullOrEmpty(contact.Email) || String.IsNullOrEmpty(contact.FirstName) || String.IsNullOrEmpty(contact.LastName))
            {
                throw new FormatException("Please fill all the fields properly");
            }
            var result = Contacts.SingleOrDefault(c => c.Email.ToLower().Equals(contact.Email.ToLower()));
            if (result != null)
            {
                throw new ItemAllreadyExistsException($"Contact with Email '{result.Email}' allready exists");
            }
            using (var db = new MessagesContext())
            {
                var con = db.Contacts.Add(contact);
                db.SaveChanges();
                Contacts.Add(con);
            }
        }

        public void SendMessage()
        {
            if (NewMessage.Recipients.Count == 0)
            {
                throw new EmptyFieldException("You forgot to add recipients to your message");
            }
            using (var db = new MessagesContext())
            {
                var mes = db.Messages.Add(NewMessage);
                foreach (var contact in NewMessage.Recipients)
                {
                    db.Contacts.Attach(contact);
                    contact.Inbox.Add(mes);
                }
                db.SaveChanges();
                Messages.Add(mes);
            }
            NewMessage = new Message();
            Recipients.Clear();
        }

        public void DeleteMessage(Message message)
        {
            var found = Messages.SingleOrDefault(m => m.Id.Equals(message.Id));
            if (found == null)
            {
                throw new ItemNotFoundException("Message was not found in the database");
            }
            using (var db = new MessagesContext())
            {
                db.Messages.Attach(found);
                db.Messages.Remove(found);
                db.SaveChanges();
                Messages.Remove(found);
            }
        }

        public void AddRecipient(Contact contact)
        {
            if (contact == null)
            {
                throw new EmptyFieldException("You did not select any contact");
            }
            var result = NewMessage.Recipients.SingleOrDefault(c => c.Email.Equals(contact.Email));
            if (result != null)
            {
                throw new ItemAllreadyExistsException("You allready chose this contact.");
            }
            result = Contacts.SingleOrDefault(c => c.Email.Equals(contact.Email));
            if (result == null)
            {
                throw new ItemNotFoundException($"Could not find {contact.Email} in database");
            }
            NewMessage.Recipients.Add(result);
            Recipients.Add(result);
        }
    }
}
