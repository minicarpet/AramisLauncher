﻿namespace AramisLauncher.Manifest
{
    using System;
    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class PackageJson
    {
        [JsonProperty("baseModLoader")]
        public BaseModLoader BaseModLoader { get; set; }

        [JsonProperty("isUnlocked")]
        public bool IsUnlocked { get; set; }

        [JsonProperty("javaArgsOverride")]
        public object JavaArgsOverride { get; set; }

        [JsonProperty("javaDirOverride")]
        public object JavaDirOverride { get; set; }

        [JsonProperty("lastPlayed")]
        public DateTimeOffset LastPlayed { get; set; }

        [JsonProperty("manifest")]
        public Manifest Manifest { get; set; }

        [JsonProperty("fileDate")]
        public DateTimeOffset FileDate { get; set; }

        [JsonProperty("installedModpack")]
        public object InstalledModpack { get; set; }

        [JsonProperty("projectID")]
        public long ProjectId { get; set; }

        [JsonProperty("fileID")]
        public long FileId { get; set; }

        [JsonProperty("customAuthor")]
        public string CustomAuthor { get; set; }

        [JsonProperty("modpackOverrides")]
        public string[] ModpackOverrides { get; set; }

        [JsonProperty("isMemoryOverride")]
        public bool IsMemoryOverride { get; set; }

        [JsonProperty("allocatedMemory")]
        public long AllocatedMemory { get; set; }

        [JsonProperty("guid")]
        public Guid Guid { get; set; }

        [JsonProperty("gameTypeID")]
        public long GameTypeId { get; set; }

        [JsonProperty("installPath")]
        public string InstallPath { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("cachedScans")]
        public CachedScan[] CachedScans { get; set; }

        [JsonProperty("isValid")]
        public bool IsValid { get; set; }

        [JsonProperty("lastPreviousMatchUpdate")]
        public DateTimeOffset LastPreviousMatchUpdate { get; set; }

        [JsonProperty("isEnabled")]
        public bool IsEnabled { get; set; }

        [JsonProperty("isPinned")]
        public bool IsPinned { get; set; }

        [JsonProperty("gameVersion")]
        public string GameVersion { get; set; }

        [JsonProperty("preferenceAlternateFile")]
        public bool PreferenceAlternateFile { get; set; }

        [JsonProperty("preferenceAutoInstallUpdates")]
        public bool PreferenceAutoInstallUpdates { get; set; }

        [JsonProperty("preferenceQuickDeleteLibraries")]
        public bool PreferenceQuickDeleteLibraries { get; set; }

        [JsonProperty("preferenceDeleteSavedVariables")]
        public bool PreferenceDeleteSavedVariables { get; set; }

        [JsonProperty("preferenceProcessFileCommands")]
        public bool PreferenceProcessFileCommands { get; set; }

        [JsonProperty("preferenceReleaseType")]
        public long PreferenceReleaseType { get; set; }

        [JsonProperty("syncProfile")]
        public SyncProfile SyncProfile { get; set; }

        [JsonProperty("preferenceShowAddOnInfo")]
        public bool PreferenceShowAddOnInfo { get; set; }

        [JsonProperty("installDate")]
        public DateTimeOffset InstallDate { get; set; }

        [JsonProperty("installedAddons")]
        public InstalledAddon[] InstalledAddons { get; set; }

        [JsonProperty("isMigrated")]
        public bool IsMigrated { get; set; }

        [JsonProperty("preferenceUploadProfile")]
        public bool PreferenceUploadProfile { get; set; }
    }

    public partial class BaseModLoader
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("gameVersionId")]
        public long GameVersionId { get; set; }

        [JsonProperty("minecraftGameVersionId")]
        public long MinecraftGameVersionId { get; set; }

        [JsonProperty("forgeVersion")]
        public string ForgeVersion { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public long Type { get; set; }

        [JsonProperty("downloadUrl")]
        public Uri DownloadUrl { get; set; }

        [JsonProperty("filename")]
        public string Filename { get; set; }

        [JsonProperty("installMethod")]
        public long InstallMethod { get; set; }

        [JsonProperty("latest")]
        public bool Latest { get; set; }

        [JsonProperty("recommended")]
        public bool Recommended { get; set; }

        [JsonProperty("approved")]
        public bool Approved { get; set; }

        [JsonProperty("dateModified")]
        public DateTimeOffset DateModified { get; set; }

        [JsonProperty("mavenVersionString")]
        public string MavenVersionString { get; set; }

        [JsonProperty("versionJson")]
        public string VersionJson { get; set; }

        [JsonProperty("librariesInstallLocation")]
        public string LibrariesInstallLocation { get; set; }

        [JsonProperty("minecraftVersion")]
        public string MinecraftVersion { get; set; }

        [JsonProperty("modLoaderGameVersionId")]
        public long ModLoaderGameVersionId { get; set; }

        [JsonProperty("modLoaderGameVersionTypeId")]
        public long ModLoaderGameVersionTypeId { get; set; }

        [JsonProperty("modLoaderGameVersionStatus")]
        public long ModLoaderGameVersionStatus { get; set; }

        [JsonProperty("modLoaderGameVersionTypeStatus")]
        public long ModLoaderGameVersionTypeStatus { get; set; }

        [JsonProperty("mcGameVersionId")]
        public long McGameVersionId { get; set; }

        [JsonProperty("mcGameVersionTypeId")]
        public long McGameVersionTypeId { get; set; }

        [JsonProperty("mcGameVersionStatus")]
        public long McGameVersionStatus { get; set; }

        [JsonProperty("mcGameVersionTypeStatus")]
        public long McGameVersionTypeStatus { get; set; }

        [JsonProperty("installProfileJson")]
        public string InstallProfileJson { get; set; }
    }

    public partial class CachedScan
    {
        [JsonProperty("folderName")]
        public string FolderName { get; set; }

        [JsonProperty("fingerprint")]
        public long Fingerprint { get; set; }

        [JsonProperty("fileDateHash")]
        public long FileDateHash { get; set; }

        [JsonProperty("sectionID")]
        public long SectionId { get; set; }

        [JsonProperty("individualFingerprints")]
        public long[] IndividualFingerprints { get; set; }

        [JsonProperty("status")]
        public long Status { get; set; }

        [JsonProperty("timestamp")]
        public DateTimeOffset Timestamp { get; set; }

        [JsonProperty("lastWriteTimeUtc")]
        public DateTimeOffset LastWriteTimeUtc { get; set; }

        [JsonProperty("queryTimestamp")]
        public DateTimeOffset QueryTimestamp { get; set; }

        [JsonProperty("fileCount")]
        public long FileCount { get; set; }

        [JsonProperty("fileSize")]
        public long FileSize { get; set; }
    }

    public partial class InstalledAddon
    {
        [JsonProperty("addonID")]
        public long AddonId { get; set; }

        [JsonProperty("gameInstanceID")]
        public Guid GameInstanceId { get; set; }

        [JsonProperty("installedFile")]
        public InstalledFile InstalledFile { get; set; }

        [JsonProperty("dateInstalled")]
        public DateTimeOffset DateInstalled { get; set; }

        [JsonProperty("dateUpdated")]
        public DateTimeOffset DateUpdated { get; set; }

        [JsonProperty("dateLastUpdateAttempted")]
        public DateTimeOffset DateLastUpdateAttempted { get; set; }

        [JsonProperty("status")]
        public long Status { get; set; }

        [JsonProperty("preferenceAutoInstallUpdates")]
        public bool PreferenceAutoInstallUpdates { get; set; }

        [JsonProperty("preferenceAlternateFile")]
        public bool PreferenceAlternateFile { get; set; }

        [JsonProperty("preferenceIsIgnored")]
        public bool PreferenceIsIgnored { get; set; }

        [JsonProperty("isModified")]
        public bool IsModified { get; set; }

        [JsonProperty("isWorkingCopy")]
        public bool IsWorkingCopy { get; set; }

        [JsonProperty("isFuzzyMatch")]
        public bool IsFuzzyMatch { get; set; }

        [JsonProperty("preferenceReleaseType")]
        public object PreferenceReleaseType { get; set; }

        [JsonProperty("manifestName")]
        public object ManifestName { get; set; }

        [JsonProperty("installedTargets")]
        public object InstalledTargets { get; set; }
    }

    public partial class InstalledFile
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("fileName")]
        public string FileName { get; set; }

        [JsonProperty("fileDate")]
        public DateTimeOffset FileDate { get; set; }

        [JsonProperty("fileLength")]
        public long FileLength { get; set; }

        [JsonProperty("releaseType")]
        public long ReleaseType { get; set; }

        [JsonProperty("fileStatus")]
        public long FileStatus { get; set; }

        [JsonProperty("downloadUrl")]
        public Uri DownloadUrl { get; set; }

        [JsonProperty("isAlternate")]
        public bool IsAlternate { get; set; }

        [JsonProperty("alternateFileId")]
        public long AlternateFileId { get; set; }

        [JsonProperty("dependencies")]
        public Dependency[] Dependencies { get; set; }

        [JsonProperty("isAvailable")]
        public bool IsAvailable { get; set; }

        [JsonProperty("modules")]
        public Module[] Modules { get; set; }

        [JsonProperty("packageFingerprint")]
        public long PackageFingerprint { get; set; }

        [JsonProperty("gameVersion")]
        public string[] GameVersion { get; set; }

        [JsonProperty("hasInstallScript")]
        public bool HasInstallScript { get; set; }

        [JsonProperty("isCompatibleWithClient")]
        public bool IsCompatibleWithClient { get; set; }

        [JsonProperty("categorySectionPackageType")]
        public long CategorySectionPackageType { get; set; }

        [JsonProperty("restrictProjectFileAccess")]
        public long RestrictProjectFileAccess { get; set; }

        [JsonProperty("projectStatus")]
        public long ProjectStatus { get; set; }

        [JsonProperty("projectId")]
        public long ProjectId { get; set; }

        [JsonProperty("gameVersionDateReleased")]
        public DateTimeOffset GameVersionDateReleased { get; set; }

        [JsonProperty("gameId")]
        public long GameId { get; set; }

        [JsonProperty("isServerPack")]
        public bool IsServerPack { get; set; }

        [JsonProperty("FileNameOnDisk")]
        public string FileNameOnDisk { get; set; }

        [JsonProperty("sortableGameVersion", NullValueHandling = NullValueHandling.Ignore)]
        public SortableGameVersion[] SortableGameVersion { get; set; }

        [JsonProperty("renderCacheId", NullValueHandling = NullValueHandling.Ignore)]
        public long? RenderCacheId { get; set; }

        [JsonProperty("packageFingerprintId", NullValueHandling = NullValueHandling.Ignore)]
        public long? PackageFingerprintId { get; set; }

        [JsonProperty("gameVersionMappingId", NullValueHandling = NullValueHandling.Ignore)]
        public long? GameVersionMappingId { get; set; }

        [JsonProperty("gameVersionId", NullValueHandling = NullValueHandling.Ignore)]
        public long? GameVersionId { get; set; }
    }

    public partial class Dependency
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("addonId")]
        public long AddonId { get; set; }

        [JsonProperty("type")]
        public long Type { get; set; }

        [JsonProperty("fileId")]
        public long FileId { get; set; }
    }

    public partial class Module
    {
        [JsonProperty("foldername")]
        public string Foldername { get; set; }

        [JsonProperty("fingerprint")]
        public long Fingerprint { get; set; }

        [JsonProperty("type")]
        public long Type { get; set; }
    }

    public partial class SortableGameVersion
    {
        [JsonProperty("gameVersionPadded")]
        public string GameVersionPadded { get; set; }

        [JsonProperty("gameVersion")]
        public string GameVersion { get; set; }

        [JsonProperty("gameVersionReleaseDate")]
        public DateTimeOffset GameVersionReleaseDate { get; set; }

        [JsonProperty("gameVersionName")]
        public string GameVersionName { get; set; }
    }

    public partial class Manifest
    {
        [JsonProperty("minecraft")]
        public Minecraft Minecraft { get; set; }

        [JsonProperty("manifestType")]
        public string ManifestType { get; set; }

        [JsonProperty("manifestVersion")]
        public long ManifestVersion { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("description")]
        public object Description { get; set; }

        [JsonProperty("projectID")]
        public object ProjectId { get; set; }

        [JsonProperty("files")]
        public ManifestFile[] Files { get; set; }

        [JsonProperty("overrides")]
        public string Overrides { get; set; }
    }

    public partial class ManifestFile
    {
        [JsonProperty("projectID")]
        public long ProjectId { get; set; }

        [JsonProperty("fileID")]
        public long FileId { get; set; }

        [JsonProperty("required")]
        public bool FileRequired { get; set; }
    }

    public partial class Minecraft
    {
        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("additionalJavaArgs")]
        public object AdditionalJavaArgs { get; set; }

        [JsonProperty("modLoaders")]
        public ModLoader[] ModLoaders { get; set; }

        [JsonProperty("libraries")]
        public object Libraries { get; set; }
    }

    public partial class ModLoader
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("primary")]
        public bool Primary { get; set; }
    }

    public partial class SyncProfile
    {
        [JsonProperty("PreferenceEnabled")]
        public bool PreferenceEnabled { get; set; }

        [JsonProperty("PreferenceAutoSync")]
        public bool PreferenceAutoSync { get; set; }

        [JsonProperty("PreferenceAutoDelete")]
        public bool PreferenceAutoDelete { get; set; }

        [JsonProperty("PreferenceBackupSavedVariables")]
        public bool PreferenceBackupSavedVariables { get; set; }

        [JsonProperty("GameInstanceGuid")]
        public Guid GameInstanceGuid { get; set; }

        [JsonProperty("SyncProfileID")]
        public long SyncProfileId { get; set; }

        [JsonProperty("SavedVariablesProfile")]
        public object SavedVariablesProfile { get; set; }

        [JsonProperty("LastSyncDate")]
        public DateTimeOffset LastSyncDate { get; set; }
    }

    public partial class PackageJson
    {
        public static PackageJson FromJson(string json) => JsonConvert.DeserializeObject<PackageJson>(json, PackageJsonConverter.Settings);
    }

    public static class PackageJsonSerialize
    {
        public static string ToJson(this PackageJson self) => JsonConvert.SerializeObject(self, PackageJsonConverter.Settings);
    }

    internal static class PackageJsonConverter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
