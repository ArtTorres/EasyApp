﻿using MagnetArgs;
using EasyApp.Events;
using System;

namespace EasyApp.Parsers
{
    public class MessagePriorityParser : IParser
    {
        public object Parse(string value)
        {
            switch (value.ToLowerInvariant())
            {
                case "high":
                    return Priority.High;
                case "medium":
                    return Priority.Medium;
                case "low":
                    return Priority.Low;
                default:
                    throw new Exception(string.Format("Value {0} for MessagePriority not found.", value));
            }
        }
    }
}
