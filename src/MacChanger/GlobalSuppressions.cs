// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Exceptions", "TI8110:Do not silently ignore exceptions", Justification = "If there is no adapter with that name, just return", Scope = "member", Target = "~M:MacChanger.Adapter.#ctor(System.String)")]
