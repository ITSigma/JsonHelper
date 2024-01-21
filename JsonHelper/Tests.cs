using JsonHelper.Infrastructure;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Legacy;
using System;
using System.IO;
using System.Threading.Tasks;

namespace JsonHelper
{
    [TestFixture]
    public class DefaultFileWriterTests
    {
        private const string TestDirectory = "TestDirectory";
        private const string TestFileName = "TestFile.json";
        private const string TestFilePath = "testfile.json";

        [SetUp]
        public void Setup()
        {
            if (!Directory.Exists(TestDirectory))
                Directory.CreateDirectory(TestDirectory);
        }

        [TearDown]
        public void Cleanup()
        {
            if (Directory.Exists(TestDirectory))
                Directory.Delete(TestDirectory, true);
        }

        [OneTimeSetUp]
        public void CreateTestFile()
        {
            File.WriteAllText(TestFilePath, "{\"Id\":1,\"Name\":\"TestObject\"}");
        }

        [OneTimeTearDown]
        public void RemoveTestFile()
        {
            if (File.Exists(TestFilePath))
            {
                File.Delete(TestFilePath);
            }
        }

        [Test]
        [TestCase("ExistingData", "NewData", "ExistingDataNewData")]
        [TestCase("", "NewData", "NewData")]
        [TestCase("ExistingData", "", "ExistingData")]
        public async Task SaveToFileAsyncJoinValues(string existingDataName, string newDataName, string expectedName)
        {
            var valueToSave = newDataName;

            var existingData = existingDataName;
            await DefaultFileWriter<string>.SaveToFileAsync(existingData, TestDirectory, TestFileName);

            Func<string, string, string> joinFunction = (existing, newData) =>
            {
                existing += newData;
                return existing;
            };

            await DefaultFileWriter<string>.SaveToFileAsync(valueToSave, TestDirectory, TestFileName, joinFunction);

            var savedData = await FileReader<string>.ReadFromFileAsync(Path.Combine(TestDirectory, TestFileName));
            Assert.Equals(expectedName, savedData);
        }

        [Test]
        [TestCase("NewData")]
        [TestCase("")]
        public async Task SaveToFileAsyncNoJoinValues(string newDataName)
        {
            var valueToSave = newDataName;

            await DefaultFileWriter<string>.SaveToFileAsync(valueToSave, TestDirectory, TestFileName);

            var savedData = await FileReader<string>.ReadFromFileAsync(Path.Combine(TestDirectory, TestFileName));
            Assert.Equals(newDataName, savedData);
        }

        [Test]
        [TestCase(false, 1, "TestObject")]
        [TestCase(true, 1, "TestObject")]
        public async Task ReadFromFileAsync_Success(bool seekBegin, int expectedId, string expectedName)
        {
            var result = await FileReader<string>.ReadFromFileAsync(TestFilePath, seekBegin);

            Assert.Equals(expectedId, result);
            Assert.Equals(expectedName, result);
        }

        [Test]
        public void ReadFromFileAsync_FileNotFound()
        {
            var fileReader = new FileReader<string>();
            var nonExistentFilePath = "nonexistentfile.json";

            var exception = Assert.ThrowsAsync<FileNotFoundException>(async () =>
                await FileReader<string>.ReadFromFileAsync(nonExistentFilePath));

            StringAssert.Contains("There is no such file", exception.Message);
        }

        [Test]
        public void ReadFromFileAsync_NullDeserialization()
        {
            var fileReader = new FileReader<string>();
            var invalidJsonFilePath = "invalidjsonfile.json";

            var exception = Assert.ThrowsAsync<NullReferenceException>(async () =>
                await FileReader<string>.ReadFromFileAsync(invalidJsonFilePath));

            StringAssert.Contains("Attempt to read null", exception.Message);
        }
    }
}