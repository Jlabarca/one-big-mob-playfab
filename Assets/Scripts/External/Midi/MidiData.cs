/*
Copyright (c) 2013 Christoph Fabritz

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/

using System.Collections.Generic;
using TracksIn = System.Collections.Generic.IEnumerable<Midi.Chunks.TrackChunk>;
using Tracks = System.Collections.Generic.List<Midi.Chunks.TrackChunk>;
using HeaderChunk = Midi.Chunks.HeaderChunk;
using TrackChunk = Midi.Chunks.TrackChunk;
using System.Linq;
using Midi.Events.MetaEvents;
using UnityEngine;

namespace Midi
{
    public class MidiData
    {
        public readonly HeaderChunk header;
        public readonly TracksIn tracks;
        public List<SoundEvent> soundEvents;

        public MidiData(HeaderChunk header, TracksIn tracks)
        {
            this.header = header;
            this.tracks = tracks.ToList().AsReadOnly();
            soundEvents = ParseSoundEvents();
        }

        private List<SoundEvent> ParseSoundEvents()
        {
            soundEvents = new List<SoundEvent>();
            var cont = 0;
            var events = tracks.ToList()[1].events.ToList();
            var header = tracks.ToList()[0].events.ToList();
            var ppq = 196;
            var bpm = 130f;
            foreach (var midiEvent in header)
            {
                switch (midiEvent)
                {
                    case SetTempoEvent @event:
                        //bpm = 60_000_000f/@event.tempo;
                        break;
                    case TimeSignatureEvent timeSignature:
                        ppq = timeSignature.metronome_pulse * timeSignature.numerator;
                        break;
                }
            }

            Debug.Log("PPQ: "+ppq);
            Debug.Log("BPM: "+bpm);
            // ReSharper disable once UselessBinaryOperation
            var freq = 60f / (ppq * bpm);
            Debug.Log("FRQ: "+freq);

            for (var i = 0; i < events.Count; i++)
            {
                var midiEvent = events[i];
                cont += midiEvent.delta_time;
                if (midiEvent.event_type != 144) continue;

                var midiEventEnd = events[++i];
                var duration = midiEventEnd.delta_time;
                var soundEvent = new SoundEvent(midiEvent.delta_time)
                {
                    duration = duration * freq,
                    time = cont * freq
                };

                cont += duration;
                soundEvents.Add(soundEvent);
            }

            Debug.Log("events.Count: "+events.Count);
            Debug.Log("soundEvents.Count: "+soundEvents.Count);

            return soundEvents;
        }

        public override string ToString()
        {
            var tracks_string = tracks.Aggregate("", (a, b) => a + b + ", ");
            tracks_string = tracks_string.Remove(tracks_string.Length - 2);

            return "MidiFile(header: " + header + ", tracks: [" + tracks_string + "]) sounds: [" + soundEvents.Count + "])";
        }
    }
}
