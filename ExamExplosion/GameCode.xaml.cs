﻿using ExamExplosion.ExamExplotionService;
using ExamExplosion.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ExamExplosion
{
    /// <summary>
    /// Lógica de interacción para GameCode.xaml
    /// </summary>
    public partial class GameCode : Page
    {
        private LobbyManager lobbyManager = null;
        

        public GameCode()
        {
            InitializeComponent();
            lobbyManager = new LobbyManager();
        }


        private void GoHome(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService != null)
            {
                this.NavigationService.Navigate(new HomePage());
                var window = Window.GetWindow(this);
                if (window != null)
                {
                    window.Height = 450;
                    window.Width = 800;
                    window.SizeToContent = SizeToContent.Manual;
                }
            }
        }

        private void NavigateToLobbyPage(string enteredCode)
        {
            if (this.NavigationService != null)
            {
                this.NavigationService.Navigate(new Lobby(enteredCode));
                var window = Window.GetWindow(this);
                if (window != null)
                {
                    window.Height = 450;
                    window.Width = 800;
                    window.SizeToContent = SizeToContent.Manual;
                }
            }
        }

        public bool JoinLobby(string code, string gamertag)
        {
            if (code.Length == 4)
            {
                try
                {
                    string player = SessionManager.CurrentSession.gamertag;
                    bool joined = lobbyManager.JoinLobby(code, player);
                    if (joined)
                    {
                        NavigateToLobbyPage(code);
                    }
                    else
                    {
                        new AlertModal("No existe tal lobby", "El código introducido no pertenece a ninguna lobby disponible").ShowDialog();
                    }
                    return joined;
                }
                catch (FaultException faultException)
                {
                    new AlertModal("Error", "Se produjo un error en el servidor").ShowDialog();
                    throw faultException;
                }
                catch (CommunicationException communicationException)
                {
                    new AlertModal("Error de comunicación", "No se pudo conectar con el servidor.").ShowDialog();
                    throw communicationException;
                }
                catch (TimeoutException timeoutException)
                {
                    new AlertModal("Tiempo de espera", "La conexión con el servidor ha expirado.").ShowDialog();
                    throw timeoutException;
                }
            }
            return false;
        }

        private void JoinClick(object sender, RoutedEventArgs e)
        {
            string enteredCode = lobbyCodeTxtBox.Text.ToUpper();
            JoinLobby(enteredCode, SessionManager.CurrentSession.gamertag);
        }
    }
}
