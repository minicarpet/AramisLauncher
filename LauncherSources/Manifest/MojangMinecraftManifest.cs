namespace AramisLauncher.Manifest
{
    using System;
    using System.Collections.Generic;

    public partial class MojangMinecraftManifest
    {
        public Latest Latest { get; set; }
        public List<Version> Versions { get; set; }
    }
    public partial class Latest
    {
        public string Release { get; set; }
        public string Snapshot { get; set; }
    }
    public partial class Version
    {
        public string Id { get; set; }
        public TypeEnum Type { get; set; }
        public Uri Url { get; set; }
        public DateTimeOffset Time { get; set; }
        public DateTimeOffset ReleaseTime { get; set; }
    }
    public enum TypeEnum { old_alpha, old_beta, Release, Snapshot };
}
