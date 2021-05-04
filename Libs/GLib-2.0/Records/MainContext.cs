﻿using System;

namespace GLib
{
    public partial class MainContext
    {
        public Native.MainContext.Handle Handle => _handle;
        
        public static MainContext New()
            => new MainContext(Native.MainContext.Methods.New());

        public static MainContext Default()
            => new MainContext(Native.MainContext.Methods.Default());
    }
}
