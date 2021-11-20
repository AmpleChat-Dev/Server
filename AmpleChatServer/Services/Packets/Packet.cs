using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace AmpleChatServer.Services.Packets {
    public enum PacketType {
        NULL = -1, 
        ERROR = -2,
        INFO_PACKET = 0,
        REGISER_PACKET = 1,
        REGISER_RESPONSE = 2,
        LOGIN_PACKET = 3,
        LOGIN_RESPONSE = 4,
    }

    public class Packet { // struct???

        internal Dictionary<string, object> PacketData { get; set; } = new();

        internal Packet(Dictionary<string, object> keyValuePairs) {
            PacketData = keyValuePairs;
        }

        public static Packet ParsePacket(string json) 
            => new(JsonConvert.DeserializeObject<Dictionary<string, object>>(json));

        public Packet(PacketType id) {
            PacketData["id"] = (int)id;
            PacketData["timepstampe"] = DateTime.Now.ToString(new CultureInfo("en-GB"));
        }

        public Packet Add(string key, object data) {
            PacketData[key] = data;
            return this;
        }

        public T Get<T>(string key) {

            if (PacketData.ContainsKey(key)) {
                return (T) PacketData[key];
            }

            return default;
        }

        public string Get(string key) {
            return Get<string>(key);
        }

        public PacketType GetPacketId() {

            if (Enum.TryParse(Get("id"), out PacketType packetType))
                return packetType;

            return PacketType.NULL;
        }

        public override string ToString() {
            return JsonConvert.SerializeObject(PacketData);
        }

        public class ErrorPacket : Packet {
            public ErrorPacket() : base(PacketType.ERROR) { }

            public Packet Message(string message) {
                Add("error_message", message);
                return this;
            }
        }
    }
}
