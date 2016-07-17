﻿using Leak.Core.Common;

namespace Leak.Core.Negotiator
{
    public interface HandshakeNegotiatorActiveContext : HandshakeNegotiatorContext
    {
        PeerHash Peer { get; }

        FileHash Hash { get; }

        HandshakeOptions Options { get; }
    }
}