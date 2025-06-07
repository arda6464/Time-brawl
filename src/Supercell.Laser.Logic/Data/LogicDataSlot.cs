using System.Collections.Generic;
using Supercell.Laser.Titan.DataStream;

namespace Supercell.Laser.Logic.Data
{
    public class LogicDataSlot
    {
        private readonly DataSlot _slotType;
        private readonly Dictionary<string, object> _data;

        public LogicDataSlot(DataSlot slotType)
        {
            _slotType = slotType;
            _data = new Dictionary<string, object>();
        }

        public void SetString(string key, string value)
        {
            _data[key] = value;
        }

        public void SetInt(string key, int value)
        {
            _data[key] = value;
        }

        public void SetBoolean(string key, bool value)
        {
            _data[key] = value;
        }

        public string GetString(string key)
        {
            return _data.TryGetValue(key, out var value) ? value as string : null;
        }

        public int GetInt(string key)
        {
            return _data.TryGetValue(key, out var value) ? (int)value : 0;
        }

        public bool GetBoolean(string key)
        {
            return _data.TryGetValue(key, out var value) && (bool)value;
        }

        public void Encode(ByteStream stream)
        {
            stream.WriteVInt((int)_slotType);
            stream.WriteVInt(_data.Count);

            foreach (var kvp in _data)
            {
                stream.WriteString(kvp.Key);
                
                if (kvp.Value is string strValue)
                {
                    stream.WriteVInt(1); // String type
                    stream.WriteString(strValue);
                }
                else if (kvp.Value is int intValue)
                {
                    stream.WriteVInt(2); // Int type
                    stream.WriteVInt(intValue);
                }
                else if (kvp.Value is bool boolValue)
                {
                    stream.WriteVInt(3); // Boolean type
                    stream.WriteBoolean(boolValue);
                }
            }
        }
    }
} 