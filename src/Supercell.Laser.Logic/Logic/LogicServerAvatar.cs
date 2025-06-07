using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supercell.Laser.Logic.Avatar;
using Supercell.Laser.Logic.Data;
using Supercell.Laser.Logic.Helper;
using Supercell.Laser.Titan.DataStream;

namespace Supercell.Laser.Logic
{
    public class LogicServerAvatar : ClientAvatar
    {
        private int _tickCount = 0;
        private readonly List<LogicDataSlot> _dataSlots;

        public LogicServerAvatar()
        {
            _dataSlots = new List<LogicDataSlot>();
        }

        public void Tick()
        {
            _tickCount++;
        }

        public void AddDataSlot(LogicDataSlot slot)
        {
            // Aynı tipteki eski slot'u kaldır
            _dataSlots.RemoveAll(s => s.GetType() == slot.GetType());
            _dataSlots.Add(slot);
        }

        public void EncodeDataSlots(ByteStream stream)
        {
            stream.WriteVInt(_dataSlots.Count);
            foreach (var slot in _dataSlots)
            {
                slot.Encode(stream);
            }
        }
    }
} 