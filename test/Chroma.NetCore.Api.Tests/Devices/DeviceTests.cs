using System;
using System.Net;
using Chroma.NetCore.Api.Chroma;
using Xunit;

namespace Chroma.NetCore.Api.Tests.Devices
{
    public class DeviceTests : IDisposable
    {

        private ChromaHttpClientTests tests;

        public DeviceTests()
        {
            tests = new ChromaHttpClientTests();
        }

        public void Dispose()
        {


            tests.Unregister_ReturnResultString0();
        }

        [Fact]
        public async void SetPosition_SetMousePadDifferentColors()
        {

            var httpClient = await tests.Register_ReturnRegisteredClient();
            
            httpClient.ClientMessage += HttpClientOnClientMessage;

            var container = new DeviceContainer();
            //For FireFly Chroma
            container.Mousepad.SetPosition(0, 0, Color.Orange);
            container.Mousepad.SetPosition(0, 6, Color.Green);
            container.Mousepad.SetPosition(0, 7, Color.Red);
            container.Mousepad.SetPosition(0, 8, Color.Yellow);

            var result =  container.Mousepad.SetDevice();
            
            Assert.True(result);
        }

        [Fact]
        public async void SetPosition_SetKeyPadDifferentColors()
        {

            var httpClient = await tests.Register_ReturnRegisteredClient();

            httpClient.ClientMessage += HttpClientOnClientMessage;

            var container = new DeviceContainer();

            container.Keypad.SetPosition(0, 0, Color.Orange);
            container.Keypad.SetPosition(1, 1, Color.Green);
            container.Keypad.SetPosition(2, 3, Color.Red);
            container.Keypad.SetPosition(3, 4, Color.Yellow);

            var result = container.Keypad.SetDevice();

            Assert.True(result);
        }

        [Fact]
        public async void SetPosition_SetMouseDifferentColors()
        {

            var httpClient = await tests.Register_ReturnRegisteredClient();
            httpClient.ClientMessage += HttpClientOnClientMessage;
            var container = new DeviceContainer();
            
            //For DeathAdder Chroma Wheel
            container.Mouse.SetPosition(2, 3, Color.Blue);
            //For DeathAdder Chroma Logo
            container.Mouse.SetPosition(7, 3, Color.Purple);

            var result = container.Mouse.SetDevice();

            Assert.True(result);
        }

        [Fact]
        public async void SetPosition_SetKeyboardDifferentColors()
        {
            var httpClient = await tests.Register_ReturnRegisteredClient();
            httpClient.ClientMessage += HttpClientOnClientMessage;
            var container = new DeviceContainer();

            container.Keyboard.SetPosition(1, 2, Color.Yellow);
            container.Keyboard.SetPosition(1, 3, Color.Blue);
            container.Keyboard.SetPosition(1, 4, Color.Green);
            container.Keyboard.SetPosition(2, 3, Color.Red);
            
            var result = container.Keyboard.SetDevice();

            Assert.True(result);
        }

        [Fact]
        public async void SetStatic_SetDifferentColors()
        {
            var httpClient = await tests.Register_ReturnRegisteredClient();
            httpClient.ClientMessage += HttpClientOnClientMessage;
            var container = new DeviceContainer();

            container.Keyboard.SetStatic(Color.Yellow);
            container.Mousepad.SetStatic(Color.Purple);
            container.Headset.SetStatic(Color.Blue);
            container.Mouse.SetStatic(Color.White);
        }


        private void HttpClientOnClientMessage(HttpStatusCode httpStatusCode, string device, string s)
        {
           Console.WriteLine($"{httpStatusCode}:{device}:{s}");
        }
    }
}
