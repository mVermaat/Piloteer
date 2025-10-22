namespace Piloteer
{
    public interface ISecretProvider
    {
        string GetSecret(string name);
    }
}