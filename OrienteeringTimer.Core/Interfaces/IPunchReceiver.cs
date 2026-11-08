using System;
using System.Threading;
using System.Threading.Tasks;
using OrienteeringTimer.Core.Models;

namespace OrienteeringTimer.Core.Interfaces
{
    //testing.....
    public interface IPunchReceiver : IDisposable
    {
        /// <summary>
        /// 接受并成功解析出一条合法打卡数据时触发
        /// </summary>
        event Action<PunchRecord>? OnPunchReceived;

        /// <summary>
        /// 监听状态或错误日志回调
        /// </summary>
        event Action<string>? OnLog;

        /// <summary>
        /// 启动监听指定端口
        /// </summary>
        Task StartListeningAsync(int port = 8888, CancellationToken cancellationToken = default);

        /// <summary>
        /// 停止监听
        /// </summary>
        void StopListening();
    }
}