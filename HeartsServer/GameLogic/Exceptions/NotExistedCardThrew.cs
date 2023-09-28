using System.Runtime.Serialization;

namespace HeartsServer.GameLogic.Exceptions
{
	[Serializable]
	internal class NotExistedCardThrew : Exception
	{
		public NotExistedCardThrew()
		{
		}

		public NotExistedCardThrew(string message) : base(message)
		{
		}

		public NotExistedCardThrew(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected NotExistedCardThrew(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
