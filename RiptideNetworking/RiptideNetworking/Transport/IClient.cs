﻿using System;

namespace RiptideNetworking.Transports
{
    /// <summary>Defines methods, properties, and events which every transport's client must implement.</summary>
    public interface IClient
    {
        /// <summary>Invoked when a connection to the server is established.</summary>
        event EventHandler Connected;
        /// <summary>Invoked when a connection to the server fails to be established.</summary>
        /// <remarks>This occurs when a connection request times out, either because no server is listening on the expected IP and port, or because something (firewall, antivirus, no/poor internet access, etc.) is preventing the connection.</remarks>
        event EventHandler ConnectionFailed;
        /// <summary>Invoked when a message is received from the server.</summary>
        event EventHandler<ClientMessageReceivedEventArgs> MessageReceived;
        /// <summary>Invoked when ping is updated.</summary>
        event EventHandler<PingUpdatedEventArgs> PingUpdated;
        /// <summary>Invoked when disconnected by the server.</summary>
        event EventHandler Disconnected;
        /// <summary>Invoked when a new client connects.</summary>
        event EventHandler<ClientConnectedEventArgs> ClientConnected;
        /// <summary>Invoked when a client disconnects.</summary>
        event EventHandler<ClientDisconnectedEventArgs> ClientDisconnected;

        /// <summary>The numeric ID of the client.</summary>
        ushort Id { get; }
        /// <summary>The round trip time of the connection. -1 if not calculated yet.</summary>
        short RTT { get; }
        /// <summary>The smoothed round trip time of the connection. -1 if not calculated yet.</summary>
        short SmoothRTT { get; }
        /// <summary>Whether or not the client is currently in the process of connecting.</summary>
        bool IsConnecting { get; }
        /// <summary>Whether or not the client is currently connected.</summary>
        bool IsConnected { get; }
        /// <summary>Whether or not to output informational log messages. Error-related log messages ignore this setting.</summary>
        bool ShouldOutputInfoLogs { get; set; }

        /// <summary>Attempts connect to the given host address.</summary>
        /// <param name="hostAddress">The host address to connect to.</param>
        void Connect(string hostAddress);
        /// <summary>Initiates handling of currently queued messages.</summary>
        /// <remarks>This should generally be called from within a regularly executed update loop (like FixedUpdate in Unity). Messages will continue to be received in between calls, but won't be handled fully until this method is executed.</remarks>
        void Tick();
        /// <summary>Sends a message to the server.</summary>
        /// <param name="message">The message to send.</param>
        /// <param name="maxSendAttempts">How often to try sending <paramref name="message"/> before giving up. Only applies to messages with their <see cref="Message.SendMode"/> set to <see cref="MessageSendMode.reliable"/>.</param>
        /// <param name="shouldRelease">Whether or not <paramref name="message"/> should be returned to the pool once its data has been sent.</param>
        void Send(Message message, byte maxSendAttempts, bool shouldRelease);
        /// <summary>Disconnects from the server.</summary>
        void Disconnect();
    }
}
