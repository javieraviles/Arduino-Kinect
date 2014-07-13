//------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.Samples.Kinect.ControlsBasics
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;

    using System.Linq;
    using System.Text;


    using System.Net;
    using System.Net.Sockets;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;
    using Microsoft.Kinect;
    using Microsoft.Kinect.Toolkit;
    using Microsoft.Kinect.Toolkit.Controls;

    /// <summary>
    /// Interaction logic for MainWindow
    /// </summary>
    public partial class MainWindow
    {
        
        private const double ScrollErrorMargin = 0.001;

        private const int PixelScrollByAmount = 20;

        private readonly KinectSensorChooser sensorChooser;

        string ip = "192.168.1.40";
  
        public MainWindow()
        {
            this.InitializeComponent();

            // initialize the sensor chooser and UI
            this.sensorChooser = new KinectSensorChooser();
            this.sensorChooser.KinectChanged += SensorChooserOnKinectChanged;
            this.sensorChooserUi.KinectSensorChooser = this.sensorChooser;
            this.sensorChooser.Start();

            // Bind the sensor chooser's current sensor to the KinectRegion
            var regionSensorBinding = new Binding("Kinect") { Source = this.sensorChooser };
            BindingOperations.SetBinding(this.kinectRegion, KinectRegion.KinectSensorProperty, regionSensorBinding);


            // Bind listner to scrollviwer scroll position change, and check scroll viewer position

        }

    


        /// <summary>
        /// Called when the KinectSensorChooser gets a new sensor
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="args">event arguments</param>
        private static void SensorChooserOnKinectChanged(object sender, KinectChangedEventArgs args)
        {
            if (args.OldSensor != null)
            {
                try
                {
                    args.OldSensor.DepthStream.Range = DepthRange.Default;
                    args.OldSensor.SkeletonStream.EnableTrackingInNearRange = false;
                    args.OldSensor.DepthStream.Disable();
                    args.OldSensor.SkeletonStream.Disable();
                }
                catch (InvalidOperationException)
                {
                    // KinectSensor might enter an invalid state while enabling/disabling streams or stream features.
                    // E.g.: sensor might be abruptly unplugged.
                }
            }

            if (args.NewSensor != null)
            {
                try
                {
                    args.NewSensor.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);
                    args.NewSensor.SkeletonStream.Enable();

                    try
                    {
                        args.NewSensor.DepthStream.Range = DepthRange.Near;
                        args.NewSensor.SkeletonStream.EnableTrackingInNearRange = true;
                    }
                    catch (InvalidOperationException)
                    {
                        // Non Kinect for Windows devices do not support Near mode, so reset back to default mode.
                        args.NewSensor.DepthStream.Range = DepthRange.Default;
                        args.NewSensor.SkeletonStream.EnableTrackingInNearRange = false;
                    }
                }
                catch (InvalidOperationException)
                {
                    // KinectSensor might enter an invalid state while enabling/disabling streams or stream features.
                    // E.g.: sensor might be abruptly unplugged.
                }
            }
        }

        /// <summary>
        /// Execute shutdown tasks
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.sensorChooser.Stop();
        }



        private void PageRightButtonClick(object sender, RoutedEventArgs e)
        {
            Socket miPrimerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint miDireccion = new IPEndPoint(IPAddress.Parse(ip), 80);
            try
            {

 

                byte[] datos ={ 82, 73, 71, 72, 84, 13, 10, 13, 10 };

                byte[] recibido = new byte[500];
                miPrimerSocket.Connect(miDireccion); // Conectamos               

                miPrimerSocket.Send(datos);

                miPrimerSocket.Receive(recibido);

                miPrimerSocket.Close();
                
                right.IsEnabled = false;
                left.IsEnabled = true;
                forward.IsEnabled = true;
                back.IsEnabled = true;
                stop.IsEnabled = true;
            }
            catch (Exception error)
            {

            }
         }


        private void PageLeftButtonClick(object sender, RoutedEventArgs e)
        {
            Socket miPrimerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint miDireccion = new IPEndPoint(IPAddress.Parse(ip), 80);

            try
            {

                byte[] datos =  { 76, 69, 70, 84, 84, 13, 10, 13, 10 };
                byte[] recibido = new byte[500];
                miPrimerSocket.Connect(miDireccion); // Conectamos               

                miPrimerSocket.Send(datos);

                miPrimerSocket.Receive(recibido);


                miPrimerSocket.Close();
                right.IsEnabled = true;
                left.IsEnabled = false;
                forward.IsEnabled = true;
                back.IsEnabled = true;
                stop.IsEnabled = true;
            }
            catch (Exception error)
            {

            }
        }
        private void PageUpButtonClick(object sender, RoutedEventArgs e)
        {
            Socket miPrimerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint miDireccion = new IPEndPoint(IPAddress.Parse(ip), 80);
            try
            {



                byte[] datos = { 85, 80, 80, 80, 80, 13, 10, 13, 10 };

                byte[] recibido = new byte[500];
                miPrimerSocket.Connect(miDireccion); // Conectamos               

                miPrimerSocket.Send(datos);

                miPrimerSocket.Receive(recibido);

                miPrimerSocket.Close();

                right.IsEnabled = true;
                left.IsEnabled = true;
                forward.IsEnabled = false;
                back.IsEnabled = true;
                stop.IsEnabled = true;
            }
            catch (Exception error)
            {

            }
        }
        private void PageDownButtonClick(object sender, RoutedEventArgs e)
        {
            Socket miPrimerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint miDireccion = new IPEndPoint(IPAddress.Parse(ip), 80);
            try
            {



                byte[] datos = { 66, 65, 67, 75, 75, 13, 10, 13, 10 };

                byte[] recibido = new byte[500];
                miPrimerSocket.Connect(miDireccion); // Conectamos               

                miPrimerSocket.Send(datos);

                miPrimerSocket.Receive(recibido);

                miPrimerSocket.Close();

                right.IsEnabled = true;
                left.IsEnabled = true;
                forward.IsEnabled = true;
                back.IsEnabled = false;
                stop.IsEnabled = true;
            }
            catch (Exception error)
            {

            }
        }
        private void PageStopButtonClick(object sender, RoutedEventArgs e)
        {
            Socket miPrimerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint miDireccion = new IPEndPoint(IPAddress.Parse(ip), 80);

            try
            {

                byte[] datos = { 83, 84, 79, 80, 80, 13, 10, 13, 10 };

                byte[] recibido = new byte[500];
                miPrimerSocket.Connect(miDireccion); // Conectamos               

                miPrimerSocket.Send(datos);

                miPrimerSocket.Receive(recibido);

                miPrimerSocket.Close();
                right.IsEnabled = true;
                left.IsEnabled = true;
                forward.IsEnabled = true;
                back.IsEnabled = true;
                stop.IsEnabled = false;
            }
            catch (Exception error)
            {

            }
        }

    }
}
