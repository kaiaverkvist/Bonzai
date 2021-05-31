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

        private class FloatMessage : BaseNetworkMessage
        {
            public float X { get; set; }
            
            public FloatMessage(float x) : base(null)
            {
                this.X = x;
            }
        }
        
        [Test]
        public void Register_And_Trigger_Test()
        {
            string expectedIdentifier = "123";
            Assert.IsEmpty(_router.GetHandlerDictionary(), "Router handler dictionary must be empty");
            
            _router.Register<TestNetworkMessage>((message =>
            {
                TestContext.WriteLine("Triggered test message");
                Assert.AreEqual(expectedIdentifier, message.Identifier);
            }));
            Assert.IsNotEmpty(_router.GetHandlerDictionary());
            Assert.AreEqual(1, _router.GetHandlerDictionary().Count);
            
            // Create a test network message and try to trigger it.
            TestNetworkMessage testMessage = new TestNetworkMessage(expectedIdentifier);
            int callCount = _router.Trigger(testMessage);
            
            // Now check that we triggered an handler.
            Assert.AreEqual(1, callCount, "Must have called one (1) handler");  
        }
        
        [Test]
        public void Register_And_Trigger_Test_WithTwo()
        {
            string expectedIdentifier = "123";
            float expectedX = 2.4f;
            Assert.IsEmpty(_router.GetHandlerDictionary(), "Router handler dictionary must be empty");
            
            _router.Register<TestNetworkMessage>((message =>
            {
                TestContext.WriteLine("Triggered test message");
                Assert.AreEqual(expectedIdentifier, message.Identifier);
            }));
            _router.Register<FloatMessage>((message =>
            {
                TestContext.WriteLine("Triggered float message");
                Assert.AreEqual(expectedX, message.X);
            }));
            Assert.IsNotEmpty(_router.GetHandlerDictionary());
            Assert.AreEqual(2, _router.GetHandlerDictionary().Count);
            
            // Create a test network message and try to trigger it.
            TestNetworkMessage testMessage = new TestNetworkMessage(expectedIdentifier);
            int callCount = _router.Trigger(testMessage);
            
            // Now check that we triggered an handler.
            Assert.AreEqual(1, callCount, "Must have called one (1) handler");
            
            // Trigger our NO.2 message
            FloatMessage floatTestMessage = new FloatMessage(expectedX);
            callCount = _router.Trigger(floatTestMessage);
            Assert.AreEqual(1, callCount, "Must have called one (1) handler");
        }
        
        [Test]
        public void Trigger_No_Registered()
        {
            Assert.IsEmpty(_router.GetHandlerDictionary(), "Router handler dictionary must be empty");
            
            TestNetworkMessage testMessage = new TestNetworkMessage("test_xyz");
            int callCount = _router.Trigger(testMessage);
            
            // Now check that we triggered an handler.
            Assert.AreEqual(0, callCount, "Must have called one (0) handler");
        }
    }
}