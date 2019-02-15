namespace EasyApp.Events
{
    public class EasyMessage
	{
		public MessageType MessageType
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
