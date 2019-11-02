using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Moq;
using System.Collections.Generic;

namespace ChustaSoft.Tools.SecureConfig.UnitTests.Helpers
{

    public class UnitTestHelper
    {

        public const string PRIVATE_TEST_KEY = "5357F6B0313A478A9BF901BB37B4A458";

        public static IConfiguration MockedConfiguration(int connectionsNumber = 0)
        {
            var mockedConfiguration = new Mock<IConfiguration>();
            var mockedConfigurationSections = GetMockedConnectionsSection(connectionsNumber);

            mockedConfiguration.Setup(i => i.GetSection(It.Is<string>(x => x.Equals("ConnectionStrings")))).Returns(mockedConfigurationSections);
            
            return mockedConfiguration.Object;
        }

        public static TestConfig GenerateTestConfig()
            => new TestConfig {
                ConnectionStrings = new Dictionary<string, string> { { "Conn1", "TestConn1" }, { "Conn2", "TestConn2" } },
                TestInt = 5,
                TestString = "TestString"
            };


        private static IConfigurationSection GetMockedConnectionsSection(int connectionsNumber)
        {
            var mockedSections = new Mock<IConfigurationSection>();

            mockedSections.Setup(i => i.GetChildren()).Returns(MockedConnections(connectionsNumber));

            return mockedSections.Object;
        }

        private static IEnumerable<IConfigurationSection> MockedConnections(int connectionsNumber)
        {
            for (int i = 0; i < connectionsNumber; i++)
                yield return new TestConfigurationConnectionString(i);
        }

    }

    public class TestConfigurationConnectionString : IConfigurationSection
    {

        private string _key;
        private string _path;
        private string _value;

        public TestConfigurationConnectionString(int num)
        {
            _key = $"Connection{num}";
            _path = num.ToString();
            _value = $"TestConnectionString-{num}";
        }

        public string this[string key] { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public string Key => _key;
        public string Path => _path;
        public string Value { get => _value; set => _value = value; }

        public IEnumerable<IConfigurationSection> GetChildren()
        {
            throw new System.NotImplementedException();
        }

        public IChangeToken GetReloadToken()
        {
            throw new System.NotImplementedException();
        }

        public IConfigurationSection GetSection(string key)
        {
            throw new System.NotImplementedException();
        }
    }

    public class TestConfig
    {
        public IDictionary<string, string> ConnectionStrings { get; set; }
        public int TestInt { get; set; }
        public string TestString { get; set; }
    }

    public struct TestConstants
    {
        public const int TEST_CONFIG_INT = 7;
        public const string TEST_CONFIG_STRING = "TestChustaSoft";
    }

}
