using System;
using System.Text.Json.Serialization;

namespace OrienteeringTimer.Core.Models
{
    public class PunchRecord
    {
        [JsonPropertyName("device_id")]
        public string DeviceId { get; set; } = string.Empty;
        
        [JsonPropertyName("checkpoint_id")]
        public string CheckpointId { get; set; } = string.Empty;

        [JsonPropertyName("timestamp")]
        public long TimeStamp { get; set; }

        [JsonPropertyName("rssi")]
        public int Rssi { get; set; }

        [JsonIgnore]
        public DateTime PunchTime => DateTimeOffset.FromUnixTimeMilliseconds(TimeStamp).LocalDateTime;

        public override string ToString()
        {
            return $"[{PunchTime:HH:mm:ss.fff}] Device: {DeviceId} | Checkpoint: {CheckpointId} | RSSI: {Rssi}";
        }
    }
}