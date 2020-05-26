using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AramisLauncher.JSON
{
    public partial class MinecraftVersionJson
    {
        [JsonProperty("arguments")]
        public Arguments Arguments { get; set; }

        [JsonProperty("assetIndex")]
        public AssetIndex AssetIndex { get; set; }

        [JsonProperty("assets")]
        public string Assets { get; set; }

        [JsonProperty("downloads")]
        public MinecraftVersionJsonDownloads Downloads { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("libraries")]
        public MinecraftLibrary[] Libraries { get; set; }

        [JsonProperty("logging")]
        public Logging Logging { get; set; }

        [JsonProperty("mainClass")]
        public string MainClass { get; set; }

        [JsonProperty("minimumLauncherVersion")]
        public long MinimumLauncherVersion { get; set; }

        [JsonProperty("releaseTime")]
        public DateTimeOffset ReleaseTime { get; set; }

        [JsonProperty("time")]
        public DateTimeOffset Time { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("minecraftArguments")]
        public string minecraftArguments { get; set; }
    }

    public partial class Arguments
    {
        [JsonProperty("game")]
        public GameElement[] Game { get; set; }

        [JsonProperty("jvm")]
        public JvmElement[] Jvm { get; set; }
    }

    public partial class GameClass
    {
        [JsonProperty("rules")]
        public GameRule[] Rules { get; set; }

        [JsonProperty("value")]
        public Value Value { get; set; }
    }

    public partial class GameRule
    {
        [JsonProperty("action")]
        public Action Action { get; set; }

        [JsonProperty("features")]
        public Features Features { get; set; }
    }

    public partial class Features
    {
        [JsonProperty("is_demo_user", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsDemoUser { get; set; }

        [JsonProperty("has_custom_resolution", NullValueHandling = NullValueHandling.Ignore)]
        public bool? HasCustomResolution { get; set; }
    }

    public partial class JvmClass
    {
        [JsonProperty("rules")]
        public JvmRule[] Rules { get; set; }

        [JsonProperty("value")]
        public Value Value { get; set; }
    }

    public partial class JvmRule
    {
        [JsonProperty("action")]
        public Action Action { get; set; }

        [JsonProperty("os")]
        public PurpleOs Os { get; set; }
    }

    public partial class PurpleOs
    {
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("version", NullValueHandling = NullValueHandling.Ignore)]
        public string Version { get; set; }

        [JsonProperty("arch", NullValueHandling = NullValueHandling.Ignore)]
        public string Arch { get; set; }
    }

    public partial class AssetIndex
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("sha1")]
        public string Sha1 { get; set; }

        [JsonProperty("size")]
        public long Size { get; set; }

        [JsonProperty("totalSize")]
        public long TotalSize { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }
    }

    public partial class MinecraftVersionJsonDownloads
    {
        [JsonProperty("client")]
        public ServerClass Client { get; set; }

        [JsonProperty("client_mappings")]
        public Mappings ClientMappings { get; set; }

        [JsonProperty("server")]
        public ServerClass Server { get; set; }

        [JsonProperty("server_mappings")]
        public Mappings ServerMappings { get; set; }
    }

    public partial class ServerClass
    {
        [JsonProperty("sha1")]
        public string Sha1 { get; set; }

        [JsonProperty("size")]
        public long Size { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }
    }

    public partial class Mappings
    {
        [JsonProperty("sha1")]
        public string Sha1 { get; set; }

        [JsonProperty("size")]
        public long Size { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }
    }

    public partial class MinecraftLibrary
    {
        [JsonProperty("downloads")]
        public LibraryDownloads Downloads { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("rules", NullValueHandling = NullValueHandling.Ignore)]
        public LibraryRule[] Rules { get; set; }

        [JsonProperty("natives", NullValueHandling = NullValueHandling.Ignore)]
        public Natives Natives { get; set; }

        [JsonProperty("extract", NullValueHandling = NullValueHandling.Ignore)]
        public Extract Extract { get; set; }
    }

    public partial class LibraryDownloads
    {
        [JsonProperty("artifact")]
        public Artifact Artifact { get; set; }

        [JsonProperty("classifiers", NullValueHandling = NullValueHandling.Ignore)]
        public Classifiers Classifiers { get; set; }
    }

    public partial class Artifact
    {
        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("sha1")]
        public string Sha1 { get; set; }

        [JsonProperty("size")]
        public long Size { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }
    }

    public partial class Classifiers
    {
        [JsonProperty("javadoc", NullValueHandling = NullValueHandling.Ignore)]
        public Artifact Javadoc { get; set; }

        [JsonProperty("natives-linux", NullValueHandling = NullValueHandling.Ignore)]
        public Artifact NativesLinux { get; set; }

        [JsonProperty("natives-macos", NullValueHandling = NullValueHandling.Ignore)]
        public Artifact NativesMacos { get; set; }

        [JsonProperty("natives-windows", NullValueHandling = NullValueHandling.Ignore)]
        public Artifact NativesWindows { get; set; }

        [JsonProperty("sources", NullValueHandling = NullValueHandling.Ignore)]
        public Artifact Sources { get; set; }

        [JsonProperty("natives-osx", NullValueHandling = NullValueHandling.Ignore)]
        public Artifact NativesOsx { get; set; }
    }

    public partial class Extract
    {
        [JsonProperty("exclude")]
        public string[] Exclude { get; set; }
    }

    public partial class Natives
    {
        [JsonProperty("osx", NullValueHandling = NullValueHandling.Ignore)]
        public string Osx { get; set; }

        [JsonProperty("linux", NullValueHandling = NullValueHandling.Ignore)]
        public string Linux { get; set; }

        [JsonProperty("windows", NullValueHandling = NullValueHandling.Ignore)]
        public string Windows { get; set; }
    }

    public partial class LibraryRule
    {
        [JsonProperty("action")]
        public Action Action { get; set; }

        [JsonProperty("os", NullValueHandling = NullValueHandling.Ignore)]
        public FluffyOs Os { get; set; }
    }

    public partial class FluffyOs
    {
        [JsonProperty("name")]
        public Name Name { get; set; }
    }

    public partial class Logging
    {
        [JsonProperty("client")]
        public LoggingClient Client { get; set; }
    }

    public partial class LoggingClient
    {
        [JsonProperty("argument")]
        public string Argument { get; set; }

        [JsonProperty("file")]
        public ClientFile File { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public partial class ClientFile
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("sha1")]
        public string Sha1 { get; set; }

        [JsonProperty("size")]
        public long Size { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }
    }

    public enum Action { Allow, Disallow };

    public enum Name { Osx };

    public partial struct Value
    {
        public string String;
        public string[] StringArray;

        public static implicit operator Value(string String) => new Value { String = String };
        public static implicit operator Value(string[] StringArray) => new Value { StringArray = StringArray };
    }

    public partial struct GameElement
    {
        public GameClass GameClass;
        public string String;

        public static implicit operator GameElement(GameClass GameClass) => new GameElement { GameClass = GameClass };
        public static implicit operator GameElement(string String) => new GameElement { String = String };
    }

    public partial struct JvmElement
    {
        public JvmClass JvmClass;
        public string String;

        public static implicit operator JvmElement(JvmClass JvmClass) => new JvmElement { JvmClass = JvmClass };
        public static implicit operator JvmElement(string String) => new JvmElement { String = String };
    }

    public partial class MinecraftVersionJson
    {
        public static MinecraftVersionJson FromJson(string json) => JsonConvert.DeserializeObject<MinecraftVersionJson>(json, AramisLauncher.JSON.MinecraftVersionJsonConverter.Settings);
    }

    public static class MinecraftVersionJsonSerialize
    {
        public static string ToJson(this MinecraftVersionJson self) => JsonConvert.SerializeObject(self, AramisLauncher.JSON.MinecraftVersionJsonConverter.Settings);
    }

    internal static class MinecraftVersionJsonConverter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                MinecraftVersionJsonGameElementConverter.Singleton,
                ActionConverter.Singleton,
                ValueConverter.Singleton,
                JvmElementConverter.Singleton,
                NameConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class MinecraftVersionJsonGameElementConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(GameElement) || t == typeof(GameElement?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.String:
                case JsonToken.Date:
                    var stringValue = serializer.Deserialize<string>(reader);
                    return new GameElement { String = stringValue };
                case JsonToken.StartObject:
                    var objectValue = serializer.Deserialize<GameClass>(reader);
                    return new GameElement { GameClass = objectValue };
            }
            throw new Exception("Cannot unmarshal type GameElement");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (GameElement)untypedValue;
            if (value.String != null)
            {
                serializer.Serialize(writer, value.String);
                return;
            }
            if (value.GameClass != null)
            {
                serializer.Serialize(writer, value.GameClass);
                return;
            }
            throw new Exception("Cannot marshal type GameElement");
        }

        public static readonly MinecraftVersionJsonGameElementConverter Singleton = new MinecraftVersionJsonGameElementConverter();
    }

    internal class ActionConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Action) || t == typeof(Action?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "allow":
                    return Action.Allow;
                case "disallow":
                    return Action.Disallow;
            }
            throw new Exception("Cannot unmarshal type Action");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Action)untypedValue;
            switch (value)
            {
                case Action.Allow:
                    serializer.Serialize(writer, "allow");
                    return;
                case Action.Disallow:
                    serializer.Serialize(writer, "disallow");
                    return;
            }
            throw new Exception("Cannot marshal type Action");
        }

        public static readonly ActionConverter Singleton = new ActionConverter();
    }

    internal class ValueConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Value) || t == typeof(Value?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.String:
                case JsonToken.Date:
                    var stringValue = serializer.Deserialize<string>(reader);
                    return new Value { String = stringValue };
                case JsonToken.StartArray:
                    var arrayValue = serializer.Deserialize<string[]>(reader);
                    return new Value { StringArray = arrayValue };
            }
            throw new Exception("Cannot unmarshal type Value");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (Value)untypedValue;
            if (value.String != null)
            {
                serializer.Serialize(writer, value.String);
                return;
            }
            if (value.StringArray != null)
            {
                serializer.Serialize(writer, value.StringArray);
                return;
            }
            throw new Exception("Cannot marshal type Value");
        }

        public static readonly ValueConverter Singleton = new ValueConverter();
    }

    internal class JvmElementConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(JvmElement) || t == typeof(JvmElement?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.String:
                case JsonToken.Date:
                    var stringValue = serializer.Deserialize<string>(reader);
                    return new JvmElement { String = stringValue };
                case JsonToken.StartObject:
                    var objectValue = serializer.Deserialize<JvmClass>(reader);
                    return new JvmElement { JvmClass = objectValue };
            }
            throw new Exception("Cannot unmarshal type JvmElement");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (JvmElement)untypedValue;
            if (value.String != null)
            {
                serializer.Serialize(writer, value.String);
                return;
            }
            if (value.JvmClass != null)
            {
                serializer.Serialize(writer, value.JvmClass);
                return;
            }
            throw new Exception("Cannot marshal type JvmElement");
        }

        public static readonly JvmElementConverter Singleton = new JvmElementConverter();
    }

    internal class NameConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Name) || t == typeof(Name?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "osx")
            {
                return Name.Osx;
            }
            throw new Exception("Cannot unmarshal type Name");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Name)untypedValue;
            if (value == Name.Osx)
            {
                serializer.Serialize(writer, "osx");
                return;
            }
            throw new Exception("Cannot marshal type Name");
        }

        public static readonly NameConverter Singleton = new NameConverter();
    }
}
