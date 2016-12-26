﻿using System.Collections.Generic;
using Leak.Common;
using Leak.Omnibus.Strategies;

namespace Leak.Omnibus
{
    public abstract class OmnibusStrategy
    {
        public static OmnibusStrategy Sequential = new OmnibusStrategySequential();

        public static OmnibusStrategy RarestFirst = new OmnibusStrategyRarestFirst();

        public abstract void Next(ICollection<OmnibusBlock> blocks, OmnibusContext context, PeerHash peer, int count);
    }
}