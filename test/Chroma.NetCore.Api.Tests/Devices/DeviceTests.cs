﻿using System;
using System.Net;
using Chroma.NetCore.Api.Chroma;
using Xunit;

namespace Chroma.NetCore.Api.Tests.Devices
{
    public class DeviceTests
    {
        private const string VALID_RESULT = "\"result\":0";

        [Fact]
        public async void SetPosition_SetMousePadDifferentColors()
        {
            var tests = new ChromaHttpClientTests();

            var httpClient = await tests.Register_ReturnRegisteredClient();
            
            httpClient.ClientMessage += HttpClientOnClientMessage;

            var container = new DeviceContainer(httpClient);
            //For FireFly Chroma
            Assert.True(container.Mousepad.SetPosition(0, 0, Color.Orange));
            Assert.True(container.Mousepad.SetPosition(0, 6, Color.Green));
            Assert.True(container.Mousepad.SetPosition(0, 7, Color.Red));
            Assert.True(container.Mousepad.SetPosition(0, 8, Color.Yellow));

            var result = await container.Mousepad.SetDevice();
            
            Assert.True(result.Contains(VALID_RESULT));
        }

        [Fact]
        public async void SetPosition_SetMouseDifferentColors()
        {
            var tests = new ChromaHttpClientTests();

            var httpClient = await tests.Register_ReturnRegisteredClient();
            httpClient.ClientMessage += HttpClientOnClientMessage;
            var container = new DeviceContainer(httpClient);
            
            //For DeathAdder Chroma Wheel
            Assert.True(container.Mouse.SetPosition(2, 3, Color.Blue));
            //For DeathAdder Chroma Logo
            Assert.True(container.Mouse.SetPosition(7, 3, Color.Purple));

            var result = await container.Mouse.SetDevice();

            Assert.True(result.Contains(VALID_RESULT));
        }

        [Fact]
        public async void SetPosition_SetKeyboardDifferentColors()
        {
            var tests = new ChromaHttpClientTests();

            var httpClient = await tests.Register_ReturnRegisteredClient();
            httpClient.ClientMessage += HttpClientOnClientMessage;
            var container = new DeviceContainer(httpClient);
            Assert.True(container.Keyboard.SetPosition(1, 2, Color.Yellow));
            Assert.True(container.Keyboard.SetPosition(1, 3, Color.Blue));
            Assert.True(container.Keyboard.SetPosition(1, 4, Color.Green));

            Assert.True(container.Keyboard.SetPosition(2, 3, Color.Red));
            
            var result = await container.Keyboard.SetDevice();

            Assert.True(result.Contains(VALID_RESULT));

        }

        [Fact]
        public async void SetStatic_SetDifferentColors()
        {
            var tests = new ChromaHttpClientTests();

            var httpClient = await tests.Register_ReturnRegisteredClient();
            httpClient.ClientMessage += HttpClientOnClientMessage;
            var container = new DeviceContainer(httpClient);

            await container.Keyboard.SetStatic(Color.Yellow);
            await container.Mousepad.SetStatic(Color.Purple);
            await container.Headset.SetStatic(Color.Blue);
            await container.Mouse.SetStatic(Color.White);

        }


        private void HttpClientOnClientMessage(HttpStatusCode httpStatusCode, string device, string s)
        {
           Console.WriteLine($"{httpStatusCode}:{device}:{s}");
        }
    }
}