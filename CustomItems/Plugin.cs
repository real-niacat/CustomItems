using HarmonyLib;
using InventorySystem.Items.Pickups;
using PluginAPI.Core.Attributes;
using PluginAPI.Events;
using RueI;
using System;

namespace CustomItems
{
    public class Plugin
    {

        internal static Plugin Instance { get; private set; }

        private static EventHandlers EventHandlers { get; set; }

        [PluginEntryPoint("custom item system", "1.0.0", "ghhumm custom gitems? does anyone read this?", "cat")]
        void LoadPluginAPI()
        {
            Instance = this;

            EventHandlers = new EventHandlers();
            EventManager.RegisterEvents(EventHandlers);
            EventManager.RegisterAllEvents(EventHandlers); //no clue if i gotta do this but its just 1 extra line so it doesnt matter that much lmao
            EventManager.RegisterEvents(this);
            RueIMain.EnsureInit();
            Harmony _harmony;
            _harmony = new Harmony("com.tpd.patches");
            _harmony.PatchAll();

            ItemPickupBase.OnPickupAdded += CustomItem.AddGlow;
            ItemPickupBase.OnPickupDestroyed += CustomItem.DeleteGlow;
            ServerConsole.AddLog("!! CUSTOM ITEM SYSTEM LOADED SUCCESSFULLY !!", ConsoleColor.Green);
        }
        [PluginUnload]
        void UnloadPluginAPI()
        {
            EventManager.UnregisterEvents(EventHandlers);
        }

    }
}
