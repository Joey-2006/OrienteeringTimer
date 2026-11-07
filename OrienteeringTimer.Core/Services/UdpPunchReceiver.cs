using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using OrienteeringTimer.Core.Interfaces;
using OrienteeringTimer.Core.Models;

namespace OrienteeringTimer.Core.Services
{
    public class UdpPunchReceiver : IPunchReceiver
    {
        private UdpClient? _udpClient;
        private bool _isRunning;

        public event Action<PunchRecord>? OnPunchReceived;
        public event Action<string>? OnLog;

        public async Task StartListeningAsync(int port = 8888, CancellationToken cancellationToken = default)
        {
            if (_isRunning) return;

            try
            {
                _udpClient = new UdpClient(port);
                _isRunning = true;
                OnLog?.Invoke($"[Core] UDP 监听已在端口 {port} 启动...");

                while (_isRunning && !cancellationToken.IsCancellationRequested)
                {
                    // 异步等待接收数据报文
                    UdpReceiveResult result = await _udpClient.ReceiveAsync(cancellationToken);
                    string rawJson = Encoding.UTF8.GetString(result.Buffer);

                    try
                    {
                        var record = JsonSerializer.Deserialize<PunchRecord>(rawJson);
                        if (record != null)
                        {
                            OnPunchReceived?.Invoke(record);
                        }
                    }
                    catch (Exception ex)
                    {
                        OnLog?.Invoke($"[Core] JSON 解析失败: {ex.Message} | 原始数据: {rawJson}");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                OnLog?.Invoke("[Core] 监听已取消。");
            }
            catch (Exception ex)
            {
                OnLog?.Invoke($"[Core] UDP 监听异常: {ex.Message}");
            }
            finally
            {
                StopListening();
            }
        }

        public void StopListening()
        {
            _isRunning = false;
            _udpClient?.Close();
            _udpClient?.Dispose();
            _udpClient = null;
            OnLog?.Invoke("[Core] UDP 监听已停止。");
        }

        public void Dispose()
        {
            StopListening();
        }
    }
}
