using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace AmpleChatServer.Services.Packets {
    public enum PacketType {
        NULL, 
        ERROR,
        INFO_PACKET,
        REGISER_PACKET,
        REGISER_RESPONSE,
        LOGIN_PACKET,
        LOGIN_RESPONSE,
    }

    public class Packet { // struct???

        internal Dictionary<string, object> PacketData { get; set; } = new();

        internal Packet(Dictionary<string, object> keyValuePairs) {
            PacketData = keyValuePairs;
        }

        public static Packet ParsePacket(string json) {
            try {
                return new(JsonConvert.DeserializeObject<Dictionary<string, object>>(json));
            }
            catch {

                return null;
            }
        }

        public Packet(PacketType id) {
            PacketData["id"] = id;
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
            
            var id = int.Parse(PacketData["id"].ToString());

            if (id == null)
                return PacketType.NULL;

            return (PacketType)id;
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
