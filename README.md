# GHGlobalsVars

A set of Rhino - Grasshopper components for using key:value global variables locally within a GH definition. Plugin components:
- Global var 'setter'; for setting variables from the global dictionary.
- Global var 'getter'; for getting variables from the global dictionary by key.
- Global var 'viewer'; for viewing all available key:value pairs within the global dictionary.
- Global var 'cleaner': for clearing all values within the global dictionary.

## Stack

The package is written in C# and built for .NET 7.0 to run within Rhino 8. 

## Setup and Usage

The .gha file within the root of this repo is ready to be copied into the Grasshopper 'Components Folder' (Grasshopper < File < Special Folders < Components Folder) as is. The source code lives within the GHGlobalVars folder and may be cloned for further adaptation. 

## Future

A number of improvements are planned for the plugin:
- Implementation of thorough testing
- Addition of simple and clear component icons

