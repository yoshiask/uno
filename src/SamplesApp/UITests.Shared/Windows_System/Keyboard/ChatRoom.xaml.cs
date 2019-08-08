using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Uno.Extensions;
using Uno.UI.Samples.Controls;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace UITests.Shared.Windows_System.Keyboard
{
	[SampleControlInfo("Keyboard", "ChatRoom")]
	public sealed partial class ChatRoom : UserControl
	{
		public ChatRoom()
		{
			this.InitializeComponent();
			this.DataContext = new ChatRoomViewModel();
		}
	}

	public class ChatRoomViewModel : INotifyPropertyChanged
	{
		public ChatRoomViewModel()
		{
			SendMessageCommand = new GenericCommand<string>(message => SendMessage(message));
			PopulateMessages();
		}

		public void PopulateMessages()
		{
			Enumerable
				.Range(0, 50)
				.Select(index => new Message($"Message #{index}"))
				.ForEach(Messages.Add);
		}

		public ObservableCollection<Message> Messages { get; } = new ObservableCollection<Message>();

		public ICommand SendMessageCommand { get; }

		public event PropertyChangedEventHandler PropertyChanged;

		private void SendMessage(string message)
		{
			Messages.Add(new Message(message));

			// TODO: This shouldn't be necessary if ObservationCollections are supported
			//PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Messages)));
		}

		public class Message
		{
			public Message(string text)
			{
				Text = text;
			}

			public string Text { get; set; }
		}
	}

	public class GenericCommand<T> : ICommand
	{
		private Action<T> _action;
		public event EventHandler CanExecuteChanged;

		public GenericCommand(Action<T> action)
		{
			_action = action;
		}

		public bool CanExecute(object parameter) => true;

		public void Execute(object parameter) => _action((T)parameter);
	}
}
