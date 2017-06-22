using MessagesLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MessageApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MessageService MessageService { get; set; } = new MessageService();
        public StatusBlock StatusBlock { get; set; }
        
        public MainWindow()
        {  
            InitializeComponent();
            DataContext = this;
            StatusBlock = new StatusBlock(StatusBar);
            StatusBlock.Alert("Welcome to the most awesome messages application!");
        }
        
        private void NewMessageButtonClick(object sender, RoutedEventArgs e)
        {
            var window = new MessageWindow(MessageService);
            window.ShowDialog();            
        }

        private void DeleteMessageButtonClick(object sender, RoutedEventArgs e)
        {
            var message = MessagesList.SelectedItem as Message;
            if( message == null)
            {
                StatusBlock.Alert("No message was selected", StatusBlock.Warning);
                return;
            }
            var result = MessageBox.Show($"Are you sure you want to delete Message with subject {message.Subject}",
                "Delete this message?",
                MessageBoxButton.YesNo);
            if ( result == MessageBoxResult.No  )
            {
                StatusBlock.Alert($"Delete was canceled. Please make up your mind.",StatusBlock.Warning);
                return;
            }
            try { 
                MessageService.DeleteMessage(message);
                StatusBlock.Alert($"Message with subject {message.Subject} was deleted",StatusBlock.Success);
            }
            catch (ItemNotFoundException ex)
            {
                StatusBlock.Alert(ex.Message, StatusBlock.Danger);
            }
            catch(Exception )
            {
                StatusBlock.Alert("Could not delete message", StatusBlock.Danger);
            }


        }

        
    }
}
