using MessagesLibrary;
using MyToolkit.Collections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace MessageApp
{
    public partial class AddressBookWindow : Window
    {
        public MessageService MessageService { get; set; }
        public ObservableCollectionView<Contact> SearchResults { get; set; }
        public StatusBlock StatusBlock { get; set; }

        public AddressBookWindow(MessageService messageService)
        {
            MessageService = messageService;
            SearchResults = new ObservableCollectionView<Contact>(MessageService.Contacts);
            InitializeComponent();
            DataContext = this;
            StatusBlock = new StatusBlock(StatusBar);
            StatusBlock.Alert("Search and select contacts from the list");
        }

        private void NewContactButtonClick(object sender, RoutedEventArgs e)
        {
            var window = new AddContactWindow(MessageService);
            window.ShowDialog();
        }

        private void SearchTextBoxKeyUp(object sender, KeyEventArgs e)
        {
            if (string.IsNullOrEmpty(SearchTextBox.Text))
            {
                SearchResults.Filter = null;
            }
            else
            {
                SearchResults.Filter = c => c.FirstName.ToLower().StartsWith(SearchTextBox.Text.ToLower())
                || c.LastName.ToLower().StartsWith(SearchTextBox.Text.ToLower())
                || (c.FirstName + " " + c.LastName).ToLower().StartsWith(SearchTextBox.Text.ToLower())
                || c.Email.ToLower().StartsWith(SearchTextBox.Text.ToLower());
            }
        }

        private void SelectContactButtonClick(object sender, RoutedEventArgs e)
        {
            var selected = ContactsList.SelectedItem as Contact;
            try {
                MessageService.AddRecipient(selected);
                StatusBlock.Alert($"{selected.FirstName} {selected.LastName} was added to the list", StatusBlock.Success);
            }
            catch (EmptyFieldException ex)
            {
                StatusBlock.Alert(ex.Message, StatusBlock.Warning);
            }
            catch(ItemAllreadyExistsException ex)
            {
                StatusBlock.Alert(ex.Message, StatusBlock.Warning);
            }
            catch(ItemNotFoundException ex)
            {
                StatusBlock.Alert(ex.Message, StatusBlock.Warning);
            }
            catch(Exception)
            {
                StatusBlock.Alert("Error while adding recipient",StatusBlock.Danger);
            }
        }
    }
}
