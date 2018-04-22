using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Speech.Synthesis;// La libreria para que la maquina pueda hablar
// se activa en: References/ (clic derecho) Add reference/y buscar: Speech/Ok

namespace Gideon
{   
    class Program
    {
        private static SpeechSynthesizer synth = new SpeechSynthesizer();

        static void Main(string[] args)
        {
            // Lista de mensajes que dira de forma random cuando la CPU esta por encima de 80%
            List<string> CPUwarningmessages = new List<string>();            
            CPUwarningmessages.Add("WARNING: Holy shit the CPU is about to catch fire!");
            CPUwarningmessages.Add("WARNING: Stop to downloading porn, you will kill the CPU");
            CPUwarningmessages.Add("WARNING: You will damage your CPU, asshole!");
            
            
            Random zoom = new Random();

            // La maquina lee la cadena de caracteres
            synth.Speak("Welcome to this program. My name is Gideon");

            #region Contadores de rendimiento
            // esto abre una especie de pestaña


            //  Se establece los datos que tomara de el pc, Informacion de procesador, memoria y tiempo de sistema
            PerformanceCounter rendimientoCpuCount = new PerformanceCounter("Processor Information", "% Processor Time", "_Total");
            rendimientoCpuCount.NextValue();
            PerformanceCounter rendimientoMemCount = new PerformanceCounter("Memory", "Available MBytes");
            rendimientoMemCount.NextValue();
            PerformanceCounter rendimientoTiempoCount = new PerformanceCounter("System", "System Up Time");
            rendimientoTiempoCount.NextValue();
            #endregion

            TimeSpan uptimeSpan = TimeSpan.FromSeconds(rendimientoTiempoCount.NextValue());
            string systemUptimeMessage = string.Format("La actividad del sistema es de {0} dias {1} horas {2} minutos {3} segundos",
            //string systemUptimeMessage = string.Format("The current system up time is {0} days {1} hours {2} minutes {3} seconds",
                (int)uptimeSpan.TotalDays,
                (int)uptimeSpan.Hours,
                (int)uptimeSpan.Minutes,
                (int)uptimeSpan.Seconds
                );
            //con el metodo Speak se llama a la cadena de caracteres que da la informacion del sistema 
            Speak(systemUptimeMessage, VoiceGender.Female, 2);

            int SpeedForce = 1;
            // switch para que solo habra una pestaña
            bool IsAlreadyOpened = false;

            while (true)
            {
                float porcentajeCorrienteCpu = rendimientoCpuCount.NextValue();
                float memoriaCorrienteDisponible = rendimientoMemCount.NextValue();

                //Console.WriteLine("CPU Load:         {0} %", porcentajeCorrienteCpu);
                //Console.WriteLine("Available Memory: {0} MB\n",memoriaCorrienteDisponible );
                Console.WriteLine("Carga de la CPU:         {0} %", porcentajeCorrienteCpu);
                Console.WriteLine("Memoria disponible: {0} MB\n", memoriaCorrienteDisponible);                

                // Solo habla cuando la CPU esta por encima del 80%  de uso
                if ( porcentajeCorrienteCpu > 80 )
                {
                    if (porcentajeCorrienteCpu == 100)
                    {                      
                        string cpuLoadVocalMessege = CPUwarningmessages[zoom.Next(3)];
                        Speak(cpuLoadVocalMessege, VoiceGender.Male, 4);
                    }

                    else
                    {   
                        //string cpuLoadVocalMessege = String.Format("The current CPU load is {0}percent", porcentajeCorrienteCpu);
                        string cpuLoadVocalMessege = String.Format("La carga de la corriente de la CPU  es {0}porciento", porcentajeCorrienteCpu);
                        Speak(cpuLoadVocalMessege, VoiceGender.Female, 2);
                    }
                }

                // Solo nos dice cuando la memoria esta por debajo de 1 Gb
                if (memoriaCorrienteDisponible < 1024)
                {
                    if (SpeedForce < 5)
                        SpeedForce++;
                    //string memAvailableVocalMessege = String.Format("The currently that you have is {0} MegaBytes of memory available", memoriaCorrienteDisponible);
                    string memAvailableVocalMessege = String.Format("La corriente que tiene es de  {0} MegaBytes de memoria disponible", memoriaCorrienteDisponible);                    
                    if(IsAlreadyOpened == false)
                    {
                       OpenWebside("https://www.youtube.com/watch?v=J17gbrBAHB0");
                       IsAlreadyOpened = true;
                    }
                    Speak(memAvailableVocalMessege, VoiceGender.Female, SpeedForce);
                }
                Thread.Sleep(1000);

            }
        }

        public static void Speak(string message, VoiceGender voiceGender)
        {
            synth.SelectVoiceByHints(voiceGender);
            synth.Speak(message);
        }

        public static void Speak(string message, VoiceGender voiceGender, int rate)
        {
            synth.Rate = rate;
            Speak(message, voiceGender);
        }
        // Metodo que permite abrir Website
        public static void OpenWebside(string URL)
        {
            Process p1 = new Process();
            p1.StartInfo.FileName = "chrome.exe";
            p1.StartInfo.Arguments = URL;
            p1.Start();

        }
    }
}
