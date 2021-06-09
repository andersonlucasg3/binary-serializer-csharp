using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace BinarySerializer.Logging
{
    internal static class Logger
    {
        internal static void Log(string message, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0) 
            => InternalLog(message, memberName, filePath, lineNumber);

        private static void InternalLog(string message, string memberName, string filePath, int lineNumber)
        {
            string finalMessage = $"[{Path.GetFileNameWithoutExtension(filePath)}::{memberName}:({lineNumber})] - {message}";
#if UNITY_EDITOR
            UnityEngine.Debug.Log(finalMessage);
#else
            Console.WriteLine(finalMessage);
#endif
        }
    }
}
