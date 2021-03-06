﻿using System;
using System.Threading;

namespace Leak.Testing
{
    public abstract class Trigger
    {
        public abstract bool Wait();

        public static Trigger Bind<T>(ref Action<T> target)
        {
            Trigger<T> trigger = new Trigger<T>(data => { });
            Trigger result = trigger;

            target = target + trigger;
            return result;
        }

        public static Trigger Bind<T>(ref Action<T> target, Action<T> callback)
        {
            Trigger<T> trigger = new Trigger<T>(callback);
            Trigger result = trigger;

            target = target + trigger;
            return result;
        }

        public static Trigger Bind<T>(ref Action<T> target, Func<T, bool> callback)
        {
            Trigger<T> trigger = new Trigger<T>(callback);
            Trigger result = trigger;

            target = target + trigger;
            return result;
        }
    }

    public class Trigger<T> : Trigger
    {
        private readonly Action<T> callback;
        private bool called;

        public Trigger(Action<T> callback)
        {
            this.callback = payload =>
            {
                callback.Invoke(payload);
                called = true;
            };
        }

        public Trigger(Func<T, bool> callback)
        {
            this.callback = payload =>
            {
                if (callback.Invoke(payload))
                {
                    called = true;
                }
            };
        }

        public override bool Wait()
        {
            for (int i = 0; i < 500; i++)
            {
                if (called == false)
                {
                    Thread.Sleep(20);
                }
            }

            return called;
        }

        public static implicit operator Action<T>(Trigger<T> trigger)
        {
            return trigger.callback;
        }
    }
}