using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace ExamExplosion.Helpers
{
    /// <summary>
    /// Administra las funcionalidades relacionadas con los jugadores, incluyendo la obtención de puntos y tablas de clasificación.
    /// </summary>
    public class PlayerManager
    {
        /// <summary>
        /// Obtiene la cantidad de puntos acumulados por un jugador específico, identificado por su ID.
        /// </summary>
        /// <param name="playerId">El identificador único del jugador.</param>
        /// <returns>El número de puntos acumulados por el jugador.</returns>
        /// <exception cref="FaultException">Se produce cuando el servidor devuelve un error relacionado con el servicio.</exception>
        /// <exception cref="CommunicationException">Se produce cuando hay un problema de comunicación con el servicio.</exception>
        /// <exception cref="TimeoutException">Se produce si la solicitud al servicio excede el tiempo de espera establecido.</exception>
        public static int GetPointsByPlayerId(int playerId)
        {
            try
            {
                using (var proxy = new ExamExplotionService.PlayerManagerClient())
                {
                    return proxy.GetPoints(playerId);
                }
            }
            catch (FaultException faultException)
            {
                // Implementar log
                throw;
            }
            catch (CommunicationException communicationException)
            {
                // Implementar log
                throw;
            }
            catch (TimeoutException timeoutException)
            {
                // Implementar log
                throw;
            }
        }

        /// <summary>
        /// Obtiene la tabla de clasificación global de los jugadores, mostrando sus gamertags y sus puntos acumulados.
        /// </summary>
        /// <returns>Un diccionario donde la clave es el gamertag del jugador y el valor son sus puntos.</returns>
        /// <exception cref="FaultException">Se produce cuando el servidor devuelve un error relacionado con el servicio.</exception>
        /// <exception cref="CommunicationException">Se produce cuando hay un problema de comunicación con el servicio.</exception>
        /// <exception cref="TimeoutException">Se produce si la solicitud al servicio excede el tiempo de espera establecido.</exception>
        public static Dictionary<string, int> GetGlobalLeaderboard()
        {
            try
            {
                using (var proxy = new ExamExplotionService.PlayerManagerClient())
                {
                    return proxy.GetGlobalLeaderboard();
                }
            }
            catch (FaultException faultException)
            {
                // Implementar log
                Console.WriteLine($"FaultException: {faultException.Message}");
                throw;
            }
            catch (CommunicationException communicationException)
            {
                // Implementar log
                Console.WriteLine($"CommunicationException: {communicationException.Message}");
                throw;
            }
            catch (TimeoutException timeoutException)
            {
                // Implementar log
                Console.WriteLine($"TimeoutException: {timeoutException.Message}");
                throw;
            }
        }

        /// <summary>
        /// Obtiene la tabla de clasificación de amigos de un jugador específico.
        /// </summary>
        /// <param name="playerId">El identificador único del jugador.</param>
        /// <returns>Un diccionario donde la clave es el gamertag de un amigo y el valor son sus puntos.</returns>
        /// <exception cref="FaultException">Se produce cuando el servidor devuelve un error relacionado con el servicio.</exception>
        /// <exception cref="CommunicationException">Se produce cuando hay un problema de comunicación con el servicio.</exception>
        /// <exception cref="TimeoutException">Se produce si la solicitud al servicio excede el tiempo de espera establecido.</exception>
        public static Dictionary<string, int> GetFriendsLeaderboard(int playerId)
        {
            try
            {
                using (var proxy = new ExamExplotionService.PlayerManagerClient())
                {
                    return proxy.GetFriendsLeaderboard(playerId);
                }
            }
            catch (FaultException faultException)
            {
                // Implementar log
                throw faultException;
            }
            catch (CommunicationException communicationException)
            {
                // Implementar log
                throw communicationException;
            }
            catch (TimeoutException timeoutException)
            {
                // Implementar log
                throw timeoutException;
            }
        }
    }
}
