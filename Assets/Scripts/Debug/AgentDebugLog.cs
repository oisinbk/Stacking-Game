using System;
using System.IO;
using UnityEngine;

namespace DebugTools
{
    public static class AgentDebugLog
    {
        private static readonly string LogPath = Path.GetFullPath(
            Path.Combine(Application.dataPath, "..", "debug-a6cd1e.log"));

        // #region agent log
        public static void Write(string location, string message, string hypothesisId, string runId = "pre-fix", string dataJson = "{}")
        {
            try
            {
                long ts = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                string line =
                    $"{{\"sessionId\":\"a6cd1e\",\"timestamp\":{ts},\"location\":\"{location}\",\"message\":\"{message}\",\"hypothesisId\":\"{hypothesisId}\",\"runId\":\"{runId}\",\"data\":{dataJson}}}\n";
                File.AppendAllText(LogPath, line);
            }
            catch
            {
                // ignore logging failures
            }
        }
        // #endregion
    }
}
