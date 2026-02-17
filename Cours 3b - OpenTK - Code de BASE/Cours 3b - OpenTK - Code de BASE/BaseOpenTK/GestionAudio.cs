using OpenTK.Audio;
using OpenTK.Audio.OpenAL;
using System;

namespace BaseOpenTK
{
    class GestionAudio
    {
        private const string myAudio = "./audio/DarkAtmosphere.wav";
        AudioContext audioContex;
        int bufferMusique;
        int sourceMusique;
        FichierWAV fichierMusiquePrincipale;
        float volumeMusique; 

        public GestionAudio()
        {
            audioContex = new AudioContext();
            fichierMusiquePrincipale = new FichierWAV(myAudio);
            volumeMusique = 1.0f; 
            AL.Listener(ALListenerf.Gain, volumeMusique); 
            init();
        }

        private void init()
        {
            bufferMusique = AL.GenBuffer();
            sourceMusique = AL.GenSource();

            AL.BufferData(bufferMusique, fichierMusiquePrincipale.getFormatSonAL(),
                fichierMusiquePrincipale.getDonneesSonores(),
                fichierMusiquePrincipale.getQteDonneesSonores(),
                fichierMusiquePrincipale.getFrequence());

            AL.Source(sourceMusique, ALSourcei.Buffer, bufferMusique);
            AL.Source(sourceMusique, ALSourceb.Looping, true); // Musique en boucle 
        }

        public void demarrerMusiqueDeFond()
        {
            AL.SourcePlay(sourceMusique);
        }

        public void setVolumeMusique(int nouveauVolume)
        {
            // Conversion de 0-100 vers 0.0-1.0
            volumeMusique = (float)nouveauVolume / 100.0f;
            AL.Listener(ALListenerf.Gain, volumeMusique);
        }

        ~GestionAudio()
        {
            AL.SourceStop(sourceMusique);
            AL.DeleteSource(sourceMusique);
            AL.DeleteBuffer(bufferMusique);
        }
    }
}