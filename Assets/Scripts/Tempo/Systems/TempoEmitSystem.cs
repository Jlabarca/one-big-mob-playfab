using System.IO;
using Jlabarca.OneBigMob.Tempo.Components;
using Leopotam.Ecs;
using Midi;
using UnityEngine;

#pragma warning disable 649

namespace Jlabarca.OneBigMob.Tempo.Systems
{
    public class TempoEmitSystem : IEcsRunSystem, IEcsInitSystem
    {
        private const string MidiFilePath = "Assets/Tempo/Midi/grass4.mid";

        private readonly EcsWorld _world;
        private readonly AudioSource _audioSource;
        private readonly GameState _gameState;

        private MidiData _midiData;
        private float _startTime;
        private int _midiEventIndex;

        public void Init()
        {
            _midiData = GetMidiFile(MidiFilePath);
            _audioSource.Play();
            _audioSource.time = Time.time;
            _startTime = Time.time;
            Debug.Log(_startTime);
        }

        public void Run()
        {
            var time = _audioSource.time;
            _gameState.time = time;
            var preFireTime = time + _gameState.timeOffset;
            if (_midiEventIndex >= _midiData.soundEvents.Count) return;
            if (_midiData.soundEvents[_midiEventIndex].time >= preFireTime) return;
            var actorEntity = _world.NewEntity();

            actorEntity.Replace(new TempoEvent
            {
                SoundEvent = _midiData.soundEvents[_midiEventIndex],
                ReleaseTime = _gameState.time
            });

            _midiEventIndex++;
        }

        private static MidiData GetMidiFile(string fileName)
        {
            var path = $"{MidiFilePath}/{fileName}";
            var reader = File.OpenRead (MidiFilePath);
            var midiFile = FileParser.parse(reader);
            reader.Close();
            return midiFile;
        }
    }
}
