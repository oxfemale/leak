﻿using Leak.Core.Collector;
using Leak.Core.Retriever;
using System;

namespace Leak.Core.Scheduler
{
    public class SchedulerContext
    {
        private readonly SchedulerConfiguration configuration;
        private readonly SchedulerQueue queue;

        public SchedulerContext(Action<SchedulerConfiguration> configurer)
        {
            configuration = configurer.Configure(with =>
            {
                with.Callback = new SchedulerCallbackNothing();
                with.Strategy = RetrieverStrategy.RarestFirst;
            });

            queue = new SchedulerQueue(this);
        }

        public SchedulerCallback Callback
        {
            get { return configuration.Callback; }
        }

        public PeerCollector Collector
        {
            get { return configuration.Collector; }
        }

        public RetrieverStrategy Strategy
        {
            get { return configuration.Strategy; }
        }

        public SchedulerQueue Queue
        {
            get { return queue; }
        }
    }
}