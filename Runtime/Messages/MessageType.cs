using UnityEngine;


namespace Atelier.Messages {

    /// <summary>
    /// A static-objecty messaging system.
    /// 
    /// Creates static message types which can be used at run time to pass messages around between
    /// objects. 
    /// 
    /// Message are sent via game objects with the tag. A message type might have one or more 
    /// components that are supposed to "pair" with the game object to serve as the actual message
    /// content. 
    /// 
    /// The receiver component then dispatches to different unity events based on message type. The
    /// target of the unity event is responsible for knowing how to deal with the game object and 
    /// unpack it. 
    ///
    /// Yes, this does fuck with the decoupling this system was supposed to provide. One could
    /// probably do something clever with the receiver to dynamically figure out what component type
    /// the message receiver wants to deal with, but it goes fucky somewhere around when you're
    /// trying to serialize the message receivers. Some kind of editor/selector code that picks
    /// component types and converts them to strings or something could probably help, but I got
    /// lazy.
    /// 
    /// Should the Messages namespace have been combined with the Events namespace? Maybe.
    /// </summary>
    [CreateAssetMenu(fileName = "NewMessageType", menuName = "Atelier/Message Type")]
    public class MessageType : ScriptableObject { }

}
