# Getting Started - Contribute

## Overview

* [Downloading The Repository](#downloading-the-repository)
* [Rules and Etiquette: General](#rules-and-etiquette-general)
* [Rules and Etiquette: MDK Mockups](#rules-and-etiquette-mdk-mockups)
* [Rules and Etiquette: MDK UI](#rules-and-etiquette-mdk-ui)
  * [None Version-Controlled Configuration](#none-version-controlled-configuration)



## Downloading The Repository

The process of downloading the repository for contribution is only a little different from [Getting Started - Use](https://github.com/malware-dev/MDK-Mockups/blob/master/Docs/Getting-Started-Use.md). Rather than checking out the repository normally, you will need to follow [GitHub's instructions on how to Fork a repository](https://help.github.com/articles/fork-a-repo/). Other than that the process is identical.



Once you've made a change you wish to share, you will need to [create a pull request](https://help.github.com/articles/creating-a-pull-request-from-a-fork/). This will become a merge task that one of the team administrators can deal with and merge. Once this is done it is available for anyone who updates their repository.



## Rules and Etiquette: General
* It's considered courteous to follow the coding standards. There is a risk that your pull request might be rejected if it deviates too far from the standards.
* Everything is open for contribution, including this documentation. This is the reason it's stored directly in the repository rather than the wiki. The better documentation, the easier for people to get started, the more people we get contributing.
* Make sure your contributions do not break something. If it does, and you think your change is important, make contact with one of the project administrators - either via the Issues or via Keen's Discord, and argue your case.
* Make sure your contributions are flexible. Others may want to expand on it.
* Comment your contributions (documentation comments is fair enough) and make sure your code is readable.
* **This project relies on cooperation.** It will fall apart if we do not, so we will not tolerate bad behavior.



## Rules and Etiquette: MDK-Mockups

* In-game scripts are locked to C# 6.0. Do not use features from newer versions, even if your IDE is recommending you do so.
* _All_ `.cs` files **must** end with the suffix `.debug.cs`, not just `.cs`. This is so MDK can exclude these files when deploying a script.
* All mockup classes should be _public_ and _partial_.
* All mockup classes should be decorated with the following:
  * This will allow debuggers to interact with them the same as they would native game implementations.
```cs
#if !MOCKUP_DEBUG
    [System.Diagnostics.DebuggerNonUserCode]
#endif
```
* Mockup properties and methods should _virtual_ whenever possible.
* Do not make your mockups work via nonstandard behavior. It should replicate game behavior or not at all.
* While there's **no requirement** to completely finish every single feature of a block, please make sure the parts you _do not_ include throws `NotImplementedException`. Obviously, the more you complete before creating your pull request, the better.

* Example mock implementation. [MockAirVent.cs](Example-Mock-Class.md)


## Rules and Etiquette: MDK-UI

* MDK-UI specific code must be part of the MDK-UI project, and not implemented within MDK-Mockups
  * The MDK-Mockups Shared Project must remain usable by MDK-SE projects without the use of the MDK-UI project.
* Mockups should be implemented by creating a new _partial_ component and implementing the new functionality.
  * Mockup classes which support realtime runtime updates (such as doors and lights) must implement the `IMockupRuntimeProvider` interface.
* If a mockup's existing implementation must be changed to facilty UI interaction (for example by replacing an already mocked method), the mockup class should be marked with the `[MockOverridden]` attribute and a sub-class created.
* To implement editable property support to a Mocked block, the block type must be decorated with the `DisplayNameAttribute`.
  * Editable properties must also be decorated with the `DisplayNameAttribute`.
  * Properties which are locked between two values must be decorated with the `RangeAttribute`.
  * Properties which are not editable must be decorated with the `ReadOnlyAttribute`.
  * If an attribute needs to be added to an existing property (IE there is no sub-class defined for UI interaction).
    * Decorate the class with a `MetadataTypeAttribute` defining the custom metadata class.
	* Create the new internal metadata class with the naming format `<BaseClass>Metadata`.
	* Create metadata properties with the format `public object <BasePropertyName> { get; set; }.
	* Decorate the metadata properties as you would a normal property.

* Example metadata implementation. [MockAirVentMetadata.cs](Example-Metadata-Class.md)


### None Version-Controlled Configuration

The MDK-UI project has a none-version controlled file `SpaceEngineers.paths.prop` which must be manually included for the project to build.

Here is the default template for a standard game install, be sure to modify the directory if you have the game installed elsewhere.

```
<?xml version="1.0" encoding="utf-8" standalone="yes"?>
<Project xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <PropertyGroup>
    <SpaceEngineersGameBin>c:\program files (x86)\steam\SteamApps\common\SpaceEngineers\Bin64</SpaceEngineersGameBin>
  </PropertyGroup>
</Project>
```
