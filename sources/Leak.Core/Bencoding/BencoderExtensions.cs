﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Leak.Core.Bencoding
{
    public static class BencoderExtensions
    {
        public static T Find<T>(this BencodedValue value, string name, Func<BencodedValue, T> selector)
        {
            if (value != null && value.Dictionary != null)
            {
                foreach (BencodedEntry entry in value.Dictionary)
                {
                    if (entry.Key.Text != null && entry.Key.Text.GetString() == name)
                    {
                        return selector.Invoke(entry.Value);
                    }
                }
            }

            return selector.Invoke(null);
        }

        public static string ToText(this BencodedValue value)
        {
            return AllTexts(value).FirstOrDefault();
        }

        public static long ToNumber(this BencodedValue value)
        {
            return value.Number.ToInt64();
        }

        public static BencodedValue[] AllItems(this BencodedValue value)
        {
            return AllItems(new List<BencodedValue>(), value.Array).ToArray();
        }

        private static List<BencodedValue> AllItems(List<BencodedValue> result, BencodedValue[] array)
        {
            if (array != null)
            {
                foreach (BencodedValue value in array)
                {
                    result.Add(value);
                }
            }

            return result;
        }

        public static string[] AllKeys(this BencodedValue value)
        {
            return AllKeys(new List<string>(), value).ToArray();
        }

        private static List<string> AllKeys(List<string> output, BencodedValue value)
        {
            AllKeys(output, value.Dictionary);
            AllKeys(output, value.Array);

            return output;
        }

        private static void AllKeys(List<string> output, BencodedValue[] array)
        {
            if (array != null)
            {
                foreach (BencodedValue value in array)
                {
                    AllKeys(output, value);
                }
            }
        }

        private static void AllKeys(List<string> output, BencodedEntry[] dictionary)
        {
            if (dictionary != null)
            {
                foreach (BencodedEntry entry in dictionary)
                {
                    output.Add(entry.Key.ToString());
                }

                foreach (BencodedEntry entry in dictionary)
                {
                    AllKeys(output, entry.Value);
                }
            }
        }

        public static string[] AllTexts(this BencodedValue value)
        {
            return AllTexts(new List<string>(), value).ToArray();
        }

        private static List<string> AllTexts(List<string> output, BencodedValue value)
        {
            if (value != null)
            {
                AllTexts(output, value.Text);
                AllTexts(output, value.Dictionary);
                AllTexts(output, value.Array);
            }

            return output;
        }

        private static void AllTexts(List<string> output, BencodedText text)
        {
            if (text != null)
            {
                output.Add(text.GetString());
            }
        }

        private static void AllTexts(List<string> output, BencodedValue[] array)
        {
            if (array != null)
            {
                foreach (BencodedValue value in array)
                {
                    AllTexts(output, value);
                }
            }
        }

        private static void AllTexts(List<string> output, BencodedEntry[] dictionary)
        {
            if (dictionary != null)
            {
                foreach (BencodedEntry entry in dictionary)
                {
                    AllTexts(output, entry.Value);
                }
            }
        }
    }
}