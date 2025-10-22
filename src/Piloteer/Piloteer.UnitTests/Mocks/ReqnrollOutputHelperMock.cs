namespace Piloteer.UnitTests.Mocks
{
    internal class ReqnrollOutputHelperMock : IReqnrollOutputHelper
    {
        public void AddAttachment(string filePath)
        {
            throw new NotImplementedException();
        }

        public void WriteLine(string message)
        {
            Console.WriteLine(message);
        }

        public void WriteLine(string format, params object[] args)
        {
            Console.WriteLine(format, args);
        }
    }
}
