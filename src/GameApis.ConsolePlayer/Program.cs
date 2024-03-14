// See https://aka.ms/new-console-template for more information

using GameApis.ConsolePlayer;
using GameApis.ConsolePlayer.Screens;

LocalState.Load();
Renderer.Render<PickUserScreen>();
await Renderer.Start();
