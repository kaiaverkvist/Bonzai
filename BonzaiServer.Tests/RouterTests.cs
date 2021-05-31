using System.Diagnostics;
using Bonzai.Networking;
using Bonzai.Routing;
using Fleck;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace BonzaiServer.Tests
{
    public class RouterTests
    {
        private Router _router;
        [SetUp]
        public void Setup()
        {
            _router = new Router();
        }

        private class TestNetworkMessage : BaseNetworkMessage
        {
            public string Identifier { get; set; }

            public TestNetworkMessage(string identifier) : base(null)
            {
                Identifier = identifier;
            }
        }
        
        [Test]
        public void Register_And_Trigger_Test()
        {
            string expectedIdentifier = "123";
            Assert.IsEmpty(_router.GetHandlerDictionary(), "Router handler dictionary must be empty");
            
            _router.Register<TestNetworkMessage>((message =>
            {
                Assert.AreEqual(expectedIdentifier, message.Identifier);
            }));
            Assert.IsNotEmpty(_router.GetHandlerDictionary());
            
            // Create a test network message and try to trigger it.
            TestNetworkMessage testMessage = new TestNetworkMessage(expectedIdentifier);
            int callCount = _router.Trigger(testMessage);
            
            // Now check that we triggered an handler.
            Assert.AreEqual(1, callCount, "Must have called one (1) handler");  
        }
    }
}