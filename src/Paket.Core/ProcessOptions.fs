namespace Paket

[<RequireQualifiedAccess>]
type SemVerUpdateMode =
    | NoRestriction
    | KeepMajor
    | KeepMinor
    | KeepPatch

// Options for UpdateProcess and InstallProcess.
/// Force             - Force the download and reinstallation of all packages
/// Redirects         - Create binding redirects for the NuGet packages
/// OnlyReferenced    - Only install packages that are referenced in paket.references files.
/// TouchAffectedRefs - Touch projects referencing installed packages even if the project file does not change.
type InstallerOptions =
    { Force : bool
      SemVerUpdateMode : SemVerUpdateMode
      Redirects : bool
      CleanBindingRedirects : bool
      CreateNewBindingFiles : bool
      OnlyReferenced : bool
      TouchAffectedRefs : bool }

    static member Default =
        { Force = false
          Redirects = false
          SemVerUpdateMode = SemVerUpdateMode.NoRestriction
          CreateNewBindingFiles = false
          OnlyReferenced = false
          CleanBindingRedirects = false
          TouchAffectedRefs = false }

    static member CreateLegacyOptions(force, redirects, cleanBindingRedirects, createNewBindingFiles, semVerUpdateMode, touchAffectedRefs) =
        { InstallerOptions.Default with
            Force = force
            CreateNewBindingFiles = createNewBindingFiles
            CleanBindingRedirects = cleanBindingRedirects
            Redirects = redirects
            SemVerUpdateMode = semVerUpdateMode
            TouchAffectedRefs = touchAffectedRefs }

type UpdaterOptions =
    { Common : InstallerOptions
      NoInstall : bool }

    static member Default =
        { Common = InstallerOptions.Default
          NoInstall = false }
