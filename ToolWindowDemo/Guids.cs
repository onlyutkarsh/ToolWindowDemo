// Guids.cs
// MUST match guids.h
using System;

namespace Utkarsh.ToolWindowDemo
{
    static class GuidList
    {
        public const string guidToolWindowDemoPkgString = "479547a3-a14a-47d1-ba47-898417117388";
        public const string guidToolWindowDemoCmdSetString = "5cafacc5-a498-461d-bcda-2b1d53b96459";

        public static readonly Guid guidToolWindowDemoCmdSet = new Guid(guidToolWindowDemoCmdSetString);
    };
}