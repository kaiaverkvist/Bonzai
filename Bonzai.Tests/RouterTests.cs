using System;
using System.Diagnostics;
using Bonzai.Routing;
using Fleck;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Bonzai.Tests
{
    public class RouterTests
    {
        private WebSocketConnection _fakeConnection;
        private Router _router;
        
        [SetUp]
        public void Setup()
        {
            _router = new Router();
            _fakeConnection = new WebSocketConnection(null, null, null, null, null);
        }
        
        [Test]
        public void Register_And_Trigger_Test()
        {
            string expectedIdentifier = "123";
            Assert.IsEmpty(_router.GetHandlerDictionary(), "Router handler dictionary must be empty");
            
            _router.Register<TestNetworkMessage>((sender, message) =>
            {
                TestContext.WriteLine("Triggered test message");
                Assert.AreEqual(expectedIdentifier, message.Identifier);
            });
            Assert.IsNotEmpty(_router.GetHandlerDictionary());
            Assert.AreEqual(1, _router.GetHandlerDictionary().Count);
            
            // Create a test network message and try to trigger it.
            TestNetworkMessage testMessage = new TestNetworkMessage(expectedIdentifier);
            int callCount = _router.Trigger(_fakeConnection, testMessage);
            
            // Now check that we triggered an handler.
            Assert.AreEqual(1, callCount, "Must have called one (1) handler");  
        }
        
        [Test]
        public void Register_And_Trigger_Test_WithTwo()
        {
            string expectedIdentifier = "123";
            float expectedX = 2.4f;
            Assert.IsEmpty(_router.GetHandlerDictionary(), "Router handler dictionary must be empty");
            
            _router.Register<TestNetworkMessage>((sender, message) =>
            {
                TestContext.WriteLine("Triggered test message");
                Assert.AreEqual(expectedIdentifier, message.Identifier);
            });
            _router.Register<FloatNetworkMessage>((sender, message) =>
            {
                TestContext.WriteLine("Triggered float message");
                Assert.AreEqual(expectedX, message.X);
            });
            Assert.IsNotEmpty(_router.GetHandlerDictionary());
            Assert.AreEqual(2, _router.GetHandlerDictionary().Count);
            
            // Create a test network message and try to trigger it.
            TestNetworkMessage testMessage = new TestNetworkMessage(expectedIdentifier);
            int callCount = _router.Trigger(_fakeConnection, testMessage);
            
            // Now check that we triggered an handler.
            Assert.AreEqual(1, callCount, "Must have called one (1) handler");
            
            // Trigger our NO.2 message
            FloatNetworkMessage floatTestMessage = new FloatNetworkMessage(expectedX);
            callCount = _router.Trigger(_fakeConnection, floatTestMessage);
            Assert.AreEqual(1, callCount, "Must have called one (1) handler");
        }
        
        [Test]
        public void Trigger_No_Registered()
        {
            Assert.IsEmpty(_router.GetHandlerDictionary(), "Router handler dictionary must be empty");
            
            TestNetworkMessage testMessage = new TestNetworkMessage("test_xyz");
            int callCount = _router.Trigger(new WebSocketConnection(null, null, null, null, null), testMessage);
            
            // Now check that we triggered an handler.
            Assert.AreEqual(0, callCount, "Must have called one (0) handler");
        }
        
        
        [Test]
        public void Parse_And_Trigger_Test()
        {
            string expectedIdentifier = "123";
            Assert.IsEmpty(_router.GetHandlerDictionary(), "Router handler dictionary must be empty");
            
            _router.Register<TestNetworkMessage>((sender, message) =>
            {
                TestContext.WriteLine("Triggered test message");
                Assert.AreEqual(expectedIdentifier, message.Identifier);
            });
            Assert.IsNotEmpty(_router.GetHandlerDictionary());
            Assert.AreEqual(1, _router.GetHandlerDictionary().Count);
            
            // Create a test network message and try to trigger it.
            TestNetworkMessage testMessage = new TestNetworkMessage(expectedIdentifier);
            Type messageType = testMessage.GetType();
            string json = JsonConvert.SerializeObject(testMessage);
            string payload =  $"{messageType}|{json}";
            int callCount = _router.ParseAndTrigger(_fakeConnection, payload);
            Debug.WriteLine(json, payload);
            
            // Now check that we triggered an handler.
            Assert.AreEqual(1, callCount, "Must have called one (1) handler");  
        }

        [Test]
        public void Test_GetPayloadType()
        {
            _router.Register<TestNetworkMessage>((sender, d) => {});
            
            TestNetworkMessage testMessage = new TestNetworkMessage("123");
            Type expectedPayloadType = testMessage.GetType();
            
            string messageJson = "Bonzai.Tests.TestNetworkMessage|{\"Identifier\":\"123\"}";
            Type payloadType = _router.GetPayloadType(messageJson.Split("|")[0]);
            
            Assert.AreEqual(expectedPayloadType, payloadType, "Payload type must be equal to expected type");
        }
    }
}