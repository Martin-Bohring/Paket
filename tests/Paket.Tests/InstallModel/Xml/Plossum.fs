﻿module Paket.InstallModel.Xml.PlossumSpecs

open Paket
open NUnit.Framework
open FsUnit
open Paket.TestHelpers
open Paket.Domain
open Paket.Requirements

let expected = """
<Choose xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <When Condition="($(TargetFrameworkIdentifier) == '.NETStandard' And ($(TargetFrameworkVersion) == 'v1.0' Or $(TargetFrameworkVersion) == 'v1.1' Or $(TargetFrameworkVersion) == 'v1.2' Or $(TargetFrameworkVersion) == 'v1.3' Or $(TargetFrameworkVersion) == 'v1.4' Or $(TargetFrameworkVersion) == 'v1.5')) Or ($(TargetFrameworkIdentifier) == '.NETFramework' And ($(TargetFrameworkVersion) == 'v4.0' Or $(TargetFrameworkVersion) == 'v4.5' Or $(TargetFrameworkVersion) == 'v4.5.1' Or $(TargetFrameworkVersion) == 'v4.5.2' Or $(TargetFrameworkVersion) == 'v4.5.3' Or $(TargetFrameworkVersion) == 'v4.6' Or $(TargetFrameworkVersion) == 'v4.6.1' Or $(TargetFrameworkVersion) == 'v4.6.2'))">
    <ItemGroup>
      <Reference Include="Plossum CommandLine">
        <HintPath>..\..\..\Plossum.CommandLine\lib\net40\Plossum CommandLine.dll</HintPath>
        <Private>True</Private>
        <Paket>True</Paket>
      </Reference>
    </ItemGroup>
  </When>
</Choose>"""

[<Test>]
let ``should generate Xml for Plossum``() = 
    let model =
        InstallModel.CreateFromLibs(PackageName "Plossum.CommandLine", SemVer.Parse "1.5.0", [],
            [ @"..\Plossum.CommandLine\lib\net40\Plossum CommandLine.dll" ],
              [],
              [],
              Nuspec.All)
    
    let _,targetsNodes,chooseNode,_,_ = ProjectFile.TryLoad("./ProjectFile/TestData/Empty.fsprojtest").Value.GenerateXml(model,Map.empty,true,true,None)
    chooseNode.OuterXml
    |> normalizeXml
    |> shouldEqual (normalizeXml expected)