﻿module Paket.IntegrationTests.UpdatePackageSpecs

open Fake
open System
open NUnit.Framework
open FsUnit
open System
open System.IO
open System.Diagnostics
open Paket
open Paket.Domain
open Paket.Requirements

[<Test>]
let ``#1018 update package in main group``() =
    paket "update nuget Newtonsoft.json" "i001018-legacy-groups-update" |> ignore
    let lockFile = LockFile.LoadFrom(Path.Combine(scenarioTempPath "i001018-legacy-groups-update","paket.lock"))
    lockFile.Groups.[Constants.MainDependencyGroup].Resolution.[PackageName "Newtonsoft.Json"].Version
    |> shouldBeGreaterThan (SemVer.Parse "6.0.3")
    lockFile.Groups.[Constants.MainDependencyGroup].Resolution.[PackageName "NUnit"].Version
    |> shouldEqual (SemVer.Parse "2.6.1")
    lockFile.Groups.[GroupName "Legacy"].Resolution.[PackageName "Newtonsoft.Json"].Version
    |> shouldEqual (SemVer.Parse "5.0.2")

[<Test>]
let ``#1018 update package in explicit main group``() =
    paket "update nuget Newtonsoft.json group Main" "i001018-legacy-groups-update" |> ignore
    let lockFile = LockFile.LoadFrom(Path.Combine(scenarioTempPath "i001018-legacy-groups-update","paket.lock"))
    lockFile.Groups.[Constants.MainDependencyGroup].Resolution.[PackageName "Newtonsoft.Json"].Version
    |> shouldBeGreaterThan (SemVer.Parse "6.0.3")
    lockFile.Groups.[Constants.MainDependencyGroup].Resolution.[PackageName "NUnit"].Version
    |> shouldEqual (SemVer.Parse "2.6.1")
    lockFile.Groups.[GroupName "Legacy"].Resolution.[PackageName "Newtonsoft.Json"].Version
    |> shouldEqual (SemVer.Parse "5.0.2")

[<Test>]
let ``#1018 update package in group``() =
    paket "update nuget Newtonsoft.json group leGacy" "i001018-legacy-groups-update" |> ignore
    let lockFile = LockFile.LoadFrom(Path.Combine(scenarioTempPath "i001018-legacy-groups-update","paket.lock"))
    lockFile.Groups.[Constants.MainDependencyGroup].Resolution.[PackageName "Newtonsoft.Json"].Version
    |> shouldEqual (SemVer.Parse "6.0.3")
    lockFile.Groups.[Constants.MainDependencyGroup].Resolution.[PackageName "NUnit"].Version
    |> shouldEqual (SemVer.Parse "2.6.1")
    lockFile.Groups.[GroupName "Legacy"].Resolution.[PackageName "Newtonsoft.Json"].Version
    |> shouldBeGreaterThan (SemVer.Parse "5.0.2")

[<Test>]
let ``#1178 update specific package``() =
    paket "update nuget NUnit" "i001178-update-with-regex" |> ignore
    let lockFile = LockFile.LoadFrom(Path.Combine(scenarioTempPath "i001178-update-with-regex","paket.lock"))
    lockFile.Groups.[Constants.MainDependencyGroup].Resolution.[PackageName "Castle.Windsor"].Version
    |> shouldEqual (SemVer.Parse "2.5.1")
    lockFile.Groups.[Constants.MainDependencyGroup].Resolution.[PackageName "NUnit"].Version
    |> shouldBeGreaterThan (SemVer.Parse "2.6.1")
    lockFile.Groups.[Constants.MainDependencyGroup].Resolution.[PackageName "Microsoft.AspNet.WebApi.SelfHost"].Version
    |> shouldEqual (SemVer.Parse "5.0.1")

[<Test>]
let ``#1469 update package in main group``() =
    update "i001469-darkseid" |> ignore
    let lockFile = LockFile.LoadFrom(Path.Combine(scenarioTempPath "i001469-darkseid","paket.lock"))
    lockFile.Groups.[Constants.MainDependencyGroup].Resolution.[PackageName "Darkseid"].Version
    |> shouldBeGreaterThan (SemVer.Parse "0.2.1")

[<Test>]
let ``#1178 update with Mircosoft.* filter``() =
    paket "update nuget Microsoft.* --filter" "i001178-update-with-regex" |> ignore
    let lockFile = LockFile.LoadFrom(Path.Combine(scenarioTempPath "i001178-update-with-regex","paket.lock"))
    lockFile.Groups.[Constants.MainDependencyGroup].Resolution.[PackageName "Castle.Windsor"].Version
    |> shouldEqual (SemVer.Parse "2.5.1")
    lockFile.Groups.[Constants.MainDependencyGroup].Resolution.[PackageName "NUnit"].Version
    |> shouldEqual (SemVer.Parse "2.6.1")
    lockFile.Groups.[Constants.MainDependencyGroup].Resolution.[PackageName "Microsoft.AspNet.WebApi.SelfHost"].Version
    |> shouldBeGreaterThan (SemVer.Parse "5.0.1")

[<Test>]
let ``#1178 update with [MN].* --filter``() =
    paket "update nuget [MN].* --filter" "i001178-update-with-regex" |> ignore
    let lockFile = LockFile.LoadFrom(Path.Combine(scenarioTempPath "i001178-update-with-regex","paket.lock"))
    lockFile.Groups.[Constants.MainDependencyGroup].Resolution.[PackageName "Castle.Windsor"].Version
    |> shouldEqual (SemVer.Parse "2.5.1")
    lockFile.Groups.[Constants.MainDependencyGroup].Resolution.[PackageName "NUnit"].Version
    |> shouldBeGreaterThan (SemVer.Parse "2.6.1")
    lockFile.Groups.[Constants.MainDependencyGroup].Resolution.[PackageName "Microsoft.AspNet.WebApi.SelfHost"].Version
    |> shouldBeGreaterThan (SemVer.Parse "5.0.1")

[<Test>]
let ``#1178 update with [MN].* and without filter should fail``() =
    try
        paket "update nuget [MN].*" "i001178-update-with-regex" |> ignore
        failwithf "Paket command expected to fail"
    with
    | exn when exn.Message.Contains "Package [MN].* was not found in paket.dependencies in group Main" -> ()

[<Test>]
let ``#1178 update with NUn.* filter to specific version``() =
    paket "update nuget NUn.* --filter version 2.6.2" "i001178-update-with-regex" |> ignore
    let lockFile = LockFile.LoadFrom(Path.Combine(scenarioTempPath "i001178-update-with-regex","paket.lock"))
    lockFile.Groups.[Constants.MainDependencyGroup].Resolution.[PackageName "Castle.Windsor"].Version
    |> shouldEqual (SemVer.Parse "2.5.1")
    lockFile.Groups.[Constants.MainDependencyGroup].Resolution.[PackageName "NUnit"].Version
    |> shouldEqual (SemVer.Parse "2.6.2")
    lockFile.Groups.[Constants.MainDependencyGroup].Resolution.[PackageName "Microsoft.AspNet.WebApi.SelfHost"].Version
    |> shouldEqual (SemVer.Parse "5.0.1")

[<Test>]
let ``#1413 doesn't take symbols``() =
    update "i001413-symbols" |> ignore
    let lockFile = LockFile.LoadFrom(Path.Combine(scenarioTempPath "i001413-symbols","paket.lock"))
    lockFile.Groups.[Constants.MainDependencyGroup].Resolution.[PackageName "Composable.Core"].Version
    |> shouldEqual (SemVer.Parse "3.4.0")

[<Test>]
let ``#1432 update doesn't throw Stackoverflow``() =
    let scenario = "i001432-stackoverflow"

    prepare scenario
    directPaket "pack templatefile paket.A.template version 1.0.0-prerelease output bin" scenario |> ignore
    directPaket "pack templatefile paket.A.template version 1.0.0 output bin" scenario |> ignore
    directPaket "pack templatefile paket.A.template version 1.1.0-prerelease output bin" scenario |> ignore
    directPaket "pack templatefile paket.B.template version 1.0.0 output bin" scenario |> ignore
    directPaket "pack templatefile paket.C.template version 1.0.0-prerelease output bin" scenario |> ignore
    directPaket "pack templatefile paket.D.template version 1.0.0-prerelease output bin" scenario  |> ignore
    directPaket "update" scenario|> ignore

[<Test>]
let ``#1579 update allows unpinned``() =
    let scenario = "i001579-unlisted"

    prepare scenario
    directPaket "pack templatefile paket.A.template version 1.0.0-prerelease output bin" scenario |> ignore
    directPaket "update" scenario|> ignore

[<Test>]
let ``#1500 don't detect framework twice``() =
    update "i001500-auto-detect" |> ignore
    let lockFile = LockFile.LoadFrom(Path.Combine(scenarioTempPath "i001500-auto-detect","paket.lock"))
    lockFile.Groups.[Constants.MainDependencyGroup].Options.Settings.FrameworkRestrictions
    |> shouldEqual (FrameworkRestrictionList [FrameworkRestriction.Exactly(FrameworkIdentifier.DotNetFramework(FrameworkVersion.V4_5_2))])


[<Test>]
let ``#1501 download succeeds``() =
    update "i001510-download" |> ignore

[<Test>]
let ``#1520 update with pinned dependency succeeds``() =
    update "i001520-pinned-error" |> ignore

[<Test>]
let ``#1534 resolves Selenium.Support``() =
    update "i001534-selenium" |> ignore


[<Test>]
let ``#1635 should tell about auth issue``() =
    try
        update "i001635-wrong-pw" |> ignore
        failwith "error expected"
    with
    | exn when exn.Message.Contains("Could not find versions for package Argu") -> ()