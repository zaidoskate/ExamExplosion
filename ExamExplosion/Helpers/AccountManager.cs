using ExamExplosion.ExamExplotionService;
using ExamExplosion.Models;
using System;
using System.ServiceModel;

namespace ExamExplosion.Helpers
{
    /// <summary>
    /// Clase que gestiona las operaciones relacionadas con cuentas de usuario, como validación de credenciales,
    /// creación de cuentas y verificación de datos existentes.
    /// </summary>
    public class AccountManager
    {
        // Proxy para la comunicación con el servicio de autenticación.
        private static ExamExplotionService.AuthenticationManagerClient proxy = new ExamExplotionService.AuthenticationManagerClient();

        /// <summary>
        /// Valida las credenciales de un usuario con el servidor.
        /// </summary>
        /// <param name="gamertag">Nombre de usuario (gamertag).</param>
        /// <param name="password">Contraseña del usuario.</param>
        /// <returns>Verdadero si las credenciales son válidas, falso en caso contrario.</returns>
        public static bool ValidateCredentials(string gamertag, string password)
        {
            try
            {
                // Configura el objeto AccountManagement con las credenciales proporcionadas.
                var account = new ExamExplotionService.AccountManagement
                {
                    Gamertag = gamertag,
                    Password = password
                };
                using(proxy = new ExamExplotionService.AuthenticationManagerClient())
                {
                    // Envía las credenciales al servicio para su validación.
                    bool result = proxy.Login(account);
                    if (result)
                    {
                        // Si las credenciales son válidas, se carga la sesión actual del usuario.
                        LoadActualSession(gamertag);
                    }

                    return result;
                }
            }
            catch (FaultException faultException)
            {
                // Loguear error y lanzar excepción de fallo del servicio.
                throw faultException;
            }
            catch (CommunicationException communicationException)
            {
                // Loguear error y lanzar excepción de comunicación.
                throw communicationException;
            }
            catch (TimeoutException timeoutException)
            {
                // Loguear error y lanzar excepción de tiempo de espera.
                throw timeoutException;
            }
        }

        /// <summary>
        /// Carga la sesión actual del usuario tras una autenticación exitosa.
        /// </summary>
        /// <param name="gamertag">Gamertag del usuario autenticado.</param>
        private static void LoadActualSession(string gamertag)
        {
            try
            {
                // Se conecta al servicio para obtener la información del jugador.
                var playerProxy = new ExamExplotionService.PlayerManagerClient();
                var player = playerProxy.GetPlayerByGamertag(gamertag);

                // Actualiza los datos de la sesión actual.
                SessionManager.CurrentSession.accountId = player.AccountId;
                SessionManager.CurrentSession.userId = player.UserId;
                SessionManager.CurrentSession.gamertag = gamertag;
                SessionManager.CurrentSession.isGuest = false;
            }
            catch (FaultException faultException)
            {
                throw faultException;
            }
            catch (CommunicationException communicationException)
            {
                throw communicationException;
            }
            catch (TimeoutException timeoutException)
            {
                throw timeoutException;
            }
        }

        /// <summary>
        /// Crea una nueva cuenta de usuario.
        /// </summary>
        /// <param name="_account">Objeto de cuenta con los datos del usuario.</param>
        /// <returns>Verdadero si la cuenta fue creada exitosamente, falso en caso contrario.</returns>
        public static bool AddAccount(Account _account)
        {
            var account = new AccountManagement
            {
                Name = _account.name,
                Password = _account.password,
                Email = _account.email,
                Lastname = _account.lastname,
                Gamertag = _account.gamertag
            };

            try
            {
                return proxy.AddAccount(account);
            }
            catch (FaultException faultException)
            {
                throw faultException;
            }
            catch (CommunicationException communicationException)
            {
                throw communicationException;
            }
            catch (TimeoutException timeoutException)
            {
                throw timeoutException;
            }
        }

        /// <summary>
        /// Verifica si un correo electrónico ya está registrado.
        /// </summary>
        /// <param name="email">Correo electrónico a verificar.</param>
        /// <returns>Verdadero si el correo ya existe, falso en caso contrario.</returns>
        public static bool VerifyExistingEmail(string email)
        {
            try
            {
                return proxy.VerifyExistingEmail(email);
            }
            catch (FaultException faultException)
            {
                throw faultException;
            }
            catch (CommunicationException communicationException)
            {
                throw communicationException;
            }
            catch (TimeoutException timeoutException)
            {
                throw timeoutException;
            }
        }

        /// <summary>
        /// Verifica si un gamertag ya está registrado.
        /// </summary>
        /// <param name="gamertag">Gamertag a verificar.</param>
        /// <returns>Verdadero si el gamertag ya existe, falso en caso contrario.</returns>
        public static bool VerifyExistingGamertag(string gamertag)
        {
            try
            {
                return proxy.VerifyExistingGamertag(gamertag);
            }
            catch (FaultException faultException)
            {
                throw faultException;
            }
            catch (CommunicationException communicationException)
            {
                throw communicationException;
            }
            catch (TimeoutException timeoutException)
            {
                throw timeoutException;
            }
        }

        /// <summary>
        /// Actualiza la contraseña de un usuario.
        /// </summary>
        /// <param name="gamertag">Gamertag del usuario.</param>
        /// <param name="newPassword">Nueva contraseña.</param>
        /// <returns>Verdadero si la contraseña fue actualizada exitosamente, falso en caso contrario.</returns>
        internal static bool UpdatePassword(string gamertag, string newPassword)
        {
            try
            {
                var account = new AccountManagement
                {
                    Gamertag = gamertag,
                    Password = newPassword
                };
                return proxy.UpdatePassword(account);
            }
            catch (FaultException faultException)
            {
                throw faultException;
            }
            catch (CommunicationException communicationException)
            {
                throw communicationException;
            }
            catch (TimeoutException timeoutException)
            {
                throw timeoutException;
            }
        }

        public int GetAccountIdByGamertag(string gamertag)
        {
            try
            {
                return proxy.GetAccountIdByGamertag(gamertag);
            }
            catch (FaultException faultException)
            {
                throw faultException;
            }
            catch (CommunicationException communicationException)
            {
                throw communicationException;
            }
            catch (TimeoutException timeoutException)
            {
                throw timeoutException;
            }
        }

        public bool DeactivateAccount(string gamertag)
        {
            try
            {
                return proxy.DeactivateAccount(gamertag);
            }
            catch (FaultException faultException)
            {
                throw faultException;
            }
            catch (CommunicationException communicationException)
            {
                throw communicationException;
            }
            catch (TimeoutException timeoutException)
            {
                throw timeoutException;
            }
        }
    }
}
