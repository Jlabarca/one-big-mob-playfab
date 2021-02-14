using System;
using Midi.Events;

namespace Midi
{
    public class SoundEvent : MidiEvent
    {
        public float duration;
        public float time;

        public SoundEvent(int delta_time, byte event_type = Byte.MaxValue) : base(delta_time, event_type) {}

        public override string ToString()
        {
            return "SoundEvent(time: " + time + ", duration: [" + duration + "]) " + base.ToString();
        }
    }
}
