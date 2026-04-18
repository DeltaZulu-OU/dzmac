// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Object lifecycle", "TI5113:Implement IDisposable if a class uses unmanaged resources, owns disposable objects or subscribes to other objects", Justification = "Ignore IDisposable in test classes", Scope = "type", Target = "~T:DZMACLib.Tests.VendorListTests")]