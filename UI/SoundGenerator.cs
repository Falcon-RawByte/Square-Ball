using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Storage;


namespace SquareBall
{
    static class SoundGenerator
    {
        private static SoundEffectInstance sound;
        private static SoundEffectInstance song;

        public static void LoadSong(SoundEffect _song)
        {
            song = _song.CreateInstance();
            song.IsLooped = true;
            song.Play();
        }

        public static bool StopSong;

        public static void ChangeSongVolume(float amount)
        {
            if (song.Volume + amount >= 1.0)
            {
                song.Volume = 1;
            }
            else if (song.Volume + amount <= 0)
            {
                song.Volume = 0;
            }
            else
            {
                song.Volume += amount;
            }
        }

        public static void Play(SoundEffect _sound, float pitch)
        {
            if ((sound == null)||(sound.State == SoundState.Stopped))
            {
                sound = _sound.CreateInstance();
                sound.Pitch = pitch;
                sound.Play();
            }
        }

        public static void Play(SoundEffect _sound, bool force, float pan)
        {
            Random rnd = new Random();
            
            if ((sound != null) && (force)) sound.Stop();
            sound = _sound.CreateInstance();
            sound.Pitch = (float)rnd.NextDouble();
            sound.Pan = pan;
            sound.Play();
        }

        public static bool Silent()
        {
            if (sound != null)
                return sound.State == SoundState.Stopped;
            else return true;
        }
    }


}
