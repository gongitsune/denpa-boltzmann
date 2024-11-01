using System;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Projects.Scripts
{
    public class PythonInitializer : IDisposable
    {
        private Process _process;

        public void Dispose()
        {
            if (_process?.HasExited ?? false)
            {
                _process.Dispose();
                return;
            }
            _process?.Kill();
            _process?.Dispose();
        }

        public void Initialize()
        {
            _process = new Process();
            _process.StartInfo.FileName = $"{Application.dataPath}/Python/.venv/Scripts/python.exe";
            _process.StartInfo.Arguments = $"{Application.dataPath}/Python/app.py";
            _process.StartInfo.UseShellExecute = false;
            _process.StartInfo.RedirectStandardOutput = true;
            _process.StartInfo.RedirectStandardError = true;
            _process.StartInfo.CreateNoWindow = false;
            _process.OutputDataReceived += (sender, args) => Debug.Log(args.Data);
            _process.ErrorDataReceived += (sender, args) => Debug.LogError(args.Data);

            _process.Start();
        }
    }
}