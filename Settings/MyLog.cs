using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace SettingsUI
{
    public class MyLog : IDisposable
    {
        string _logFile, _logFilePrefix;

        FileStream _file;
        StreamWriter _writer;

        bool _debugEnabled;

        int _errors = 0;
        int _warnings = 0;

        public MyLog(string logFile, bool dailyMode = false, int keepLogsForDays = 10)
        {
            _logFile = logFile;

            Console.WriteLine("Log file location: " + logFile);

            if (dailyMode)
            {
                _logFilePrefix = Path.GetFileName(_logFile).Replace(".log.txt", "");
                _logFile = _logFile.Replace(".log.txt", GetDatePostfix(DateTime.Now) + ".log.txt");
                CleanupDailyLogs(keepLogsForDays);
            }

            _file = File.Open(_logFile, FileMode.Append, FileAccess.Write, FileShare.Read);
            _writer = new StreamWriter(_file);
            _writer.AutoFlush = true;

#if DEBUG
            _debugEnabled = true;
#endif
        }

        public int Errors { get { return _errors; } }
        public int Warnings { get { return _warnings; } }

        public string GetLogFileName() { return _logFile; }

        public void ResetCounters()
        {
            _errors = _warnings = 0;
        }
        static string GetDatePostfix(DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd");
        }

        void CleanupDailyLogs(int keepLogsForDays)
        {
            try
            {
                var path = Path.GetDirectoryName(_logFile) + "\\" + _logFilePrefix + "*.log.txt";
                foreach (var f in Directory.GetFiles(path))
                {
                    if ((DateTime.Now - File.GetCreationTime(f)).TotalDays > keepLogsForDays)
                        File.Delete(f);
                }
            }
            catch (Exception)
            {
                //
            }
        }

        public void Dispose()
        {
            _writer.Dispose();
            _writer = null;

            _file.Dispose();
            _file = null;
        }

        string TimePrefix
        {
            get
            {
                return DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss ");
            }
        }

        public string LogFileName { get { return _logFile; } }

        void WriteLog(string text)
        {
            _writer.WriteLine(text);
        }
        public void Info(string message)
        {
            WriteLog(TimePrefix + message);
        }

        public void Note(string message)
        {
            message = TimePrefix + "Note: " + message;

            WriteLog(message);
        }
        public void Warning(string message)
        {
            message = TimePrefix + "Warning: " + message;

            WriteLog(message);
            ++_warnings;
        }

        public void Error(string message, Exception ex = null)
        {
            message = TimePrefix + "Error: " + message;

            WriteLog(message);

            if (ex != null)
            {
                message = "Exception details: " + ex.Message;
                if (ex.InnerException != null)
                    message += Environment.NewLine + ex.InnerException.Message;
                if (ex.StackTrace != null)
                    message += Environment.NewLine + "Stack trace: " + ex.StackTrace;

                WriteLog(message);
            }

            ++_errors;
        }

        public void DebugLog(string message)
        {
            if (!_debugEnabled)
                return;

            message = TimePrefix + "Debug: " + message;

            WriteLog(message);
        }
    }
}