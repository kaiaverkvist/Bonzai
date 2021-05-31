using System;
using System.Collections.Generic;
using System.Reflection;
using Bonzai.Networking;

namespace Bonzai.Routing
{
    /// <summary>
    /// Router is responsible for packet routing internally, letting clients declare 
    /// </summary>
    public class Router
    {
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
        public int Trigger(BaseNetworkMessage message)
        {
            // Keep track of how many handlers were triggered.
            int callCount = 0;
            
            // Iterate through our handlers and call all the methods.
            foreach (var handlerPair in _handlers)
            {
                // Invokes message Action<T> if our type matches.
                if(message.GetType() == handlerPair.Key)
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
    }
}