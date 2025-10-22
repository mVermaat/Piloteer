namespace Piloteer.UnitTests
{
    public class ErrorCodesTest
    {
        [Fact]
        public void TestThatNonExistingErrorCodesGetsSpecificMessage()
        {
            try
            {

            }
            catch (TestExecutionException ex)
            {
                Assert.Equal(-1, ex.ErrorCode);
                Assert.Equal("Error code -1 doesn't exist", ex.Message);
            }
        }
    }
}
