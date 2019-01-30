using System;
namespace QApp.Events
{
	public class QMessage
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
		public MessagePriority Priority
		{
			get;
			set;
		}
	}
}
