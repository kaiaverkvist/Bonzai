using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Bonzai.Extensions;
using Bonzai.Networking;
using Newtonsoft.Json;

namespace Bonzai.Routing
{
    /// <summary>
    /// Router is responsible for packet routing internally, letting clients declare 
    /// </summary>
    public class Router
    {
        /// <summary>
        /// Holds the actual handler delegates and their message types
        /// </summary>
        private readonly Dictionary<Type, Delegate> _handlers;

        public Router()
        {
            // Initialize our handler dictionary which holds a reference to the payload handlers.
            _handlers = new Dictionary<Type, Delegate>();
        }
        
        /// <summary>
        /// Registers a new handler for a payload of type T.
        /// </summary>
        /// <param name="handler"></param>
        /// <typeparam name="T">Type of network message</typeparam>
        public void Register<T>(Action<T> handler) where T : INetworkMessage
        {
            _handlers.Add(typeof(T), handler);
        }

        /// <summary>
        /// Returns the handler dictionary.
        /// </summary>
        /// <returns>dictionary containing type and actual delegate</returns>
        public Dictionary<Type, Delegate> GetHandlerDictionary()
        {
            return _handlers;
        }

        /// <summary>
        /// Triggers a network message
        /// </summary>
        /// <param name="message">network message</param>
        /// <returns>counter of handlers called</returns>
        public int Trigger(object? message)
        {
            // Keep track of how many handlers were triggered.
            int callCount = 0;
            
            // Iterate through our handlers and call all the methods.
            foreach (var handlerPair in _handlers)
            {
                // Invokes message Action<T> if our type matches.
                if(message != null && message.GetType() == handlerPair.Key)
                {
                    var action = handlerPair.Value;
                    action.DynamicInvoke(message);
                    
                    // Increment our call counter so we keep track of how many handlers have been called.
                    callCount++;
                }
            }

            // Return the counter.
            return callCount;
        }

        /// <summary>
        /// Converts a json message with a payload type identifier to
        /// a valid Instance of a network message.
        /// Warns and returns if the message is invalid.
        /// </summary>
        /// <param name="payloadJson"></param>
        public int ParseAndTrigger(string payloadJson)
        {
            string[] payload = payloadJson.Split("|");
            string className = payload[0];
            
            // Actual json from the message:
            string json = payload[1];
            
            // Attempts to get a concrete message type according to the registered handlers.
            Type payloadType = GetPayloadType(className);
            
            // Don't do anything with unrecognized messages. Consumer should handle.
            if (payloadType == null)
            {
                return 0;
            }

            // Deserialize into 
            var instance = JsonConvert.DeserializeObject(json, payloadType);
            return Trigger(instance);
        }
        
        /// <summary>
        /// Attempts to parse the Payload type from a classname.
        /// This is a lookup against the handler dictionary.
        /// </summary>
        /// <param name="className"></param>
        /// <returns>type of payload</returns>
        public Type GetPayloadType(string className)
        {
            Type messageType = TypeExtensions.GetTypeFromAssemblies(className);

            return _handlers.FirstOrDefault(element => element.Key == messageType).Key;
        }
    }
}