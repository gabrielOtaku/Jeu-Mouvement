using OpenTK.Audio;
using OpenTK.Audio.OpenAL;
using System;

namespace BaseOpenTK
{
    class GestionAudio
    {
        AudioContext audioContex;
        int bufferMusique;
        int sourceMusique;
        FichierWAV fichierMusiquePrincipale;
        float volumeMusique; // [cite: 713]

        public GestionAudio()
        {
            audioContex = new AudioContext();
            // Assurez-vous que le fichier existe dans le dossier bin/Debug/audio/
            fichierMusiquePrincipale = new FichierWAV("./audio/DarkAtmosphere.wav");
            volumeMusique = 1.0f; // [cite: 715]
            AL.Listener(ALListenerf.Gain, volumeMusique); // [cite: 717]
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
            AL.Source(sourceMusique, ALSourceb.Looping, true); // Musique en boucle [cite: 505]
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