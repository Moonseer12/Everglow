global using System.Collections.Generic;
global using Terraria;
global using Terraria.ID;
global using Terraria.ModLoader;

global using Item_id = System.Int32;
global using Buff_id = System.Int32;

using Everglow.Sources.Commons;

namespace Everglow
{
    public class Everglow : Mod
	{
        /// <summary>
        /// Get the instance of Everglow
        /// </summary>
        public static Everglow Instance
        {
            get { return m_instance; }
        }

        public ModuleManager ModuleManager
        {
            get
            {
                return m_moduleManager;
            }
        }

        private static Everglow m_instance;

        private ModuleManager m_moduleManager;


        public Everglow()
        {
        }

        public override void Load()
        {
            m_instance = this;

            m_moduleManager = new ModuleManager();
            m_moduleManager.LoadAll();
        }

        public override void Unload()
        {
            m_moduleManager.UnloadAll();

            m_instance = null;
        }
    }
}