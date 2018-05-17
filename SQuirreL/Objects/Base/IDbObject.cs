namespace TomLabs.SQuirreL.Data
{
	public interface IDbObject
	{
		string Schema { get; }
		string Name { get; }

		string Script { get; }
	}
}