using MessagesLibrary;
using System;
using System.Windows;
using System.ComponentModel;

namespace MessageApp
{

    public partial class MessageWindow : Window
    {
        public MessageService MessageService { get; set; }
        public StatusBlock StatusBlock { get; set; }

        public MessageWindow(MessageService messageService)
        {
            MessageService = messageService;
            MessageService.NewMessage = new Message();
            DataContext = this;
            InitializeComponent();
            StatusBlock = new StatusBlock(StatusBar);
            StatusBlock.Alert("Add recipients and send your message!");
        }

        private void AddRecipientButtonClick(object sender, RoutedEventArgs e)
        {
            var window = new AddressBookWindow(MessageService);
            window.ShowDialog();
        }

        private void SendButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageService.SendMessage();
                Close();
            }
            catch(EmptyFieldException ex)
            {
                StatusBlock.Alert(ex.Message, StatusBlock.Danger);
            }
            catch (Exception)
            {
                StatusBlock.Alert("An error occured while sending your message",StatusBlock.Danger);
            }
        }

        private void SubjectTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(SubjectTextBox.Text))
            {
                StatusBlock.Alert("You are about to send a message with no subject",StatusBlock.Warning);
            }
        }

        private void BodyTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(BodyTextBox.Text))
            {
                StatusBlock.Alert("You are about to send a message with no body", StatusBlock.Warning);
            }
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            
            if(MessageService.NewMessage.IsEmpty())
            {
                base.OnClosing(e);
                return;
            }
            var result = MessageBox.Show($"Are you sure you want to discard this Message?",
               "Delete this message?",
               MessageBoxButton.YesNo);
            if (result == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                e.Cancel = false;
                MessageService.Recipients.Clear();
            }
            base.OnClosing(e);
        }
    }
}
