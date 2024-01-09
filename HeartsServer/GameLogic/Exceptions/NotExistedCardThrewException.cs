using System.Runtime.Serialization;

namespace HeartsServer.GameLogic.Exceptions
{
	[Serializable]
	internal class NotExistedCardThrewException : Exception
	{
		public NotExistedCardThrewException()
		{
		}

		public NotExistedCardThrewException(string message) : base(message)
		{
		}

		public NotExistedCardThrewException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected NotExistedCardThrewException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
