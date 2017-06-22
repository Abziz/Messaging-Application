using MessagesLibrary;
using System;
using System.Collections.ObjectModel;
using System.Data.Entity.Validation;
using System.Windows;

namespace MessageApp
{
    /// <summary>
    /// Interaction logic for AddContactWindow.xaml
    /// </summary>
    public partial class AddContactWindow : Window
    {
        public MessageService MessageService { get; set; }
        public StatusBlock StatusBlock { get; set; }
        public AddContactWindow(MessageService messageService)
        {
            MessageService = messageService;
            DataContext = this;
            InitializeComponent();
            StatusBlock = new StatusBlock(StatusBar);
        }

        private void AddContactButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var contact = new Contact
                {
                    Email = EmailTextBox.Text,
                    FirstName = FirstNameTextBox.Text,
                    LastName = LastNameTextBox.Text
                };
                MessageService.AddContact(contact);
                StatusBlock.Alert($"Contact {contact.Email} created successfully",StatusBlock.Success);
                EmailTextBox.Clear();
                FirstNameTextBox.Clear();
                LastNameTextBox.Clear();
            }
            catch(ItemAllreadyExistsException ex)
            {
                StatusBlock.Alert(ex.Message, StatusBlock.Danger);
            }
            catch(FormatException ex)
            {
                StatusBlock.Alert(ex.Message, StatusBlock.Warning);
            }
            catch(DbEntityValidationException )
            {
                StatusBlock.Alert("Validation failed for one of the fields", StatusBlock.Danger);
            }
            catch(Exception)
            {
                StatusBlock.Alert("Error while saving to database",StatusBlock.Danger);
            }
        }
    }
}
