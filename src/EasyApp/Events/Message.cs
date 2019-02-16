namespace EasyApp.Events
{
    public class Message
	{
		public MessageType Type
		{
			get;
			set;
		}

		public string Text
		{
			get;
			set;
		}

		public Priority Priority
		{
			get;
			set;
		}
	}
}
