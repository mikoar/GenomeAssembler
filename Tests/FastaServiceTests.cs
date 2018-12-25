using Assembly;
using Xunit;

namespace Tests
{
    public class FastaServiceTests
    {
        public void Parse()
        {
            var fileReader = new TestFileReader();
            var fastaService = new FastaService(fileReader);
        }
    }
}